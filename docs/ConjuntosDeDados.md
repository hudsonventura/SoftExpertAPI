# Conjuntos de dados (SoftExpert)

Esta biblioteca consulta conjuntos de dados via REST:

`POST /apigateway/v1/dataset-integration/{id}`

O body da requisição é um JSON com os parâmetros documentados em cada conjunto abaixo. É necessário criar cada conjunto no SoftExpert com o **ID exatamente igual** ao indicado e a **SQL correspondente**.

Requer `token` válido em `Configurations.token`.

---

## queryGetAttachmentFile

**Funções que utilizam:** `ListAttachmentFromInstance`, `GetFileFromOID`

**Parâmetros JSON:** `WorkflowID`, `OID`, `CDFILE`

**SQL:**

```sql
select 1 AS TYPE --1 FORM, 2 ANEXO DE INSTANCIA
, NULL AS IDSTRUCT
, seblob.NMNAME AS NMFILE
, EFFILE.CDFILE
, NULL AS CDATTACHMENT
, oid
from softexpert.seblob
LEFT JOIN softexpert.EFFILE ON SEBLOB.CDEFFILE = EFFILE.CDEFFILE
where 1=1
AND (:OID is null or oid = :OID)
AND (:CDFILE is null or effile.cdfile = :CDFILE)
AND (:WorkflowID is null)
       --
       UNION
       --                    
select 2 AS TYPE --1 FORM, 2 ANEXO DE INSTANCIA
, a.idstruct
, g.NMFILE
, g.CDFILE
, ANEXO.CDATTACHMENT
, NULL AS oid
--
from softexpert.wfprocess p
JOIN softexpert.WFSTRUCT A ON A.IDPROCESS = P.IDOBJECT
JOIN softexpert.WFPROCATTACHMENT ATAASSOC ON A.IDOBJECT = ATAASSOC.IDSTRUCT
JOIN softexpert.ADATTACHMENT ANEXO ON ATAASSOC.CDATTACHMENT = ANEXO.CDATTACHMENT
join softexpert.ADATTACHFILE attach on ANEXO.CDATTACHMENT = attach.CDATTACHMENT
join softexpert.GNCOMPFILECONTCOPY c on attach.CDCOMPLEXFILECONT = c.CDCOMPLEXFILECONT
join softexpert.gnfile g on c.CDCOMPLEXFILECONT = g.CDCOMPLEXFILECONT
--
where ANEXO.CDATTACHMENT IS NOT NULL 
AND (:WorkflowID is null or p.idprocess = :WorkflowID)
AND (:CDFILE is null or g.cdfile = :CDFILE)
```

---

## queryGetWorkflowInstanceData

**Funções que utilizam:** `GetWorflowStatus`, `reactivateWorkflow`, `returnWorkflow`, `delegateWorkflow`

**Parâmetros JSON:** `workflowID`

**SQL:**

```sql
select p.idprocess
, p.IDOBJECT
, P.FGSTATUS
, p.cduserstart
, p.nmprocess
, p.cdprocessmodel
, p.idprocessmodel
, p.nmprocessmodel
, p.idrevision
--
, p.dtstart
, p.tmstart
, dhstart
--
, p.dtfinish
, p.tmfinish
, dhfinish
--
, gnf.OIDENTITYREG
from softexpert.WFPROCESS p
JOIN softexpert.GNASSOCFORMREG GNF on p.cdassocreg = GNF.cdassoc
where p.IDPROCESS = :workflowID
```

---

## queryGetActivitiesFromInstance

**Funções que utilizam:** `GetActivitiesFromInstance`, `GetCurrentActivities`, `reactivateWorkflow`, `returnWorkflow`, `finishWorkflow`, `delegateWorkflow`

**Parâmetros JSON:** `WorkflowID`

**SQL:**

```sql
SELECT a.idprocess, a.idobject, a.idstruct, a.nmstruct, a.fgstatus
    , A.DHENABLED AS DTENABLED
    , a.DTESTIMATEDFINISH + ( A.NRTIMEESTFINISH/24/60) AS DTESTIMATEDFINISH
    , TO_DATE(to_char(a.DTEXECUTION, 'dd/mm/yyyy') || a.TMEXECUTION, 'dd/mm/yyyyHH24:MI:SS') AS DTEXECUTION
FROM softexpert.wfprocess p
JOIN softexpert.wfstruct a on a.idprocess = p.idobject
WHERE p.idprocess = :WorkflowID
```

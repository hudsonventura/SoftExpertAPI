using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Domain;
using Newtonsoft.Json;


namespace SoftExpertAPI;

public class SoftExpertWorkflowApi : SoftExpertBaseAPI
{
    public SoftExpertWorkflowApi(Configurations configs) : base(configs)
    {
    }


    protected override void SetUriModule()
    {
        _uriModule = "/apigateway/se/ws/wf_ws.php";
    }



    


    /// <summary>
    /// Este método cria uma nova instância de processo de Workflow
    /// </summary>
    /// <param name="ProcessID">ID do processo</param>
    /// <param name="WorkflowTitle">Titulo da instância</param>
    /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
    /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public string newWorkflow(string ProcessID, string WorkflowTitle, string? UserID = null)
    {
        //BUG: dependendo dos caracteres do WorkflowTitle, a instancia não pode ser criada.
        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:newWorkflow>
                                    <urn:ProcessID>{ProcessID}</urn:ProcessID>
                                    <urn:WorkflowTitle>{WorkflowTitle}</urn:WorkflowTitle>
                                    <urn:UserID>{UserID}</urn:UserID>
                                </urn:newWorkflow>
                            </soapenv:Body>
                        </soapenv:Envelope>"
        ;

        var se_response = SendRequestSOAP("newWorkflow", body);
        return se_response.SelectToken("RecordID").ToString();
    }









    /// <summary>
    /// Este método edita os valores do formulário de uma instância de Workflow. Anexos podem ser adicionados nos campos do formulário
    /// </summary>
    /// <param name="WorkflowID">ID da instância</param>
    /// <param name="EntityID">ID da tabela do formulário</param>
    /// <param name="EntityAttributeList">Dicionário contendo os campos do formulário no formato chave - valor (pode ser nulo)</param>
    /// <param name="RelationshipList">Dicionário contendo os campos do formulário no formato chave - valor dentro de um dicionário com o ID do relacionamento na chave do dicionário superior. (pode ser nulo) </param>
    /// <param name="EntityAttributeFileList">Dicionário contendo os arquivos no formato chave - valor (byte[])</param>
    /// <returns>editEntityRecordResponse, objeto com os campos Status, Code, Detail. Se Code = 1 entao houve sucesso. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public void editEntityRecord(string WorkflowID, string EntityID, Dictionary<string, string> EntityAttributeList = null, Dictionary<string, Dictionary<string, string>> RelationshipList = null, Dictionary<string, Anexo> EntityAttributeFileList = null)
    {
        string camposForm = Gerar_EntityAttributeList(EntityAttributeList);
        string camposRelacionamento = Gerar_RelationshipList(RelationshipList);
        string anexos = Gerar_EntityAttributeFileList(EntityAttributeFileList);
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:editEntityRecord>
                         <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                         <urn:EntityID>{EntityID}</urn:EntityID>

                         <urn:EntityAttributeList>
                            {camposForm}
                         </urn:EntityAttributeList>

                         <urn:RelationshipList>
                            {camposRelacionamento}
                         </urn:RelationshipList>

                         <EntityAttributeFileList>
                            {anexos}
                        </EntityAttributeFileList> 

                      </urn:editEntityRecord>
                   </soapenv:Body>
                </soapenv:Envelope>";

        SendRequestSOAP("editEntityRecord", body);
    }

    private string Gerar_EntityAttributeFileList(Dictionary<string, Anexo> EntityAttributeFileList)
    {
        string anexos = String.Empty;
        if (EntityAttributeFileList is not null)
        {
            foreach (var arquivo in EntityAttributeFileList)
            {
                string base64 = Convert.ToBase64String(arquivo.Value.Content);
                anexos += $@"
                        <EntityAttributeFile>
                            <EntityAttributeID>{arquivo.Key}</EntityAttributeID>
                            <FileName>{arquivo.Value.FileName}</FileName>
                            <FileContent>{base64}</FileContent>
                        </EntityAttributeFile>
                    ";
            }
        }
        return anexos;
    }

    private string Gerar_RelationshipList(Dictionary<string, Dictionary<string, string>> RelationshipList)
    {
        string camposRelacionamento = String.Empty;
        if (RelationshipList is not null)
        {
            camposRelacionamento += $@"";
            foreach (KeyValuePair<string, Dictionary<string, string>> RelationshipAttribute in RelationshipList)
            {
                camposRelacionamento += $@"
                             <Relationship>
                                     <RelationshipID>{RelationshipAttribute.Key}</RelationshipID>
                            "
                ;

                foreach (KeyValuePair<string, string> Attribute in RelationshipAttribute.Value)
                {
                    camposRelacionamento += $@"
                                     <RelationshipAttribute>
                                             <RelationshipAttributeID>{Attribute.Key}</RelationshipAttributeID>
                                             <RelationshipAttributeValue>{Attribute.Value}</RelationshipAttributeValue>
                                     </RelationshipAttribute>
                            "
                    ;
                }
                camposRelacionamento += $@"
                             </Relationship>
                            "
                ;
            }
        }
        return camposRelacionamento;
    }

    private string Gerar_EntityAttributeList(Dictionary<string, string> EntityAttributeList) {
        string camposForm = String.Empty;
        if (EntityAttributeList is not null)
        {
            foreach (KeyValuePair<string, string> keyValues in EntityAttributeList)
            {
                camposForm += $@"
                            <urn:EntityAttribute>
                                <EntityAttributeID>{keyValues.Key}</EntityAttributeID>            
                                <EntityAttributeValue>{keyValues.Value}</EntityAttributeValue>
                            </urn:EntityAttribute>"
                ;
            }
        }
        return camposForm;
    }








    /// <summary>
    /// Este método executa uma atividade de usuário de uma instância
    /// </summary>
    /// <param name="WorkflowID">ID da instancia</param>
    /// <param name="ActivityID">ID da atividade a ser executada</param>
    /// <param name="ActionSequence">ID / Sequência da ação da atividade. </param>
    /// <param name="UserID">Matrícula do usuario executor</param>
    /// <param name="ActivityOrder"></param>
    /// <returns>executeActivityResponse, objeto com os campos Status, Code, Detail. Se Code = 1 entao houve sucesso. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public void executeActivity(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        try
        {
            executeActivityWorkflowTry(WorkflowID, ActivityID, ActionSequence, UserID, ActivityOrder);
            return;
        }
        catch (SoftExpertException errorWF)
        {
            if(errorWF.Code == -20){
                throw;
            }
            try
            {
                executeActivityProblemTry(WorkflowID, ActivityID, ActionSequence, UserID, ActivityOrder);
                return;
            }
            catch (SoftExpertException errorPB)
            {
                if(errorPB.Code == -20){
                    throw;
                }
                try
                {
                    executeActivityIncidentTry(WorkflowID, ActivityID, ActionSequence, UserID, ActivityOrder);
                    return;
                }
                catch (SoftExpertException errorIn)
                {
                    if(errorIn.Code == -20){
                        throw;
                    }
                }
            }
            throw;
        }
        catch (System.Exception){
            throw;
        }
    }

    private void executeActivityWorkflowTry(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:executeActivity>
                         <!--You may enter the following 5 items in any order-->
                         <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:ActionSequence>{ActionSequence}</urn:ActionSequence>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                         <!--Optional:-->
                         <urn:ActivityOrder>{ActivityOrder}</urn:ActivityOrder>
                      </urn:executeActivity>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("executeActivity", body);
    }

    private void executeActivityProblemTry(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:executeActivity>
                         <!--You may enter the following 5 items in any order-->
                         <urn:ProblemID>{WorkflowID}</urn:ProblemID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:ActionSequence>{ActionSequence}</urn:ActionSequence>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                         <!--Optional:-->
                         <urn:ActivityOrder>{ActivityOrder}</urn:ActivityOrder>
                      </urn:executeActivity>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("executeActivity", body, "/apigateway/se/ws/pb_ws.php");
    }

    private void executeActivityIncidentTry(string WorkflowID, string ActivityID, int ActionSequence, string? UserID = null, int? ActivityOrder = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:executeActivity>
                         <!--You may enter the following 5 items in any order-->
                         <urn:IncidentID>{WorkflowID}</urn:IncidentID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:ActionSequence>{ActionSequence}</urn:ActionSequence>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                         <!--Optional:-->
                         <urn:ActivityOrder>{ActivityOrder}</urn:ActivityOrder>
                      </urn:executeActivity>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("executeActivity", body, "/apigateway/se/ws/in_ws.php");
    }









    /// <summary>
    /// Este método anexa um arquivo no menu de anexo do lado esquerdo de uma instancia
    /// </summary>
    /// <param name="WorkflowID">ID da instancia</param>
    /// <param name="ActivityID">ID da atividade a ser executada</param>
    /// <param name="File">Arquivo a ser anexado</param>
    /// <returns></returns>
    public int newAttachment(string WorkflowID, string ActivityID, Anexo File, string UserID = null)
    {
        if (File is null)
        {
            throw new SoftExpertException("Um dos itens da lista é nulo. Então a comunicação com o SE não foi inciada. Verifique sua lista de arquivos e tente novamente.");
        }
        if (File.Content is null || File.Content.Length == 0)
        {
            throw new SoftExpertException("Um dos itens da lista possui o conteúdo nulo ou vazio. Então a comunicação com o SE não foi inciada. Verifique sua lista de arquivos e tente novamente.");
        }
        if (File.FileName is null || File.FileName.Length == 0)
        {
            throw new SoftExpertException("Um dos itens da lista não possui o nome do arquivo (FileName), ou este é vazio. Então a comunicação com o SE não foi inciada. Verifique sua lista de arquivos e tente novamente.");
        }

        string base64 = Convert.ToBase64String(File.Content);
        string body = $@"
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:workflow"">
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:newAttachment>
                         <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                         <urn:ActivityID>{ActivityID}</urn:ActivityID>
                         <urn:FileName>{File.FileName}</urn:FileName>
                         <urn:FileContent>{base64}</urn:FileContent>
                         <!--Optional:-->
                         <urn:UserID>{UserID}</urn:UserID>
                      </urn:newAttachment>
                   </soapenv:Body>
                </soapenv:Envelope>";

        var se_response = SendRequestSOAP("newAttachment", body);
        return Int32.Parse(se_response.SelectToken("RecordKey").ToString());
    }








    /// <summary>
    /// Este método permite você criar itens de uma grid de um formulário principal
    /// </summary>
    /// <param name="ProcessID">ID do processo</param>
    /// <param name="WorkflowTitle">Titulo da instância</param>
    /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
    /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public void newChildEntityRecord(string WorkflowID, string MainEntityID, string ChildRelationshipID, Dictionary<string, string> EntityAttributeList = null, Dictionary<string, Dictionary<string, string>> RelationshipList = null, Dictionary<string, Anexo> EntityAttributeFileList = null)
    {
        string camposForm = Gerar_EntityAttributeList(EntityAttributeList);
        string camposRelacionamento = Gerar_RelationshipList(RelationshipList);
        string anexos = Gerar_EntityAttributeFileList(EntityAttributeFileList);
        string body = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:workflow"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <urn:newChildEntityRecord>
                                        <!--You may enter the following 6 items in any order-->
                                        <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                                        <urn:MainEntityID>{MainEntityID}</urn:MainEntityID>
                                        <urn:ChildRelationshipID>{ChildRelationshipID}</urn:ChildRelationshipID>
                                        
                                        <urn:EntityAttributeList>
                                            {camposForm}
                                        </urn:EntityAttributeList>

                                        <urn:RelationshipList>
                                            {camposRelacionamento}
                                        </urn:RelationshipList>

                                        <EntityAttributeFileList>
                                            {anexos}
                                        </EntityAttributeFileList>
                                        
                                    </urn:newChildEntityRecord>
                                </soapenv:Body>
                            </soapenv:Envelope>";

        SendRequestSOAP("newChildEntityRecord", body);
    }






    /// <summary>
    /// Traz os arquivos anexados do lado esquerdo de uma instancia. Pode especificar a atividade em que o arquivo fora anexado. Obrigatório implementar o parametro _db.
    /// </summary>
    /// <param name="WorkflowID">ID da instância</param>
    /// <param name="ActivityID">ID da atividade (opcional)</param>
    /// <returns>Lista de objetos da classe Anexo, contendo nome do arquivo, cdfile e conteúdo em byte[]</returns>
    /// <exception cref="SoftExpertException"></exception>
    /// <exception cref="Exception"></exception>
    public List<Anexo> ListAttachmentFromInstance(string WorkflowID, string ActivityID = "")
    {
        requireInterfaceImplementation("IFileDownloader", _downloader);

        var files = QueryAttachmentFiles(workflowID: WorkflowID);
        if (files == null || files.Count == 0)
        {
            return new List<Anexo>();
        }

        if (!string.IsNullOrWhiteSpace(ActivityID))
        {
            files = files.Where(f => f.idstruct == ActivityID).ToList();
        }

        var retorno = new List<Anexo>();
        foreach (var file in files)
        {
            retorno.Add(DownloadAttachment(file));
        }
        return retorno;
    }









    /// <summary>
    /// Obter um arquivo de um OID obtível de um campo de anexo do formulário
    /// </summary>
    /// <param name="oid"></param>
    /// <returns></returns>
    /// <exception cref="SoftExpertException"></exception>
    public Anexo GetFileFromOID(string oid)
    {
        requireInterfaceImplementation("IFileDownloader", _downloader);

        var files = QueryAttachmentFiles(oid: oid);
        var file = files?.FirstOrDefault();
        if (file == null || file.cdfile is null)
        {
            throw new SoftExpertException($"O oid '{oid}' não foi encontrado");
        }

        return DownloadAttachment(file);
    }







    /// <summary>
    /// Este método permite você EDITAR itens de uma grid de um formulário principal
    /// </summary>
    /// <param name="workflowID"></param>
    /// <param name="mainEntityID"></param>
    /// <param name="childRelationshipID"></param>
    /// <param name="childRecordOID"></param>
    /// <param name="formulario"></param>
    /// <returns></returns>
    public void editChildEntityRecord(string WorkflowID, string MainEntityID, string ChildRelationshipID, string childRecordOID, Dictionary<string, string> EntityAttributeList, Dictionary<string, Anexo>  EntityAttributeFileList = null)
    {
        string camposForm = Gerar_EntityAttributeList(EntityAttributeList);
        string anexos = Gerar_EntityAttributeFileList(EntityAttributeFileList);
        string body = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:workflow"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <urn:editChildEntityRecord>
                                        <!--You may enter the following 6 items in any order-->
                                        <urn:WorkflowID>{WorkflowID}</urn:WorkflowID>
                                        <urn:MainEntityID>{MainEntityID}</urn:MainEntityID>
                                        <urn:ChildRelationshipID>{ChildRelationshipID}</urn:ChildRelationshipID>
                                        <urn:ChildRecordOID>{childRecordOID}</urn:ChildRecordOID>

                                        <urn:EntityAttributeList>
                                            {camposForm}
                                        </urn:EntityAttributeList>


                                        <EntityAttributeFileList>
                                            {anexos}
                                        </EntityAttributeFileList>
                                        
                                    </urn:editChildEntityRecord>
                                </soapenv:Body>
                            </soapenv:Envelope>";
        
        SendRequestSOAP("editChildEntityRecord", body);
    }




    private void requireInterfaceImplementation(string type, dynamic obj)
    {
        if(obj == null){
            throw new SoftExpertException($"Objeto do tipo {type} é nulo ou ausente, não foi implementado ou nao foi iniciado corretamente. Veja a documentação.");
        }
    }

    /*
        Criar um conjunto de dados com o ID 'queryGetAttachmentFile' no SE com o SQL abaixo

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
    */
    private List<AttachmentFileObject> QueryAttachmentFiles(string workflowID = null, string oid = null, long? cdfile = null)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/apigateway/v1/dataset-integration/queryGetAttachmentFile");

        var payload = new Dictionary<string, string>
        {
            { "WorkflowID", workflowID ?? string.Empty },
            { "OID", oid ?? string.Empty },
            { "CDFILE", cdfile?.ToString() ?? string.Empty },
        };

        string jsonBody = JsonConvert.SerializeObject(payload);
        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        HttpResponseMessage response = _dataSetClient.SendAsync(request).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new SoftExpertException("Houve um problema ao consultar os arquivos no SoftExpert");
        }

        string responseBody = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<List<AttachmentFileObject>>(responseBody) ?? new List<AttachmentFileObject>();
    }

    private Anexo DownloadAttachment(AttachmentFileObject file)
    {
        if (file.cdfile is null)
        {
            throw new SoftExpertException($"Arquivo '{file.nmfile}' sem CDFILE");
        }

        var anexo = new Anexo
        {
            FileName = file.nmfile,
            cdfile = file.cdfile.Value,
            cdattachment = file.cdattachment ?? 0,
            extension = file.Extension,
        };

        string storedName = $"{anexo.cdfile.ToString($"D{8}")}.{anexo.extension}";
        anexo.Content = file.IsFormFile
            ? _downloader.DownloadFileForm(storedName)
            : _downloader.DownloadFileAttach(storedName);

        return anexo;
    }









    /// <summary>
    /// Retorna os dados da instância de workflow (incluindo status) via dataset SoftExpert
    /// </summary>
    /// <param name="WorkflowID">IDPROCESS da instância</param>
    /// <returns>ManageInstanceObject com os dados da instância</returns>
    public WFStruct.WFStatus GetWorflowStatus(string WorkflowID)
    {
        var obj = GetWorkflowInstanceData(WorkflowID);
        if (obj == null)
        {
            throw new SoftExpertException($"Não foi encontrado um workflow com o id '{WorkflowID}'");
        }

        if (!Enum.IsDefined(typeof(WFStruct.WFStatus), obj.p_fgstatus))
        {
            throw new SoftExpertException($"Valor desconhecido para fgstatus: {obj.p_fgstatus}");
        }

        return obj.Status;
    }






    /// <summary>
    /// Retorna a lista de atividades em execução de uma instância via dataset SoftExpert
    /// </summary>
    /// <param name="WorkflowID">IDPROCESS da instância</param>
    /// <returns>Lista de WFStruct das atividades atuais</returns>
    public List<WFStruct> GetCurrentActivities(string WorkflowID)
    {
        return GetActivitiesFromWorkflow(WorkflowID).Where(item => item.fgstatus == WFStruct.WFStatus.Em_Andamento).ToList();
    }



    /// <summary>
    /// Retorna a lista de atividades de uma instancia, não importando o status
    /// </summary>
    /// <param name="WorkflowID">IDPROCESS da instância</param>
    /// <returns>Lista de WFStruct das atividades atuais</returns>
    public List<WFStruct> GetActivitiesFromWorkflow(string WorkflowID)
    {
        /*
            Criar um conjunto de dados com o ID 'queryGetCurrentActivities' no SE com o SQL abaixo

            SELECT a.idprocess, a.idobject, a.idstruct, a.nmstruct, a.fgstatus
                    , A.DHENABLED AS DTENABLED
                    , a.DTESTIMATEDFINISH + ( A.NRTIMEESTFINISH/24/60) AS DTESTIMATEDFINISH
                    , TO_DATE(to_char(a.DTEXECUTION, 'dd/mm/yyyy') || a.TMEXECUTION, 'dd/mm/yyyyHH24:MI:SS') AS DTEXECUTION
                FROM softexpert.wfprocess p
                JOIN softexpert.wfstruct a on a.idprocess = p.idobject AND A.FGSTATUS = 2
                WHERE p.idprocess = :WorkflowID
        */

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/apigateway/v1/dataset-integration/queryGetCurrentActivities");

        var payload = new Dictionary<string, string>
        {
            { "WorkflowID", WorkflowID }
        };
        string jsonBody = JsonConvert.SerializeObject(payload);
        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        HttpResponseMessage response = _dataSetClient.SendAsync(request).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new SoftExpertException($"Houve um problema ao consultar as atividades da instância '{WorkflowID}'");
        }

        string responseBody = response.Content.ReadAsStringAsync().Result;
        var list = JsonConvert.DeserializeObject<List<CurrentActivityObject>>(responseBody);

        if (list == null || list.Count == 0)
        {
            throw new SoftExpertException($"Não foi encontrado um workflow com o id '{WorkflowID}'");
        }

        return list.Select(item => item.ToWFStruct()).ToList();
    }










    /// <summary>
    /// Cancela um fluxo de workflow, incidente ou problema
    /// </summary>
    /// <param name="workflowID">ID da instancia de workflow, incidente ou problema</param>
    /// <param name="explanation">Justificativa</param>
    /// <param name="userID">Matricula do usuario que está cancelando. Ele precisa ter permissão na segurança para cancelar</param>
    public void cancelWorkflow(string workflowID, string explanation, string userID = null)
    {
        try
        {
            cancelWorkflowTry(workflowID, explanation, userID);
            return;
        }
        catch (System.Exception errorWF)
        {
            try
            {
                cancelProblemTry(workflowID, explanation, userID);
                return;
            }
            catch (System.Exception)
            {
                 try
                {
                    cancelIncidentTry(workflowID, explanation, userID);
                    return;
                }
                catch (System.Exception)
                {
                    
                }
            }
            throw;
        }
    }


    private void cancelWorkflowTry(string workflowID, string explanation, string userID = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:cancelWorkflow>
                         <urn:WorkflowID>{workflowID}</urn:WorkflowID>
                         <urn:Explanation>{explanation}</urn:Explanation>
                         <urn:UserID>{userID}</urn:UserID>
                      </urn:cancelWorkflow>
                   </soapenv:Body>
                </soapenv:Envelope>";
        SendRequestSOAP("cancelWorkflow", body);
    }

    private void cancelProblemTry(string workflowID, string explanation, string userID = null)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:cancelProblem>
                         <urn:ProblemID>{workflowID}</urn:ProblemID>
                         <urn:Explanation>{explanation}</urn:Explanation>
                         <urn:UserID>{userID}</urn:UserID>
                      </urn:cancelProblem>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("cancelProblem", body, "/apigateway/se/ws/pb_ws.php");
    }

    private void cancelIncidentTry(string workflowID, string explanation, string userID)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:cancelIncident>
                         <urn:IncidentID>{workflowID}</urn:IncidentID>
                         <urn:Explanation>{explanation}</urn:Explanation>
                         <urn:UserID>{userID}</urn:UserID>
                      </urn:cancelIncident>
                   </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("cancelIncident", body, "/apigateway/se/ws/in_ws.php");
    }






    /// <summary>
    /// Adiciona um comentário no histório de uma instancia de WorkFlow
    /// </summary>
    /// <param name="workflowID">ID da instancia de workflow, incidente ou problema</param>
    /// <param name="explanation">Justificativa</param>
    /// <param name="iduser">Matricula do usuario </param>
    public void addHistoryComment(string workflowID, string comment, int iduser, string idactivity, bool is_private = false){
        addHistoryComment(workflowID, comment, iduser.ToString(), idactivity, is_private);
    }

    /// <summary>
    /// Adiciona um comentário no histório de uma instancia de WorkFlow
    /// </summary>
    /// <param name="workflowID">ID da instancia de workflow, incidente ou problema</param>
    /// <param name="explanation">Justificativa</param>
    /// <param name="userID">Matricula do usuario</param>
    public void addHistoryComment(string workflowID, string comment, string userID, string idactivity, bool is_private = false)
    {
        string activity = $"<urn:ActivityID>{idactivity}</urn:ActivityID>";


        string body = $@"
                        <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                        <soapenv:Header/>
                        <soapenv:Body>
                            <urn:newComment>
                                <urn:WorkflowID>{workflowID}</urn:WorkflowID>
                                {activity}
                                <urn:Text>{comment.Replace("&", "e")}</urn:Text>
                                <urn:Private>{((is_private) ? 1 : 0)}</urn:Private>
                                <urn:UserID>{userID}</urn:UserID>
                            </urn:newComment>
                        </soapenv:Body>
                        </soapenv:Envelope>";

        SendRequestSOAP("newComment", body, "/apigateway/se/ws/wf_ws.php");
    }

    



    /// <summary>
    /// Desassocia uma atividade de um usuário e devolve para o papel funcional ou equipe
    /// </summary>
    /// <param name="workflowID"></param>
    /// <param name="ActivityID"></param>
    public void unlinkActivityFromUser(string workflowID, string ActivityID)
    {
        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                <soapenv:Header/>
                <soapenv:Body>
                    <urn:unlinkActivityFromUser>
                        <urn:WorkflowID>{workflowID}</urn:WorkflowID>
                        <urn:ActivityID>{ActivityID}</urn:ActivityID>
                    </urn:unlinkActivityFromUser>
                </soapenv:Body>
                </soapenv:Envelope>";
        
        SendRequestSOAP("unlinkActivityFromUser", body);
    }





    /// <summary>
    /// Reativa uma instância de processo
    /// </summary>
    /// <param name="workflowID"></param>
    /// <param name="ActivityID"></param>
    /// <param name="explanation"></param>
    /// <param name="userID"></param>
    public void reactivateWorkflow(string workflowID, string ActivityID, string explanation, string userID)
    {
        //Obs.: reactivateWorkflow original nõa permite reativar instancia cancelada. Então pq existe?
        try
        {
            var obj = GetWorkflowInstanceData(workflowID);
            if(obj == null){
                throw new Exception($"Não foi encontrada nenhuma instância de workflow com o ID '{workflowID}'");
            }

            var activities = GetActivitiesFromWorkflow(workflowID);
            var activity = activities.FirstOrDefault(a => a.idstruct == ActivityID) ?? activities.FirstOrDefault();
            if(activity == null){
                throw new Exception($"Não foi encontrada nenhuma atividade na instância '{workflowID}'");
            }

            Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(){
                {"oid", obj.p_idobject},
                {"action", 2},
            };
            string query = string.Join("&", parametros.Select(p => $"{p.Key}={p.Value}"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/se/v16780/workflow/wf_management/management_action.php?{query}");

            string token = GetToken();
            request.Headers.Add("Cookie", $"se-authentication-token={token}");



            var payload = new Dictionary<string, string>
            {
                { "fgstatus", "1" },
                { "cditemreturn", activity.idobject },
                { "justify", SanitizeString(explanation) }
            };
            string jsonBody = JsonConvert.SerializeObject(payload);

            request.Content = new FormUrlEncodedContent(payload);
            request.Content.Headers.ContentType.CharSet = "UTF-8";
            //request.Headers.Add("Content-Type", "text/xml; charset=iso-8859-1");


            HttpResponseMessage  response = _restClient.SendAsync(request).Result;
            if(!response.IsSuccessStatusCode){
                throw new Exception("Houve um problema ao reativar a instancia");
            }

            string responseBody = response.Content.ReadAsStringAsync().Result;
            if(responseBody.Contains("softexpert/login")){
                throw new Exception("Houve um problema ao reativar a instancia");
            }

            return;
        }
        catch (System.Exception errorWF)
        {
            throw;
        }
    }



    private ManageInstanceObject GetWorkflowInstanceData(string workflowID)
    {
        /*
            Criar um conjunto de dados com o ID 'queryGetWorkflowInstanceData' no SE com o SQL abaixo

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
        */

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/apigateway/v1/dataset-integration/queryGetWorkflowInstanceData");

        var payload = new Dictionary<string, string>
        {
            { "workflowID", workflowID }
        };

        string jsonBody = JsonConvert.SerializeObject(payload);
        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");


        HttpResponseMessage  response = _dataSetClient.SendAsync(request).Result;
        if(!response.IsSuccessStatusCode){
            throw new SoftExpertException("Houve um problema ao consultar a instancia");
        }

        string responseBody = response.Content.ReadAsStringAsync().Result;
        var list = JsonConvert.DeserializeObject<List<ManageInstanceObject>>(responseBody);

        if (list == null || list.Count == 0)
        {
            return null;
        }

        return list[0];
    }   






    /// <summary>
    /// Retorna uma instância de processo para uma atividade específica
    /// </summary>
    /// <param name="workflowID"></param>
    /// <param name="ActivityID"></param>
    /// <param name="explanation"></param>
    /// <param name="userID"></param>
    public void returnWorkflow(string workflowID, string ActivityID, string explanation, string userID)
    {
        try
        {
            var obj = GetWorkflowInstanceData(workflowID);
            if(obj == null){
                throw new Exception($"Não foi encontrada nenhuma instância de workflow com o ID '{workflowID}'");
            }


            var activities = GetCurrentActivities(workflowID);
            var activity = activities.FirstOrDefault(a => a.idstruct == ActivityID) ?? activities.FirstOrDefault();
            if(activity == null){
                throw new Exception($"Não foi encontrada nenhuma atividade na instância '{workflowID}'");
            }

            Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(){
                {"idobject", activity.idobject},
                {"idprocess", obj.p_idobject}
            };
            string query = string.Join("&", parametros.Select(p => $"{p.Key}={p.Value}"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/se/v16780/workflow/wf_management/management_activity_cancel.php?{query}");

            string token = GetToken();
            request.Headers.Add("Cookie", $"se-authentication-token={token}");

            var payload = new Dictionary<string, string>
            {
                { "dscomment", explanation }
            };
            string jsonBody = JsonConvert.SerializeObject(payload);
            request.Content = new FormUrlEncodedContent(payload);


            HttpResponseMessage  response = _restClient.SendAsync(request).Result;
            if(!response.IsSuccessStatusCode){
                throw new SoftExpertException("Houve um problema ao reativar a instancia");
            }

            string responseBody = response.Content.ReadAsStringAsync().Result;
            if(responseBody.Contains("softexpert/login")){
                throw new SoftExpertException("Houve um problema ao retornar a instancia");
            }

            if(responseBody.Contains("Ocorreu um erro ao tentar processar informações")){
                throw new SoftExpertException("Houve um problema ao retornar a instancia");
            }

            return;
        }
        catch (System.Exception errorWF)
        {
            throw;
        }
    } 







    /// <summary>
    /// Encerra uma instância de processo mesmo sem chegar ao final do processo
    /// </summary>
    /// <param name="workflowID"></param>
    /// <param name="ActivityID"></param>
    /// <param name="explanation"></param>
    /// <param name="userID"></param>
    public void finishWorkflow(string workflowID, string explanation, string userID)
    {
        try
        {
            var activity = GetCurrentActivities(workflowID).FirstOrDefault();
            if(activity == null){
                throw new Exception($"Não foi encontrada nenhuma instância de workflow com o ID '{workflowID}' e que possua ao menos uma atividade");
            }
            Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(){
                {"oid", activity.idprocess},
                {"caption", "Gest%25E3o%2Bde%2Bworkflow"},
                {"action", 2},
                {"type", 3}
            };
            string query = string.Join("&", parametros.Select(p => $"{p.Key}={p.Value}"));

            IEnumerable<KeyValuePair<string, string>> formdata = new Dictionary<string, dynamic>(){
                {"oid_proc", activity.idprocess},
                {"cdProd", 39},
                {"idprocess", workflowID},
                {"sit", "Andamento"},
                {"fgstatus", "4"},
                {"idrevisionstatus", string.Empty},
                {"hidden_field_to_reset_name_nmrevisionstatus", string.Empty},
                {"cdrevisionstatus", string.Empty},
                {"nmrevisionstatus", string.Empty},
                {"justify", explanation},
                {"elms_filters", string.Empty},
                {"elms_allfilters", "{\"100528\":{\"id\":\"idprocess\",\"value\":\"\",\"tokens\":[],\"type\":\"text\"},\"100532\":{\"id\":\"sit\",\"value\":\"\",\"tokens\":[],\"type\":\"text\"},\"100609\":{\"id\":\"fgfinish\",\"value\":\"\",\"tokens\":[],\"type\":\"checkbox\"},\"106951\":{\"id\":\"fgsusp\",\"value\":\"\",\"tokens\":[],\"type\":\"checkbox\"},\"100095\":{\"id\":\"fgcancel\",\"value\":\"\",\"tokens\":[],\"type\":\"checkbox\"},\"105933\":{\"id\":\"fgreativa\",\"value\":\"\",\"tokens\":[],\"type\":\"checkbox\"},\"100072\":{\"id\":\"hidden_field_to_reset_name_nmrevisionstatus\",\"value\":\"\",\"tokens\":[],\"type\":\"text\"},\"101072\":{\"id\":\"justify\",\"value\":\"\",\"tokens\":[],\"type\":\"text\"},\"207026\":{\"id\":\"bc_quick_filter\",\"value\":\"\",\"tokens\":[],\"type\":\"text\"},\"100366\":{\"id\":\"selectedTypeInput\",\"value\":\"\",\"tokens\":[],\"type\":\"text\"},\"100549\":{\"id\":\"selectedAttributesInput_json\",\"value\":\"\",\"tokens\":[],\"type\":\"text\"}}"}
            }.Select(p => new KeyValuePair<string, string>(p.Key, p.Value.ToString()));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/se/v34445/workflow/wf_management/management_action.php?{query}")
            {
                Content = new FormUrlEncodedContent(formdata)
            };

            string token = GetToken();
            request.Headers.Add("Cookie", $"se-authentication-token={token}");

        
            //request.Headers.Add("content-type", $"application/x-www-form-urlencoded");
            //request.Headers.Add("Referer", "https://seqas.amaggi.com.br/se/v58859/workflow/wf_management/management_data.php");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");	
            request.Headers.Add("cookie", "se-authentication-token=065ba7e059b1c54c785ac4a5e5f69683d300; redirectDomain=IEFNQUdHSQ==");





            HttpResponseMessage  response = _restClient.SendAsync(request).Result;
            if(!response.IsSuccessStatusCode){
                throw new SoftExpertException("Houve um problema ao reativar a instancia");
            }

            string responseBody = response.Content.ReadAsStringAsync().Result;
            if(responseBody.Contains("softexpert/login")){
                throw new SoftExpertException("Houve um problema ao retornar a instancia");
            }

            if(responseBody.Contains("Ocorreu um erro ao tentar processar informações")){
                throw new SoftExpertException("Houve um problema ao retornar a instancia");
            }

            return;
        }
        catch (System.Exception errorWF)
        {
            throw;
        }
    } 







    /// <summary>
    /// Delega uma atividade
    /// </summary>
    /// <param name="workflowID"></param>
    /// <param name="ActivityID"></param>
    /// <param name="explanation"></param>
    /// <param name="userID"></param>
    public void delegateWorkflow(string workflowID, string ActivityID, string explanation, int cduser)
    {
        try
        {
            var obj = GetWorkflowInstanceData(workflowID);
            if(obj == null){
                throw new Exception($"Não foi encontrada nenhuma instância de workflow com o ID '{workflowID}'");
            }

            var activities = GetCurrentActivities(workflowID);
            var activity = activities.FirstOrDefault(a => a.idstruct == ActivityID) ?? activities.FirstOrDefault();
            if(activity == null){
                throw new Exception($"Não foi encontrada nenhuma atividade na instância '{workflowID}'");
            }

            Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>(){
                {"savetype", "activityExecutor"},
                {"idobject", activity.idobject},
                {"idprocess", obj.p_idobject}
            };
            string query = string.Join("&", parametros.Select(p => $"{p.Key}={p.Value}"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"/se/v16780/workflow/wf_gen_instance/wf_gen_instance_executor_action.php?{query}");

            string token = GetToken();
            request.Headers.Add("Cookie", $"se-authentication-token={token}");


            var payload = new Dictionary<string, string>
            {
                { "typeexecutor", "3" },
                { "fgtypeexecutor", "3" },
                { "cduser", cduser.ToString() },
                { "justifActivityExecutor", explanation }
            };
            string jsonBody = JsonConvert.SerializeObject(payload);
            request.Content = new FormUrlEncodedContent(payload);


            HttpResponseMessage  response = _restClient.SendAsync(request).Result;
            if(!response.IsSuccessStatusCode){
                throw new Exception("Houve um problema ao reativar a instancia");
            }

            string responseBody = response.Content.ReadAsStringAsync().Result;
            if(responseBody.Contains("softexpert/login")){
                var error = new SoftExpertException("Houve um problema ao retornar a instancia");
                error.setRequestSent(jsonBody);
                error.setResponseReceived(responseBody);
                throw error;
            }

            if(responseBody.Contains("Ocorreu um erro ao tentar processar informações")){
                throw new Exception("Houve um problema ao retornar a instancia");
            }

            /* se chegou até aqui, então houve sucesso. Sendo assim ...
             * as vezes a tabela wftask fica com o campo FGEXECUTEACTION não nulo.
             * isso faz com que a atividade, mesmo que a atividade esteja ativa, não apareça para o usuário
             * então fazemos um UPDATE wftask SET FGEXECUTEACTION=null forçando que volte a aparecer para o usuário executor
             */

            try{
                string sql = @$"UPDATE softexpert.wftask SET FGEXECUTEACTION=null 
                                WHERE idobject = (
                                    SELECT c.idobject
                                    FROM softexpert.wfprocess p
                                    LEFT JOIN softexpert.wfstruct a on a.idprocess = p.idobject AND A.FGSTATUS = 2
                                    LEFT JOIN softexpert.wftask c on c.IDACTIVITY = a.idobject
                                    WHERE p.idprocess = :workflowID
                                )";
                Dictionary<string, dynamic> params2 = new Dictionary<string, dynamic>();
                params2.Add(":workflowID", workflowID.Trim());

                //int affected = _db.Execute(sql, params2);
                //Desativado para migração para a Cloud
            }
            catch (System.Exception errorWF)
            {
                
            }

            return;
        }
        catch (System.Exception errorWF)
        {
            throw;
        }
    }











    /// <summary>
    /// Altera o iniciador de uma instância de processo via SOAP editWorkflowData
    /// </summary>
    /// <param name="workflowID">IDPROCESS da instância do processo</param>
    /// <param name="explanation">Texto de justificativa (mantido por compatibilidade; não utilizado pelo SOAP)</param>
    /// <param name="userID">Matrícula do novo iniciador</param>
    /// <param name="rename">Mantido por compatibilidade; não utilizado pelo SOAP</param>
    /// <param name="requesterID">Mantido por compatibilidade; não utilizado pelo SOAP</param>
    public void AlterUserStart(string workflowID, string requesterID, string explanation = null)
    {
        ValidateInstance(workflowID.Trim(), WFStruct.WFStatus.Em_Andamento);

        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:editWorkflowData>
                                    <urn:WorkflowID>{workflowID.Trim()}</urn:WorkflowID>
                                    <urn:Requester>
                                        <urn:User>
                                            <urn:UserID>{requesterID}</urn:UserID>
                                        </urn:User>
                                    </urn:Requester>
                                </urn:editWorkflowData>
                            </soapenv:Body>
                        </soapenv:Envelope>";

        SendRequestSOAP("editWorkflowData", body);
    }




    private void ValidateInstance(string workflowID, WFStruct.WFStatus fgstatus)
    {
        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:getWorkflow>
                                    <urn:WorkflowID>{workflowID.Trim()}</urn:WorkflowID>
                                </urn:getWorkflow>
                            </soapenv:Body>
                        </soapenv:Envelope>";

        var se_response = SendRequestSOAP("getWorkflow", body);
        int got_fgstatus = int.Parse(se_response.SelectToken("InstanceStatus").ToString());

        if(got_fgstatus != (int)fgstatus){
             throw new SoftExpertException($"A instância '{workflowID}' foi encontrada mas o status não é {fgstatus}");
        }


        return;
    }



    /// <summary>
    /// Edita um registro de uma tabela do SoftExpert Form via SOAP editTableRecord
    /// </summary>
    /// <param name="UserID">Matrícula do usuário</param>
    /// <param name="TableID">ID da tabela (entidade)</param>
    /// <param name="TableFieldOID">OID do registro a ser editado</param>
    /// <param name="TableFieldList">Campos da tabela no formato chave - valor</param>
    /// <param name="RelationshipList">Relacionamentos (selectbox) opcionais</param>
    /// <param name="TableFieldFileList">Arquivos opcionais para campos da tabela</param>
    public void editTableRecord(
        string UserID,
        string TableID,
        string TableFieldOID,
        Dictionary<string, string> TableFieldList = null,
        Dictionary<string, Dictionary<string, string>> RelationshipList = null,
        Dictionary<string, Anexo> TableFieldFileList = null)
    {
        string camposForm = Gerar_TableFieldList(TableFieldList);
        string camposRelacionamento = Gerar_TableRelationshipList(RelationshipList);
        string anexos = Gerar_TableFieldFileList(TableFieldFileList);

        string body = $@"
                <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:form'>
                   <soapenv:Header/>
                   <soapenv:Body>
                      <urn:editTableRecord>
                         <urn:UserID>{UserID}</urn:UserID>
                         <urn:TableID>{TableID}</urn:TableID>
                         <urn:TableFieldOID>{TableFieldOID}</urn:TableFieldOID>

                         <urn:TableFieldList>
                            {camposForm}
                         </urn:TableFieldList>

                         <urn:RelationshipList>
                            {camposRelacionamento}
                         </urn:RelationshipList>

                         <urn:TableFieldFileList>
                            {anexos}
                         </urn:TableFieldFileList>
                      </urn:editTableRecord>
                   </soapenv:Body>
                </soapenv:Envelope>";

        SendRequestSOAP("editTableRecord", body, "/apigateway/se/ws/fm_ws.php", soapUrn: "form");
    }

    private string Gerar_TableFieldList(Dictionary<string, string> TableFieldList)
    {
        string campos = string.Empty;
        if (TableFieldList is not null)
        {
            foreach (KeyValuePair<string, string> field in TableFieldList)
            {
                campos += $@"
                            <urn:TableField>
                               <urn:TableFieldID>{field.Key}</urn:TableFieldID>
                               <urn:TableFieldValue>{field.Value}</urn:TableFieldValue>
                            </urn:TableField>";
            }
        }
        return campos;
    }

    private string Gerar_TableRelationshipList(Dictionary<string, Dictionary<string, string>> RelationshipList)
    {
        string camposRelacionamento = string.Empty;
        if (RelationshipList is not null)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> relationship in RelationshipList)
            {
                camposRelacionamento += $@"
                             <urn:Relationship>
                                     <urn:RelationshipID>{relationship.Key}</urn:RelationshipID>
                            ";

                foreach (KeyValuePair<string, string> attribute in relationship.Value)
                {
                    camposRelacionamento += $@"
                                     <urn:RelationshipField>
                                             <urn:RelationshipFieldID>{attribute.Key}</urn:RelationshipFieldID>
                                             <urn:RelationshipFieldValue>{attribute.Value}</urn:RelationshipFieldValue>
                                     </urn:RelationshipField>
                            ";
                }

                camposRelacionamento += @"
                             </urn:Relationship>
                            ";
            }
        }
        return camposRelacionamento;
    }

    private string Gerar_TableFieldFileList(Dictionary<string, Anexo> TableFieldFileList)
    {
        string anexos = string.Empty;
        if (TableFieldFileList is not null)
        {
            foreach (var arquivo in TableFieldFileList)
            {
                string base64 = Convert.ToBase64String(arquivo.Value.Content);
                anexos += $@"
                            <urn:TableFieldFile>
                               <urn:TableFieldID>{arquivo.Key}</urn:TableFieldID>
                               <urn:FileName>{arquivo.Value.FileName}</urn:FileName>
                               <urn:FileContent>{base64}</urn:FileContent>
                            </urn:TableFieldFile>
                    ";
            }
        }
        return anexos;
    }
}



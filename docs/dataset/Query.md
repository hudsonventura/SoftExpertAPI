# Query

Consulta um conjunto de dados do SoftExpert via REST `POST /apigateway/v1/dataset-integration/{id_dataset}`.

O parâmetro `query` **não é enviado** na requisição. Serve apenas para, em caso de erro HTTP, informar que o conjunto de dados com o ID `id_dataset` deve ser criado com aquele SQL.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

Conjuntos já usados pela biblioteca: [ConjuntosDeDados.md](../ConjuntosDeDados.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `id_dataset` | `string` | Sim | ID do conjunto de dados no SoftExpert |
| `parameters` | `Dictionary<string, string>` | Não | Binds enviados no body JSON |
| `query` | `string` | Não | SQL do conjunto. Usado somente na mensagem de erro HTTP |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `List<T>` | Linhas retornadas pelo conjunto, desserializadas para `T` |

Em caso de falha HTTP, lança `SoftExpertException` (ou `Exception`, se `query` não for informada) descrevendo o status e, quando `query` foi passada, o SQL que deve ser cadastrado no SoftExpert.

## Exemplo

```csharp
SoftExpertDataSetApi dsAPI = new SoftExpertDataSetApi(configs);

var rows = dsAPI.Query<ManageInstanceObject>(
    "queryGetWorkflowInstanceData",
    new Dictionary<string, string> { { "workflowID", "CCF202614358" } },
    @"select p.idprocess
    from softexpert.WFPROCESS p
    where p.IDPROCESS = :workflowID"
);
```

# GetWorflowStatus

Consulta o status de uma instância de Workflow via dataset SoftExpert.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Conjunto de dados (SoftExpert)

- [queryGetWorkflowInstanceData](../ConjuntosDeDados.md#querygetworkflowinstancedata)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância (`IDPROCESS`) |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `WFStruct.WFStatus` | Status da instância |

Valores possíveis:

| Valor | Código |
| --- | --- |
| `Em_Andamento` | 1 |
| `Suspenso` | 2 |
| `Cancelado` | 3 |
| `Encerrado` | 4 |

## Exemplo

```csharp
WFStruct.WFStatus status = wfAPI.GetWorflowStatus("CCF202614358");
Console.WriteLine(status);
```

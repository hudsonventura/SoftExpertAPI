# GetActivitiesFromWorkflow

Retorna a lista de atividades de uma instância (conforme o dataset configurado no SoftExpert).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Pré-requisito (dataset)

É necessário existir no SoftExpert o conjunto de dados `queryGetCurrentActivities`.

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `List<WFStruct>` | Atividades da instância |

Se nenhuma atividade/instância for encontrada, lança `SoftExpertException`.

## Exemplo

```csharp
List<WFStruct> atividades = wfAPI.GetActivitiesFromWorkflow("CCF202614358");

foreach (var atividade in atividades)
{
    Console.WriteLine($"{atividade.idstruct} | {atividade.fgstatus}");
}
```

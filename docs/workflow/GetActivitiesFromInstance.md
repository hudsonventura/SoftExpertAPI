# GetActivitiesFromInstance

Retorna a lista de atividades de uma instância (conforme o dataset configurado no SoftExpert).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Conjunto de dados (SoftExpert)

- [queryGetActivitiesFromInstance](../ConjuntosDeDados.md#querygetactivitiesfrominstance)

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
List<WFStruct> atividades = wfAPI.GetActivitiesFromInstance("CCF202614358");

foreach (var atividade in atividades)
{
    Console.WriteLine($"{atividade.idstruct} | {atividade.fgstatus}");
}
```

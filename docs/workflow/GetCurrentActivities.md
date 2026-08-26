# GetCurrentActivities

Retorna as atividades em andamento de uma instância.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Conjunto de dados (SoftExpert)

Utiliza internamente `GetActivitiesFromInstance`.

- [queryGetActivitiesFromInstance](../ConjuntosDeDados.md#querygetactivitiesfrominstance)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `List<WFStruct>` | Atividades com `fgstatus == Em_Andamento` |

Cada item possui, entre outros: `idstruct`, `nmstruct`, `fgstatus`.

## Exemplo

```csharp
List<WFStruct> atividades = wfAPI.GetCurrentActivities("CCF202614358");

foreach (var atividade in atividades)
{
    Console.WriteLine($"{atividade.idstruct} - {atividade.nmstruct}");
}
```

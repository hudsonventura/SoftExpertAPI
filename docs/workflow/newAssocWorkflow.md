# newAssocWorkflow

Associa uma instância de workflow a outra (ex.: SA e LA).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | Instância pai |
| `AssocWorkflowID` | `string` | Sim | Instância a associar |
| `assocWorkflowType` | `AssocWorkflowType?` | Não | `NaoBloqueante` (0) ou `Bloqueante` (1); omitido = não bloqueante no serviço |
| `ActivityID` | `string?` | Não | ID da atividade |
| `ActivityOrder` | `int?` | Não | Ordem da atividade |

## Enum `AssocWorkflowType`

| Valor | Inteiro | Descrição |
| --- | --- | --- |
| `NaoBloqueante` | 0 | Padrão |
| `Bloqueante` | 1 | Associação bloqueante |

## Retorno

Não retorna valor. Em caso de erro, lança `SoftExpertException`.

## Exemplo

```csharp
wfAPI.newAssocWorkflow("SA202600001", "LA202600002", AssocWorkflowType.Bloqueante, ActivityID: "ATIV-XYZ", ActivityOrder: 1);
wfAPI.newAssocWorkflow("SA202600001", "LA202600002"); // tipo não bloqueante (omite AssocWorkflowType)
```
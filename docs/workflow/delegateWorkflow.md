# delegateWorkflow

Delega a execução de uma atividade para outro usuário (endpoint de gestão do SoftExpert).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Pré-requisitos

- Dataset `queryGetWorkflowInstanceData`
- Dataset `queryGetCurrentActivities`
- `token` válido na configuração

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `ActivityID` | `string` | Sim | ID da atividade |
| `explanation` | `string` | Sim | Justificativa da delegação |
| `cduser` | `int` | Sim | Código numérico (`cduser`) do novo executor |

## Retorno

Não retorna valor.

## Exemplo

```csharp
wfAPI.delegateWorkflow(
    workflowID: "CCF202614358",
    ActivityID: "ATIV-centralCadastro",
    explanation: "Delegação via SoftExpertAPI",
    cduser: 99
);
```

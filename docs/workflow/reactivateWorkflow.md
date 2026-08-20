# reactivateWorkflow

Reativa uma instância de processo (endpoint de gestão do SoftExpert).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Pré-requisitos

- Dataset `queryGetWorkflowInstanceData`
- Dataset `queryGetCurrentActivities`
- `token` válido na configuração (autenticação via cookie de gestão)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `ActivityID` | `string` | Sim | ID da atividade alvo (se não encontrada, usa a primeira disponível) |
| `explanation` | `string` | Sim | Justificativa |
| `userID` | `string` | Sim | Matrícula do usuário |

## Retorno

Não retorna valor.

## Exemplo

```csharp
wfAPI.reactivateWorkflow(
    workflowID: "CCF202614358",
    ActivityID: "ATIV-centralCadastro",
    explanation: "Reativação via SoftExpertAPI",
    userID: "sistema.teste"
);
```

# returnWorkflow

Retorna uma instância para uma atividade específica (endpoint de gestão do SoftExpert).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Pré-requisitos

- Dataset `queryGetWorkflowInstanceData`
- Dataset `queryGetCurrentActivities`
- `token` válido na configuração

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `ActivityID` | `string` | Sim | ID da atividade corrente (se não encontrada, usa a primeira em andamento) |
| `explanation` | `string` | Sim | Justificativa / comentário |
| `userID` | `string` | Sim | Matrícula do usuário |

## Retorno

Não retorna valor.

## Exemplo

```csharp
wfAPI.returnWorkflow(
    workflowID: "CCF202614358",
    ActivityID: "ATIV-centralCadastro",
    explanation: "Retorno via SoftExpertAPI",
    userID: "sistema.teste"
);
```

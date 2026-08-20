# cancelWorkflow

Cancela uma instância de Workflow, Problema ou Incidente (tenta os módulos automaticamente).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `explanation` | `string` | Sim | Justificativa do cancelamento |
| `userID` | `string` | Não | Matrícula do usuário que cancela (precisa ter permissão) |

## Retorno

Não retorna valor.

## Exemplo

```csharp
wfAPI.cancelWorkflow(
    workflowID: "VBG202002801",
    explanation: "Cancelamento via integração SoftExpertAPI",
    userID: "sistema.teste"
);
```

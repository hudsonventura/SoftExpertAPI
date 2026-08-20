# newWorkflow

Cria uma nova instância de processo de Workflow.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `ProcessID` | `string` | Sim | Identificador do processo |
| `WorkflowTitle` | `string` | Sim | Título da instância |
| `UserID` | `string?` | Não | Matrícula do usuário iniciador |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `string` | ID da instância criada (`RecordID`) |

Em caso de falha na API SoftExpert, lança `SoftExpertException`.

## Exemplo

```csharp
string ProcessID = "CCF";
string WorkflowTitle = "Teste de integração";
string UserID = "sistema.teste";

string WorkflowID = wfAPI.newWorkflow(ProcessID, WorkflowTitle, UserID);
```

# executeActivity

Executa uma atividade de Workflow, Problema ou Incidente (tenta os módulos automaticamente).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |
| `ActivityID` | `string` | Sim | ID da atividade |
| `ActionSequence` | `int` | Sim | Sequência/ID da ação da atividade |
| `UserID` | `string?` | Não | Matrícula do usuário executor |
| `ActivityOrder` | `int?` | Não | Ordem da atividade (quando aplicável) |

## Retorno

Não retorna valor. Em caso de erro, lança `SoftExpertException`.

## Exemplo

```csharp
string WorkflowID = "CCF202614358";
string ActivityID = "ATIV-centralCadastro";
int ActionSequence = 2;
string UserID = "sistema.teste";

wfAPI.executeActivity(WorkflowID, ActivityID, ActionSequence, UserID);
```

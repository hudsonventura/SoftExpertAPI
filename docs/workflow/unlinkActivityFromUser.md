# unlinkActivityFromUser

Desassocia uma atividade do usuário executor e a devolve para o papel funcional ou equipe.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `ActivityID` | `string` | Sim | ID da atividade |

## Retorno

Não retorna valor.

## Exemplo

```csharp
wfAPI.unlinkActivityFromUser("PRO20240518", "AnalisarDemanda");
```

# finishWorkflow

Encerra uma instância de processo mesmo sem chegar ao final do fluxo (endpoint de gestão do SoftExpert).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Conjunto de dados (SoftExpert)

Consulta o dataset via `GetCurrentActivities`. Também requer `token` válido na configuração.

- [queryGetActivitiesFromInstance](../ConjuntosDeDados.md#querygetactivitiesfrominstance)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `explanation` | `string` | Sim | Justificativa |
| `userID` | `string` | Sim | Matrícula do usuário |

## Retorno

Não retorna valor.

## Exemplo

```csharp
wfAPI.finishWorkflow(
    workflowID: "CCF202614358",
    explanation: "Encerramento via SoftExpertAPI",
    userID: "sistema.teste"
);
```

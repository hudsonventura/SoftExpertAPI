# reactivateWorkflow

Reativa uma instância de processo (endpoint de gestão do SoftExpert).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Conjunto de dados (SoftExpert)

Também requer `token` válido na configuração (autenticação via cookie de gestão).

- [queryGetWorkflowInstanceData](../ConjuntosDeDados.md#querygetworkflowinstancedata)
- [queryGetActivitiesFromInstance](../ConjuntosDeDados.md#querygetactivitiesfrominstance)

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

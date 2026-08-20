# addHistoryComment

Adiciona um comentário no histórico de uma instância de Workflow.

Há duas sobrecargas: uma recebe a matrícula como `string` e outra como `int` (convertida internamente para string).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `workflowID` | `string` | Sim | ID da instância |
| `comment` | `string` | Sim | Texto do comentário |
| `userID` / `iduser` | `string` ou `int` | Sim | Usuário autor do comentário |
| `idactivity` | `string` | Sim | ID da atividade associada |
| `is_private` | `bool` | Não | Se `true`, comentário privado (padrão: `false`) |

## Retorno

Não retorna valor.

## Exemplo

```csharp
// Por matrícula (string)
wfAPI.addHistoryComment(
    workflowID: "CCF202614358",
    comment: "Comentário de integração",
    userID: "sistema.teste",
    idactivity: "ATIV-centralCadastro"
);

// Por código numérico (int)
wfAPI.addHistoryComment(
    workflowID: "CCF202614358",
    comment: "Comentário via cduser",
    iduser: 88,
    idactivity: "ATIV-centralCadastro",
    is_private: false
);
```

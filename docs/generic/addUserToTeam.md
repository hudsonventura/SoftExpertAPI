# addUserToTeam

Adiciona um usuário a uma equipe via SOAP `addUserToTeam`.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `idteam` | `string` | Sim | Identificador da equipe |
| `iduser` | `string` | Sim | Matrícula do usuário |

## Retorno

Não retorna valor.

## Exemplo

```csharp
genAPI.addUserToTeam(idteam: "EQUIPE01", iduser: "sistema.teste");
```

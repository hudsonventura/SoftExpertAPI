# removeUserFromTeam

Remove um usuário de uma equipe via SOAP `removeUserFromTeam`.

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
genAPI.removeUserFromTeam(idteam: "EQUIPE01", iduser: "sistema.teste");
```

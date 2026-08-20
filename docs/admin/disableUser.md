# disableUser

Desabilita um usuário no SoftExpert via SOAP `changeUserStatus`.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `UserID` | `string` | Sim | Matrícula do usuário |

## Retorno

Não retorna valor.

## Exemplo

```csharp
adAPI.disableUser("sistema.teste");
```

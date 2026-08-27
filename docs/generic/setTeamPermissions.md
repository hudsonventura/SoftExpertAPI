# setTeamPermissions

Define as permissões de segurança de uma equipe via REST (`PUT /se/exp/generic/team/team.php`).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

**Requisito:** o usuário autenticado precisa de **licença de gestão** e de uma **sessão de gestão disponível** no SoftExpert, além de permissão de acesso à gestão de equipes. Sem isso, a chamada falha (não autorizado ou redirecionamento para login).

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `cdteam` | `int` | Sim | Código numérico da equipe no SoftExpert |
| `idteam` | `string` | Sim | Identificador da equipe |
| `nmteam` | `string` | Sim | Nome da equipe |
| `public_read` | `bool` | Sim | Leitura pública (`fgPublicRead`) |
| `idteam_editor` | `string` | Sim | Código da equipe editora (`cdTeam`) nas permissões |
| `cdprod` | `int` | Não | Código do produto (padrão: `153`) |
| `modules` | `List<int>` | Não | Códigos dos módulos (padrão: `39` — Workflow) |

Se `modules` for `null` ou vazio, a biblioteca usa `{ 39 }` (Workflow). A equipe editora recebe permissão de leitura e edição.

## Retorno

Não retorna valor.

Em caso de falha (não autorizado, equipe não encontrada, ou erro no processamento), lança `SoftExpertException`.

## Exemplo

```csharp
genAPI.setTeamPermissions(
    cdteam: 67062,
    idteam: "EQUIPE_INTEGRACAO",
    nmteam: "Equipe de integração",
    public_read: true,
    idteam_editor: "8"
);
```

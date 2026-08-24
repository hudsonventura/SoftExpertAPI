# newTeam

Insere uma nova equipe no SoftExpert via SOAP `newTeam` (`urn:generic`).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `idteam` | `string` | Sim | Identificador da equipe |
| `nmteam` | `string` | Sim | Nome da equipe |
| `component` | `string` | Sim | Códigos dos componentes separados por vírgula (ex.: `"109,107,73"`) |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `string` | `RecordID` do registro criado |

Em caso de falha na API SoftExpert, lança `SoftExpertException`.

## Exemplo

```csharp
string recordId = genAPI.newTeam(
    idteam: "EQUIPE_INTEGRACAO",
    nmteam: "Equipe de integração",
    component: "109,107,73"
);
```

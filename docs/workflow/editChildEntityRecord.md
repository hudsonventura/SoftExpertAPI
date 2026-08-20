# editChildEntityRecord

Edita um item existente de uma grid (child entity) do formulário principal.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |
| `MainEntityID` | `string` | Sim | ID da entidade principal |
| `ChildRelationshipID` | `string` | Sim | ID do relacionamento/grid |
| `childRecordOID` | `string` | Sim | OID do registro da grid a editar |
| `EntityAttributeList` | `Dictionary<string, string>` | Sim | Campos a atualizar |
| `EntityAttributeFileList` | `Dictionary<string, Anexo>` | Não | Arquivos a anexar nos campos |

## Retorno

Não retorna valor. Em caso de erro, lança `SoftExpertException`.

## Exemplo

```csharp
Dictionary<string, string> campos = new Dictionary<string, string>
{
    { "synced", "1" },
    { "usuario", "teste SoftExpertAPI" },
};

wfAPI.editChildEntityRecord(
    WorkflowID: "IR088482",
    MainEntityID: "IR",
    ChildRelationshipID: "ircomentariorel",
    childRecordOID: "7898431bf32fd35d5636146ce502d057",
    EntityAttributeList: campos
);
```

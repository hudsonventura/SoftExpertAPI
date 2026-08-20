# newChildEntityRecord

Cria um novo item em uma grid (child entity) vinculada ao formulário principal da instância.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |
| `MainEntityID` | `string` | Sim | ID da entidade principal do formulário |
| `ChildRelationshipID` | `string` | Sim | ID do relacionamento/grid |
| `EntityAttributeList` | `Dictionary<string, string>` | Não | Campos do item da grid |
| `RelationshipList` | `Dictionary<string, Dictionary<string, string>>` | Não | Relacionamentos do item |
| `EntityAttributeFileList` | `Dictionary<string, Anexo>` | Não | Arquivos do item |

## Retorno

Não retorna valor. Em caso de erro, lança `SoftExpertException`.

## Exemplo

```csharp
Dictionary<string, string> campos = new Dictionary<string, string>
{
    { "pais", "Brasil" },
    { "contabancaria", "10203040506070" },
};

wfAPI.newChildEntityRecord(
    WorkflowID: "CCF202614358",
    MainEntityID: "SOLCLIENTEFORNE",
    ChildRelationshipID: "invoices",
    EntityAttributeList: campos
);
```

# editTableRecord

Edita um registro de tabela do SoftExpert Form via SOAP `editTableRecord` (`urn:form`).

Diferente de `editEntityRecord`, esta função atua diretamente sobre uma tabela/formulário pelo OID do registro, sem depender de uma instância de Workflow.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `UserID` | `string` | Sim | Matrícula do usuário |
| `TableID` | `string` | Sim | ID da tabela (entidade) |
| `TableFieldOID` | `string` | Sim | OID do registro a editar |
| `TableFieldList` | `Dictionary<string, string>` | Não | Campos (`campo` → `valor`) |
| `RelationshipList` | `Dictionary<string, Dictionary<string, string>>` | Não | Relacionamentos/selectbox |
| `TableFieldFileList` | `Dictionary<string, Anexo>` | Não | Arquivos para campos da tabela |

## Retorno

Não retorna valor. Em caso de erro, lança `SoftExpertException`.

## Exemplo

```csharp
Dictionary<string, string> campos = new Dictionary<string, string>
{
    { "campo1", "valor1" },
    { "campo2", "valor2" },
};

wfAPI.editTableRecord(
    UserID: "sistema.teste",
    TableID: "MINHA_TABELA",
    TableFieldOID: "abcdef0123456789abcdef0123456789",
    TableFieldList: campos
);
```

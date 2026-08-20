# editEntityRecord

Edita os valores do formulário de uma instância de Workflow. Também permite relacionamentos (selectbox) e anexos em campos do formulário.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |
| `EntityID` | `string` | Sim | ID da tabela/entidade do formulário |
| `EntityAttributeList` | `Dictionary<string, string>` | Não | Campos do formulário (`campo` → `valor`) |
| `RelationshipList` | `Dictionary<string, Dictionary<string, string>>` | Não | Relacionamentos: chave = ID do relacionamento; valor = campos do relacionamento |
| `EntityAttributeFileList` | `Dictionary<string, Anexo>` | Não | Arquivos para campos de anexo (`campo` → `Anexo`) |

## Retorno

Não retorna valor. Em caso de erro, lança `SoftExpertException`.

## Exemplo

```csharp
string WorkflowID = "CCF202614358";
string EntityID = "SOLCLIENTEFORNE";

Dictionary<string, string> campos = new Dictionary<string, string>
{
    { "razaosocial", "Empresa Exemplo LTDA" },
    { "cnpj", "00000000000191" },
};

Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>
{
    {
        "pais",
        new Dictionary<string, string> { { "idpais", "BR" } }
    }
};

Dictionary<string, Anexo> anexos = new Dictionary<string, Anexo>
{
    {
        "comprovante",
        new Anexo
        {
            FileName = "comprovante.pdf",
            Content = File.ReadAllBytes(@"C:\temp\comprovante.pdf")
        }
    }
};

wfAPI.editEntityRecord(WorkflowID, EntityID, campos, relacionamentos, anexos);
```

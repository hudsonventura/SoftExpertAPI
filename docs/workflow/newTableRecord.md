# newTableRecord

Cadastra um registro em uma tabela do SoftExpert Form via SOAP `newTableRecord` (`urn:form`).

Assim como `editTableRecord`, esta função atua diretamente sobre uma tabela/formulário, sem depender de uma instância de Workflow.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `UserID` | `string` | Sim | Matrícula do usuário |
| `TableID` | `string` | Sim | ID da tabela (entidade) |
| `TableFieldList` | `Dictionary<string, string>` | Não | Campos (`campo` → `valor`) |
| `RelationshipList` | `Dictionary<string, Dictionary<string, string>>` | Não | Relacionamentos/selectbox |
| `TableFieldFileList` | `Dictionary<string, Anexo>` | Não | Arquivos para campos da tabela |
| `RelatedRelationshipID` | `string` | Não | ID do relacionamento com a tabela principal. Usar quando o registro é de uma tabela grid |
| `RelatedRelationshipValue` | `string` | Não | OID do registro da tabela principal a ser relacionado |

Ao menos um entre `TableFieldList`, `RelationshipList`, `TableFieldFileList` e `RelatedRelationshipID` deve ser informado para que o registro seja incluído.

O bloco `RelatedTo` só é enviado quando `RelatedRelationshipID` é informado.

## Retorno

| Tipo | Descrição |
| --- | --- |
| `string` | `RecordID` do registro criado |

Em caso de erro, lança `SoftExpertException`.

## Exemplo

```csharp
Dictionary<string, string> campos = new Dictionary<string, string>
{
    { "campo1", "valor1" },
    { "campo2", "valor2" },
};

string recordID = wfAPI.newTableRecord(
    UserID: "sistema.teste",
    TableID: "MINHA_TABELA",
    TableFieldList: campos
);
```

Registro em tabela grid, vinculado ao registro da tabela principal:

```csharp
string recordID = wfAPI.newTableRecord(
    UserID: "sistema.teste",
    TableID: "MINHA_TABELA_GRID",
    TableFieldList: campos,
    RelatedRelationshipID: "meurelacionamento",
    RelatedRelationshipValue: "abcdef0123456789abcdef0123456789"
);
```

## Formatos aceitos nos valores dos campos

| Tipo do campo | Formato |
| --- | --- |
| Número | dígitos numéricos, sem separador de milhar e decimal |
| Decimal | dígitos numéricos, sem separador de milhar e com ponto (`.`) como separador decimal |
| Data | `YYYY-MM-DD` |
| Hora | `HH:MM` |
| Boolean | `0` ou `1` |

Nos relacionamentos, o valor informado deve existir na tabela relacionada para que o sistema o preencha.

## Documentação oficial

<https://developer.softexpert.com/docs/2.2.4/data-integration/reference/web-service-soap/form/newTableRecord>

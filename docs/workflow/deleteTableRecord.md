# deleteTableRecord

Exclui um registro de uma tabela do SoftExpert Form via SOAP `deleteTableRecord` (`urn:form`).

Assim como `editTableRecord`, esta função atua diretamente sobre uma tabela/formulário pelo OID do registro, sem depender de uma instância de Workflow.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `TableID` | `string` | Sim | ID da tabela (entidade) |
| `TableFieldOID` | `string` | Sim | OID do registro a ser excluído |

## Retorno

Não retorna valor. Em caso de erro, lança `SoftExpertException`.

Códigos de falha retornados pelo SoftExpert:

| Código | Descrição |
| --- | --- |
| `-30` | O usuário não tem permissão para excluir o registro |
| `-31` | Ocorreu um erro ao excluir o registro |

## Exemplo

```csharp
wfAPI.deleteTableRecord(
    TableID: "MINHA_TABELA",
    TableFieldOID: "abcdef0123456789abcdef0123456789"
);
```

## Documentação oficial

<https://developer.softexpert.com/docs/2.2.4/data-integration/reference/web-service-soap/form/deleteTableRecord>

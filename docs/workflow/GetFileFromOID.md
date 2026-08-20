# GetFileFromOID

Obtém um arquivo a partir do OID (geralmente proveniente de um campo de anexo do formulário).

Requer implementação de `IFileDownload` na configuração (`configs.downloader`).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Pré-requisito (dataset)

É necessário existir no SoftExpert o conjunto de dados `queryGetAttachmentFile`.

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `oid` | `string` | Sim | OID do arquivo no SoftExpert |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `Anexo` | Objeto com `FileName` e `Content` (`byte[]`) |

Se o OID não for encontrado, lança `SoftExpertException`.

## Exemplo

```csharp
Anexo arquivo = wfAPI.GetFileFromOID("0e6fbe048a635aaf00deea99b9f3bbc3");

File.WriteAllBytes($@"C:\temp\{arquivo.FileName}", arquivo.Content);
```

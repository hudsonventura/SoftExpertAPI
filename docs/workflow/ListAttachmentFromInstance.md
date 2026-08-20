# ListAttachmentFromInstance

Lista os arquivos anexados no menu de anexos da instância. Opcionalmente filtra pela atividade em que o arquivo foi anexado.

Requer implementação de `IFileDownload` na configuração (`configs.downloader`).

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Pré-requisito (dataset)

É necessário existir no SoftExpert o conjunto de dados `queryGetAttachmentFile` (veja o SQL comentado no código-fonte de `SoftExpertWorkflowApi`).

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |
| `ActivityID` | `string` | Não | Filtra anexos pela atividade (`idstruct`) |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `List<Anexo>` | Lista de anexos com `FileName`, `Content` (`byte[]`), códigos e metadados |

## Exemplo

```csharp
List<Anexo> arquivos = wfAPI.ListAttachmentFromInstance("IR088482");

foreach (var arquivo in arquivos)
{
    Console.WriteLine($"{arquivo.FileName} ({arquivo.Content.Length} bytes)");
}
```

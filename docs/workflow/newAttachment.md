# newAttachment

Anexa um arquivo no menu de anexos (lado esquerdo) de uma instância.

> Instanciação: [Instanciacao.md](../Instanciacao.md)

## Parâmetros de entrada

| Nome | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `WorkflowID` | `string` | Sim | ID da instância |
| `ActivityID` | `string` | Sim | ID da atividade em que o anexo será associado |
| `File` | `Anexo` | Sim | Arquivo (`FileName` e `Content` obrigatórios) |
| `UserID` | `string` | Não | Matrícula do usuário |

## Retorno

| Tipo | Descrição |
| --- | --- |
| `int` | Chave do registro criado (`RecordKey`) |

## Exemplo

```csharp
Anexo arquivo = new Anexo
{
    FileName = "Teste.txt",
    Content = Encoding.UTF8.GetBytes("Conteúdo do arquivo")
};

int recordKey = wfAPI.newAttachment("CCF202614358", "ATIV-centralCadastro", arquivo);
```

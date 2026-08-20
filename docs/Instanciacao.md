# Instanciação e configuração

Antes de chamar qualquer função, configure e instancie a API do módulo desejado.

## Namespace

```csharp
using SoftExpertAPI;
```

## Configurations

| Propriedade | Tipo | Obrigatório | Descrição |
| --- | --- | --- | --- |
| `baseUrl` | `string` | Sim | URL completa do SoftExpert (ex.: `https://se.dominio.com.br`) |
| `token` | `string` | Sim* | Token gerado no perfil do usuário no SoftExpert. Usado nas chamadas REST/dataset e no header `Authorization` |
| `login` | `string` | Condicional | Login do usuário (quando não houver token Basic, ou para autenticação de gestão via cookie) |
| `pass` | `string` | Condicional | Senha do usuário |
| `domain` | `string` | Não | Domínio (padrão: `"SE Suite"`). Usado na autenticação Basic `dominio\login:senha` |
| `downloader` | `IFileDownload` | Condicional | Necessário para `ListAttachmentFromInstance` e `GetFileFromOID` quando os arquivos estão em diretório controlado |
| `headers` | `Dictionary<string, string>` | Não | Headers HTTP adicionais |
| `db` | `IDataBase` | Não | Legado. As funções públicas atuais não dependem mais de acesso direto ao banco |

\* O token é obrigatório para as integrações via dataset e para várias operações de gestão.

## Exemplo de configuração

```csharp
// Opcional: necessário apenas para download de anexos/arquivos de formulário em diretório controlado
IFileDownload downloader = new MinhaImplementacaoFileDownload();

SoftExpertAPI.Configurations configs = new Configurations()
{
    baseUrl = "https://se.dominio.com.br",
    login = "usuario",
    pass = "senha",
    domain = "SE Suite",
    token = "SEU_TOKEN_GERADO_NO_PERFIL",

    // Opcional
    downloader = downloader,
};
```

### Autenticação

- Com `token` preenchido: o header `Authorization` usa o token informado.
- Sem token (apenas login/senha/domain): monta Basic Auth `dominio\login:senha`. Mesmo assim, o token continua necessário para o cliente de dataset.

## Instanciação por módulo

```csharp
// Workflow
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(configs);

// Administração
SoftExpertAdminApi adAPI = new SoftExpertAdminApi(configs);

// Genérico
SoftExpertGenericApi genAPI = new SoftExpertGenericApi(configs);
```

## Tratamento de erros (padrão)

```csharp
try
{
    // chamada à API
}
catch (SoftExpertException erro)
{
    // Erro retornado pela API SoftExpert (comunicação ok, parâmetro/regra rejeitada)
    Console.WriteLine(erro.Message);
}
catch (Exception erro)
{
    // Falha genérica (rede, configuração, etc.)
    Console.WriteLine(erro.Message);
}
```

## Implementação de `IFileDownload`

Necessária quando os arquivos do SoftExpert não estão no banco e sim em diretório controlado.

```csharp
public class MinhaImplementacaoFileDownload : IFileDownload
{
    public byte[] DownloadFileAttach(string filename)
    {
        // Baixa anexo de instância a partir do nome/caminho controlado
        throw new NotImplementedException();
    }

    public byte[] DownloadFileForm(string filename)
    {
        // Baixa arquivo de formulário a partir do nome/caminho controlado
        throw new NotImplementedException();
    }
}
```

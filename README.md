# SoftExpertAPI
  
<p align="center">
	<img src="src/120.png" alt="SoftExpert Logo" width="100px" height="100px">
	<img src="src/dotnet_logo.png" alt="Dotnet Logo" width="180px" height="100px">
</p>
  
SoftExpertAPI é uma biblioteca em .NET / C# que abstrai a comunicação SOAP e REST com a API do SoftExpert SE Suite.  
Esta biblioteca não está completa e será desenvolvida conforme necessidades e pedidos.

Direitos reservados a https://www.softexpert.com/

Documentação original: https://documentation.softexpert.com/en/integration/index.html  
Documentação original (nova versão): https://developer.softexpert.com/docs/data-integration/getting-started/platform-overview/

Contato: `hudsonventura@outlook.com`

### Obs.: Testado no SoftExpert 2.1.4.x

## Get Started

```bash
dotnet add package SoftExpertAPI
```

```csharp
using SoftExpertAPI;
```

Pacote NuGet: [https://www.nuget.org/packages/SoftExpertAPI/](https://www.nuget.org/packages/SoftExpertAPI/)

### Instanciação

A configuração e instanciação das classes estão documentadas em:

**[docs/Instanciacao.md](docs/Instanciacao.md)**

Resumo:

```csharp
SoftExpertAPI.Configurations configs = new Configurations()
{
    baseUrl = "https://se.dominio.com.br",
    login = "usuario",
    pass = "senha",
    domain = "SE Suite",
    token = "SEU_TOKEN",
    // downloader = minhaImplementacaoIFileDownload, // opcional: anexos em diretório controlado
};
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(configs);
```

### Exemplo rápido

```csharp
try
{
    string WorkflowID = wfAPI.newWorkflow("CCF", "Teste de integração", "sistema.teste");
}
catch (SoftExpertException erro)
{
    // Erro retornado pela API SoftExpert
    Console.WriteLine(erro.Message);
}
catch (Exception erro)
{
    // Falha genérica (rede, configuração, etc.)
    Console.WriteLine(erro.Message);
}
```

## Documentação das funções

Cada função pública possui um arquivo próprio com explicação, parâmetros, retorno e exemplo:

**[docs/README.md](docs/README.md)**

Conjuntos de dados necessários no SoftExpert: **[docs/ConjuntosDeDados.md](docs/ConjuntosDeDados.md)**

### Workflow (`SoftExpertWorkflowApi`)
Para as funções abaixo, considerar o instanciamento do objeto abaixo:
``` C#
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(configs);
```
| Função | Diretório controlado | Conjunto de dados | Objetivo |
| --- | :---: | :---: | --- |
| [newWorkflow](docs/workflow/newWorkflow.md) | | | Criar instância de processo |
| [cancelWorkflow](docs/workflow/cancelWorkflow.md) | | | Cancelar instância |
| [executeActivity](docs/workflow/executeActivity.md) | | | Executar atividade |
| [newAttachment](docs/workflow/newAttachment.md) | | | Anexar arquivo no menu de anexos |
| [ListAttachmentFromInstance](docs/workflow/ListAttachmentFromInstance.md) | ✔ | ✔ | Listar anexos da instância |
| [GetFileFromOID](docs/workflow/GetFileFromOID.md) | ✔ | ✔ | Obter arquivo a partir do OID |
| [editEntityRecord](docs/workflow/editEntityRecord.md) | | | Editar campos do formulário |
| [newChildEntityRecord](docs/workflow/newChildEntityRecord.md) | | | Criar registro em grid |
| [editChildEntityRecord](docs/workflow/editChildEntityRecord.md) | | | Editar registro em grid |
| [editTableRecord](docs/workflow/editTableRecord.md) | | | Editar registro de tabela (Form) |
| [addHistoryComment](docs/workflow/addHistoryComment.md) | | | Adicionar comentário no histórico |
| [unlinkActivityFromUser](docs/workflow/unlinkActivityFromUser.md) | | | Desassociar atividade do usuário |
| [reactivateWorkflow](docs/workflow/reactivateWorkflow.md) | | ✔ | Reativar instância |
| [returnWorkflow](docs/workflow/returnWorkflow.md) | | ✔ | Retornar para uma atividade |
| [finishWorkflow](docs/workflow/finishWorkflow.md) | | ✔ | Encerrar instância |
| [delegateWorkflow](docs/workflow/delegateWorkflow.md) | | ✔ | Delegar atividade |
| [AlterUserStart](docs/workflow/AlterUserStart.md) | | | Alterar usuário iniciador |
| [GetWorflowStatus](docs/workflow/GetWorflowStatus.md) | | ✔ | Consultar status da instância |
| [GetCurrentActivities](docs/workflow/GetCurrentActivities.md) | | ✔ | Listar atividades em andamento |
| [GetActivitiesFromInstance](docs/workflow/GetActivitiesFromInstance.md) | | ✔ | Listar atividades da instância |

### Administração (`SoftExpertAdminApi`)
Para as funções abaixo, considerar o instanciamento do objeto abaixo:
``` C#
SoftExpertAdminApi adAPI = new SoftExpertAdminApi(configs);
```
| Função | Objetivo |
| --- | --- |
| [enableUser](docs/admin/enableUser.md) | Habilitar usuário |
| [disableUser](docs/admin/disableUser.md) | Desabilitar usuário |

### Genérico (`SoftExpertGenericApi`)
Para as funções abaixo, considerar o instanciamento do objeto abaixo:
``` C#
SoftExpertGenericApi genAPI = new SoftExpertGenericApi(configs);
```
| Função | Objetivo |
| --- | --- |
| [newTeam](docs/generic/newTeam.md) | Criar nova equipe |
| [addUserToTeam](docs/generic/addUserToTeam.md) | Adicionar usuário a uma equipe |
| [removeUserFromTeam](docs/generic/removeUserFromTeam.md) | Remover usuário de uma equipe |

## Observações

- Funções iniciadas com letra minúscula (`newWorkflow`) espelham APIs originais do SoftExpert.
- Funções iniciadas com letra maiúscula (`AlterUserStart`) possuem implementação própria e/ou usam endpoints/datasets do SoftExpert.
- ✔ **Diretório controlado**: requer implementação de `IFileDownload` em `configs.downloader`.
- ✔ **Conjunto de dados**: requer criação do conjunto de dados correspondente no SoftExpert — ver [docs/ConjuntosDeDados.md](docs/ConjuntosDeDados.md).

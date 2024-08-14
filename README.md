# SoftExpertAPI
  
<p align="center">
	<img src="src/120.png" alt="Dotnet Logo" width="100px" height="100px">
	<img src="src/dotnet_logo.png" alt="Dotnet Logo" width="180px" height="100px">
</p>
  
SoftExpertAPI é uma biblioteca em Dotnet Core / C#, que possui um conjunto de classes para abstrair a comunicação SOAP, REST com a API do SoftExpert SESuite e/ou via banco de dados para algumas implementações.<br>
Esta biblioteca não está completa e será desenvolvida conforme necessidades e pedidos.  Há uma tabela no final com as funções implementada e suas limitações.   

Direitos reservados a https://www.softexpert.com/  

Documentação original: https://documentation.softexpert.com/en/integration/index.html  
Documentação original nova versão: https://developer.softexpert.com/docs/data-integration/getting-started/platform-overview/  

Meu email. `hudsonventura@outlook.com`  

### Obs.: Testado no SoftExpert 2.1.9.x e 2.2.1.x com bando de dados Oracle

Há exemplos funcionais no diretório `Examples`  

## Get Started 
Instale os pacotes do Nuget  
``` bash
dotnet add package SoftExpertAPI
```

### Importe os namespaces ...
```C#
using SoftExpertAPI;
using SoftExpertAPI.Workflow;  //para o módulo de Workflow
using SoftExpertAPI.Admin;     //para o módulo de Administração
```  


### Crie uma instancia da API do módulo de Workflow

```C#
using SoftExpertAPI;
using SoftExpertAPI.Workflow; 

//Implementação OPCIONAL de uma classe para acessar banco de dados. É necessário respeitar a interface SoftExpertAPI.Interfaces.IDataBase
//Necessário para algumas implementações fora do escopo da API padrão do SoftExpert.
ExampleOracleImplementation _db = new ExampleOracleImplementation(_appsettings);


//Necessário em casos em que os arquivos do SE não ficam no banco de dados
IFileDownload _downloader = new ExampleFileDownloadImplementation(appsettings);


//Criar instancia da API para utilizar na injeção de dependecias. Necessário informar a URL completa do SE e o header Authorization ou todos os headers.
//Se o parâmetro 'db' não for passado, algumas funções não serão corretamente executadas
SoftExpertAPI.Configurations configs = new Configurations(){

	baseUrl = "https://se.dominio.com.br",
	
	login = "usuario para login",
	pass = "senha do usuario",
	domain = "dominio em caso de sincronização com o AD. Senao, não preencher",
	token = "SEU TOKEN GERADO NO SEU PERFIL DE USUARIO" //passar em caso de não passar login e senha. Se passado o token, login e senha serão ignorados

	//OPCIONAIS
	db = _db,                 //Necessário para funções que acessam o banco de dados. Implementar a interface SoftExpert.IDataBase
	downloader = _downloader, //Necessário para caso os arquivos do SE fiquem em um diretório controlado. Implementar a interface SoftExpert.IFileDownload
};
wfAPI = new SoftExpertWorkflowApi(configs);
```  


### Crie uma instancia da API do módulo de Administração COM acesso a banco de dados (necessário para muitas funcionalidades)

```C#
SoftExpertAdminApi adminAPI = new SoftExpertAdminApi(configs);
```  

### Usando a API - Criando uma instancia de workflow  

```C#
string ProcessID = "CCF";                       //identificador do processo
string WorkflowTitle = "Teste de integração"; ; //titulo da instancia a ser criado
string UserID = "00000000000";                  //matricula do usuario


try
{
    string WorkflowIDCreated = wfAPI.newWorkflow(ProcessID, WorkflowTitle, UserID);
}
catch (SoftExpertException erro)
{
    Console.WriteLine($"Este tipo de erro é um erro retornado pela API do SoftExpert. Quando ele for lançado, significa que a comunicação com o servidor funcionou, mas você passou algum parametro que o SESuite não aceito. No 'erro.Message' há detalhes sobre o problema. Erro: {erro.Message}");
}
catch (Exception erro)
{
    Console.WriteLine($"Este tipo de erro é um erro genérico, que provavelmente acontecerá em caso de falha de comunicação com o servidor. Erro: {erro.Message}");
}
```

## Demais exemplos
Há mais exemplos nos arquivos [Examples/WorkflowExamples.cs](Examples/WorkflowExamples.cs) e no [Examples/AdminExamples.cs](Examples/AdminExamples.cs).  
Não se esquece de que se precisar de acesso ao banco de dados, implemente a interface `SoftExpert.IDataBase` como no exemplo em [Examples/ExampleOracleImplementation.cs](Examples/ExampleOracleImplementation.cs).  
Em caso dos arquivos de formulário, documentos e anexos não estiverem no banco de dados, será necessário implementar a interface `SoftExpert.IFileDownload` como no exemplo em [Examples/ExampleFileDownloadImplementation.cs](Examples/ExampleFileDownloadImplementation.cs).


## Funções do módulo de Workflow

| Função                         | Banco de dados | Diretório Controlado | Exemplo | Objetivo                                                                           |
| ------------------------------ | :------------: | :------------------: | :-----: | ---------------------------------------------------------------------------------- |
| **newWorkflow**                |                |                      |    ✔    | Criar uma instância de um processo                                                 |
| **cancelWorkflow**             |                |                      |    ✔    | Cancelar uma instância                                                             |
| **ChangeWorflowTitle**         |       ❌        |                      |    ✔    | Alterar o título de uma instância de processo                                      |
| **executeActivity**            |                |                      |    ✔    | Executar uma atividade                                                             |
| **newAttachment**              |                |                      |    ✔    | Anexar um novo arquivo no menu de anexos do lado esquerdo                          |
| **ListAttachmentFromInstance** |       ❌        |          ❌           |    ✔    | Listar os arquivos de uma instância                                                |
| **editEntityRecord**           |                |                      |    ✔    | Editar os campos de um formulário                                                  |
| **GetFormData**                |       ❌        |                      |    ✔    | Obter os dados (campos e valores) de um formulário                                 |
| **GetFormSelectBox**           |       ❌        |                      |    ❌    | Obter o valor de um selectbox de um formulário                                     |
| **GetWorkFlowData**            |       ❌        |                      |    ❌    | Obter os dados de uma instancia da tabela wfprocess                                |
| **GetFileFromOID**             |       ❌        |          ❌           |    ✔    | Obter um arquivo de um OID obtível de um campo de anexo do formulário              |
| **GetFileFromFormField**       |       ❌        |          ❌           |    ✔    | Obter um arquivo de um campo do formulário                                         |
| **newChildEntityRecord**       |                |                      |    ✔    | Criar um registro de uma grid                                                      |
| **editChildEntityRecord**      |                |                      |    ✔    | Editar um registro de uma grid                                                     |
| **ListGridItems**              |       ❌        |                      |    ✔    | Listar os registros de uma grid do menu do lado esquerdo                           |
| **getFormSelectBox**           |                |                      |    ❌    |                                                                                    |
| **getWorkFlowData**            |                |                      |    ❌    |                                                                                    |
| **MarkActivityAsExecuted**     |                |                      |    ❌    | Marcar uma atividade como executada, mas não a executa de fato.                    |
| **addHistoryComment**          |                |                      |    ✔    | Adiciona um comentário no histórico                                                |
| **unlinkActivityFromUser**     |                |                      |    ✔    | Desassocia uma atividade de um usuário e devolve para o papel funcional ou equipe. |
| **reactivateWorkflow**         |                |                      |    ✔    | Reativa uma instância de processo                                                  |
| **returnWorkflow**             |                |                      |    ✔    | Retorna uma instância de processo para uma atividade específica                    |
| **delegateWorkflow**           |                |                      |    ✔    | Delega uma atividade                                                               |
| **GetWorflowStatus**           |       ❌        |                      |    ❌    |                                                                                    |
| **GetActualActivities**        |       ❌        |                      |    ❌    | Este método tráz a lista de atividades habilitadas de uma instancia                |

Obs. Exemplos no arquivo  [Examples/WorkflowExamples.cs](Examples/WorkflowExamples.cs).  

## Funções do módulo de Administração

| Função          | Banco de dados | Diretório Controlado | Exemplo | Objetivo             |
| --------------- | :------------: | :------------------: | :-----: | -------------------- |
| **enableUser**  |       ❌        |                      |    ✔    | Ativar um usuário    |
| **disableUser** |       ❌        |                      |    ✔    | Desativar um usuário |

Obs. Exemplos no arquivo [Examples/AdminExamples.cs](Examples/AdminExamples.cs).  


Obs. Gerais:  
Funções iniciadas com letra minúscula como `newWorkflow`, são apenas uma tradução para alguma API original do SE.  
Funções iniciadas com letra maiúscula como `AddHistoryComment`, possuem desenvolvimento próprio e/ou não usam apenas alguma API original do SE.  
❌ **Coluna Banco de dados** - Itens necessitam de acesso a banco de dados, então precisarão da implementação da interface `IDatabase`.  
❌ **Coluna Diretório Controlado** - Itens que caso você possua arquivos de formulário, anexo e documento em um diretório controlado, então precisarão da implementação da interface `IFileDownloader`.  
✔ **Coluna Exemplo** - Itens que já possuem exemplo implementado. Veja no diretório `Examples`.  
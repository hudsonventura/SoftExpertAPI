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
using SoftExpert;
using SoftExpert.Workflow;  //para o módulo de Workflow
using SoftExpert.Admin;     //para o módulo de Administração
```  


### Crie uma instancia da API do módulo de Workflow

```C#
using SoftExpert;
using SoftExpert.Workflow; 

string authorization = "Basic base64encode(DOMINIO\USUARIO:SENHA)"; //deve ser codificado em base64

//Implementação OPCIONAL de uma classe para acessar banco de dados. É necessário respeitar a interface SoftExpertAPI.Interfaces.IDataBase
//Necessário para algumas implementações fora do escopo da API padrão do SoftExpert.
ExampleOracleImplementation _db = new ExampleOracleImplementation(_appsettings);


//Necessário em casos em que os arquivos do SE não ficam no banco de dados
IFileDownload _downloader = new ExampleFileDownloadImplementation(appsettings);


//Criar instancia da API para utilizar na injeção de dependecias. Necessário informar a URL completa do SE e o header Authorization ou todos os headers.
//Se o parâmetro 'db' não for passado, algumas funções não serão corretamente executadas
SoftExpert.Configurations configs = new Configurations(){

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

| Função                         | Banco de dados | Diretório Controlado | Objetivo                                                  |
| ------------------------------ | -------------- | -------------------- | --------------------------------------------------------- |
| **NewWorkflow**                |                |                      | Criar uma instância de um processo                        |
| **CancelWorkflow**             |                |                      | Cancelar uma instância                                    |
| **ChangeWorflowTitle**         | ❌              |                      | Alterar o título de uma instância                         |
| **ExecuteActivity**            |                |                      | Executar uma atividade                                    |
| **NewAttachment**              |                |                      | Anexar um novo arquivo no menu de anexos do lado esquerdo |
| **listAttachmentFromInstance** | ❌              | ❌                    | Listar os arquivos de uma instância                       |
| **EditEntityRecord**           |                |                      | Editar os campos de um formulário                         |
| **getFormData**                | ❌              |                      | Obter os dados (campos e valores) de um formulário        |
| **getFormSelectBox**           | ❌              |                      | Obter o valor de um selectbox de um formulário            |
| **getWorkFlowData**            | ❌              |                      | Obter os dados de uma instancia da tabela wfprocess       |
| **GetFileFromOID**             | ❌              |                      | Obter um arquivo de um formlário                          |
| **getFileFromFormField**       | ❌              | ❌                    | Obter um arquivo de um campo do formulário                |
| **NewChildEntityRecord**       |                |                      | Criar um registro de uma grid                             |
| **EditChildEntityRecord**      |                |                      | Editar um registro de uma grid                            |
| **ListGridItems**              | ❌              |                      | Listar os registros de uma grid do menu do lado esquerdo  |


 
Obs.:  
**Coluna Banco de dados** - Itens necessitam de acesso a banco de dados, então precisarão da implementação da interface `IDatabase`.


## Funções do módulo de Administração

| Função          | Banco de dados | Objetivo             |
| --------------- | -------------- | -------------------- |
| **enableUser**  | ❌              | Ativar um usuário    |
| **disableUser** | ❌              | Desativar um usuário |

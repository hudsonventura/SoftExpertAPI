# SoftExpertAPI
SoftExpertAPI é uma biblioteca que possui um conjunto de classes para abstrair a comunicação SOAP, REST com a API do SoftExpert SESuite e/ou via banco de dados para algumas implementações.<br>
Esta biblioteca não está completa e será desenvolvida conforme necessidades e pedidos.  

Direitos reservados a https://www.softexpert.com/  

Documentação original: https://documentation.softexpert.com/en/integration/index.html  
Documentação original nova versão: https://developer.softexpert.com/docs/data-integration/getting-started/platform-overview/  

Meu email. `hudsonventura@outlook.com`  

### Obs.: Testado no SoftExpert 2.1.9.x e Oracle

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


### Crie uma instancia da API do módulo de Workflow SEM acesso a banco de dados
```C#
using SoftExpert;
using SoftExpert.Workflow; 

string authorization = "Basic base64encode(DOMINIO\USUARIO:SENHA)"; //deve ser codificado em base64
string url = "https://se.dominio.com.br";
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(url, authorization);
```  


### Crie uma instancia da API do módulo de Administração COM acesso a banco de dados (necessário para muitas funcionalidades)
Necessário para as funções: `listAttachmentFromInstance`<br>
Necessário a implementação de um banco de dados (IDatabase). Ver exemplo de implementação no arquivo `Examples/ExampleOracleImplement.cs`. Podem ser implementados outros bancos de dados, desde que estes implementem a interface `IDatabase`.  
Esta biblioteca foi testada apenas no banco de dados Oracle, utilizando a implmentação  do arquivo `Examples/ExampleOracleImplement.cs`, mas implementando a interface `IDatabase` você pode obter trabalhar normalmente.  

```C#
string authorization = "Basic base64encode(DOMINIO\USUARIO:SENHA)"; //deve ser codificado em base64
string url = "https://se.dominio.com.br";

ExampleOracleImplement oracle = new ExampleOracleImplement(appsettings);
SoftExpertAdminApi adminAPI = new SoftExpertAdminApi(url, authorization, db: oracle);
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
Não se esquece de que se precisar de acesso ao banco de dados, implemente a interface `IDatabase` como no exemplo [Examples/ExampleOracleImplementation.cs](Examples/ExampleOracleImplementation.cs)


## Funções do módulo de Workflow
 - **NewWorkflow** - Criar uma nova instância de um processo
 - **CancelWorkflow** - Cancelar uma instância
 - **ChangeWorflowTitle** ¹ - Alterar o título de uma instância
 - **ExecuteActivity** - Executar uma atividade

 - **NewAttachment** - Anexar um novo arquivo no menu de anexos do lado esquerdo
 - **listAttachmentFromInstance** *¹ - Listar os arquivos de uma instância 

 - **EditEntityRecord** - Editar os campos de um formulário
 - **getFormData** ¹ - Obter os dados (campos e valores) de um formulário

 - **getFormSelectBox** *¹ - Obter o valor de um selectbox de um formulário
 - **getWorkFlowData** *¹ - 
 - **GetFileFromOID** *¹ - Onter um arquivo de um formlário
 - **GetFileFromFormField** *¹ - 

 - **NewChildEntityRecord** - Criar um registro de uma grid
 - **EditChildEntityRecord** - Editar um registro de uma grid
 - **ListGridItems** ¹ - Listar os registros de uma grid
do menu do lado esquerdo

Obs.:  
( * ) - Itens já foram implementados mas ainda não possuem exemplos.  
( ¹ ) - Itens necessitam de acesso a banco de dados, então precisarão da implementação da interface `IDatabase`.


## Funções do módulo de Administração
 - **enableUser** ¹ - Ativar um usuário
 - **disableUser** ¹ - Desativar um usuário

Obs.:  
( * ) - Itens já foram implementados mas ainda não possuem exemplos.  
( ¹ ) - Itens necessitam de acesso a banco de dados, então precisarão da implementação da interface `IDatabase`.
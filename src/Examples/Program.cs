using Examples;
using Services;
using SoftExpert.Workflow;

//TODO: dependendo dos caracteres do WorkflowTitle, a instancia não pode ser criada.
//TODO: anexar arquivo no anexo do lado esquerdo: CCF202323106
//TODO: anexar arquivo no form

//Carregar as consigurações
var appsettings = Appsettings.GetSettings();
string url = appsettings["url"].ToString();
string authorization = appsettings["authorization"].ToString();


//Criar instancia da API para utilizar na injeção de dependecias. Necessário informar a URL completa do SE e o header Authorization ou todos os headers.
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(url, authorization);




//Criar instancia de processo - Veja o exemplo complexto no arquivo newWorkflowExample.cs
newWorkflowExample newWorkflowExample = new newWorkflowExample(wfAPI);
newWorkflowExample.Main();





//Editar um formulário
editEntityRecordExample editEntityRecordExample = new editEntityRecordExample(wfAPI);
editEntityRecordExample.Main();


//Executar uma atividade
executeActivityExample executeActivityExample = new executeActivityExample(wfAPI);
executeActivityExample.Main();




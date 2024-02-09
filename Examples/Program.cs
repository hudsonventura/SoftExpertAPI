using Examples;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SoftExpert.Workflow;

//TODO: dependendo dos caracteres do WorkflowTitle, a instancia não pode ser criada.
//TODO: anexar arquivo no form
//BUG: ao passa uma atividade para a função listAttachmentFromInstance, o SQL não traz resultados. Usar sem informar a atividade.


//Carregar as consigurações
string configs = $"{System.AppDomain.CurrentDomain.BaseDirectory.ToString()}/appsettings.json";
bool exists = File.Exists(configs);
if (!exists)
{
    Console.WriteLine("arquivo appsettings.json nao encontrado.");
    return;
}
var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Certifique-se de que o diretório atual seja o diretório do seu aplicativo
            .AddJsonFile("appsettings.json"); // Nome do arquivo de configuração

var appsettings = builder.Build();

string url = appsettings["url"].ToString();
string authorization = appsettings["authorization"].ToString();


//Implementação OPCIONAL de uma classe para acessar banco de dados. É necessário respeitar a interface SoftExpertAPI.Interfaces.IDataBase
//Necessário para algumas implementações fora do escopo da API padrão do SoftExpert.
ExampleOracleImplementation oracle = new ExampleOracleImplementation(appsettings);


//Criar instancia da API para utilizar na injeção de dependecias. Necessário informar a URL completa do SE e o header Authorization ou todos os headers.
//Se o parâmetro 'db' não for passado, alguns
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(
    url, 
    authorization, 
    db: oracle //opcional. Necessário para: listAttachmentFromInstance
);



//Criar instancia de processo - Veja o exemplo complexto no arquivo newWorkflowExample.cs
newWorkflowExample newWorkflowExample = new newWorkflowExample(wfAPI);
newWorkflowExample.Main();

/*
//Anexar um arquivo no menu de anexo
newAttachmentExample newAttachmentExample = new newAttachmentExample(wfAPI);
newAttachmentExample.Exemplo1_ArquivoUnico();
newAttachmentExample.Exemplo2_VariosArquivos();


//Editar um formulário
editEntityRecordExample editEntityRecordExample = new editEntityRecordExample(wfAPI);
editEntityRecordExample.Main();

//Adiciona um item em uma grid de um formulário
newChildEntityRecordExample newChildEntityRecordExample = new newChildEntityRecordExample(wfAPI);
newChildEntityRecordExample.Main();


//Executar uma atividade
executeActivityExample executeActivityExample = new executeActivityExample(wfAPI);
executeActivityExample.Main();




//Lista os anexos de uma instancia
listAttachmentFromInstanceExample listAttachmentFromInstanceExample = new listAttachmentFromInstanceExample(wfAPI);
listAttachmentFromInstanceExample.Main();


//marcar um anexo com syncronizado
setAttachmentSynced setAttachmentSynced = new setAttachmentSynced(wfAPI);
setAttachmentSynced.Main();


//lista os itens de uma grid de uma dada instancia
listGridItems listGridItems = new listGridItems(wfAPI);
listGridItems.Main();

//lista os parametros de uma instancia deworkflow
getWorkFlowData getWorkFlowData = new getWorkFlowData(wfAPI);
getWorkFlowData.Main();

//lista os campos e valores de um furmulário de um dados workflow
getFormData getFormData = new getFormData(wfAPI);
getFormData.Main();


//obtem os campos de um select box
getFormSelectBox getFormSelectBox = new getFormSelectBox(wfAPI);
getFormSelectBox.Main(); 

//marca uma atividade como executada com sucesso
markActivityAsExecuted markActivityAsExecuted = new markActivityAsExecuted(wfAPI);
markActivityAsExecuted.Main();*/


editChildEntityRecordExample editChildEntityRecordExample = new editChildEntityRecordExample(wfAPI);
editChildEntityRecordExample.Main();

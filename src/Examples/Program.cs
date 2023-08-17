using Examples;
using Newtonsoft.Json.Linq;
using SoftExpert.Workflow;

//TODO: dependendo dos caracteres do WorkflowTitle, a instancia não pode ser criada.
//TODO: anexar arquivo no anexo do lado esquerdo: CCF202323106
//TODO: anexar arquivo no form


//Carregar as consigurações
string appsettings = $"{System.AppDomain.CurrentDomain.BaseDirectory.ToString()}/appsettings.json";
bool exists = File.Exists(appsettings);
if (!exists)
{
    Console.WriteLine("arquivo appsettings.json nao encontrado.");
    return;
}
string json = System.IO.File.ReadAllText(appsettings);
var settings = JArray.Parse($"[{json}]").FirstOrDefault();

string url = settings["url"].ToString();
string authorization = settings["authorization"].ToString();



//Criar instancia da API para utilizar na injeção de dependecias. Necessário informar a URL completa do SE e o header Authorization ou todos os headers.
SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(url, authorization);




//Criar instancia de processo - Veja o exemplo complexto no arquivo newWorkflowExample.cs
newWorkflowExample newWorkflowExample = new newWorkflowExample(wfAPI);
newWorkflowExample.Main();


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




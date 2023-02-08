using Services;
using SoftExpert.Workflow;

var appsettings = Appsettings.GetSettings();

string url = appsettings["url"].ToString();
string authorization = appsettings["authorization"].ToString();


SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(url, authorization);


//CRIAR INSTANCIA DE PROCESSO
string ProcessID = "CCF";                       //identificador do processo
string WorkflowTitle = "Teste de integração"; ; //titulo da instancia a ser criado
string UserID = "00000000000";                  //matricula do usuario

newWorkflowResponse responseNewWF;
try
{
    responseNewWF = wfAPI.newWorkflow(ProcessID, WorkflowTitle, UserID);
}
catch (Exception erro)
{
    Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
    return;
}
string WorkflowID = responseNewWF.RecordID;
int codigoNewWorkFlow = responseNewWF.Code;
SoftExpert.SoftExpertResponse.STATUS sucessoNewWorkFlow = responseNewWF.Status;
string detalhesNewWorkflow = responseNewWF.Detail;







//EDITAR FORMULÁRIO
Dictionary<string, string> formulario = new Dictionary<string, string>();
formulario.Add("possuiendereco", "1"); //id do campo do formulário e valor (em string)
formulario.Add("ramal", "N/A");

Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>();
relacionamentos.Add("tipocliente", //idrelacionamento
    new Dictionary<string, string>() {
            //{ "campodoformdorelacionamento", "valor" },
            { "tipo", "PESSOA JURIDICA (CNPJ)" },
    }
);

string EntityID = "SOLCLIENTEFORNE";

editEntityRecordResponse entityResponse;
try
{
    entityResponse = wfAPI.editEntityRecord(WorkflowID, EntityID, formulario, relacionamentos);
}
catch (Exception erro)
{
    Console.WriteLine($"Não foi possivel editar o formulário. Erro: {erro.Message}");
    return;
}
int sucessoEntity = entityResponse.Code;
string detalhesEntity = entityResponse.Detail;





//EXECUÇÃO DE ATIVIDADE
string ActivityID = "ATIV-SOLCCF";      //ID da atividade do fluxograma
int ActionSequence = 3;                 //Sequence da ação da atividade. Veja na lista de ações da atividade

executeActivityResponse executeResponse;
try
{
    executeResponse = wfAPI.executeActivity(WorkflowID, ActivityID, ActionSequence, UserID);
}
catch (Exception erro)
{
    Console.WriteLine($"Não foi possivel executar a atividade. Erro: {erro.Message}");
    return;
}
var houveSucesso = executeResponse.Code;
var detalhes = executeResponse.Detail;
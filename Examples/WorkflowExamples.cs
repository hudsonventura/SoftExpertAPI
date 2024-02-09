using System.Text;
using Examples;
using Microsoft.Extensions.Configuration;
using SoftExpert.Workflow;
using SoftExpertAPI.Domain;

//TODO: dependendo dos caracteres do WorkflowTitle, a instancia não pode ser criada.
//TODO: anexar arquivo no form
//BUG: ao passa uma atividade para a função listAttachmentFromInstance, o SQL não traz resultados. Usar sem informar a atividade.


class WorkflowExamples{

    IConfiguration _appsettings;
    public WorkflowExamples(IConfiguration appsettings)
    {
        _appsettings = appsettings;
    }

    public enum Teste{
        NewWorkflow,
        NewAttachment,
        EditEntityRecord,
        NewChildEntityRecord,
        EditChildEntityRecord,
        ExecuteActivity
    }

    public void Execute(Teste tipo){

        #region Preparação
        string url = _appsettings["url"].ToString();
        string authorization = _appsettings["authorization"].ToString();


        //Implementação OPCIONAL de uma classe para acessar banco de dados. É necessário respeitar a interface SoftExpertAPI.Interfaces.IDataBase
        //Necessário para algumas implementações fora do escopo da API padrão do SoftExpert.
        ExampleOracleImplementation oracle = new ExampleOracleImplementation(_appsettings);


        //Criar instancia da API para utilizar na injeção de dependecias. Necessário informar a URL completa do SE e o header Authorization ou todos os headers.
        //Se o parâmetro 'db' não for passado, alguns
        SoftExpertWorkflowApi wfAPI = new SoftExpertWorkflowApi(
            url, 
            authorization, 
            db: oracle //opcional. Necessário para: listAttachmentFromInstance
        );
        #endregion

        #region Dados para uso nestes exemplos
        string ProcessID = "SPF";       //identificador do processo
        string WorkflowTitle = "O titulo da instancia de workflow";
        string UserID = "sistema.automatico";
        string ActivityID = "ATIV-SOLCCF"; //ID da atividade do processo
        string WorkflowID = "ID da instancia do processo";
        #endregion

        switch (tipo)
        {
            case Teste.NewWorkflow:
                #region Criar instancia de processo
                try
                {
                    newWorkflowResponse responseNewWF; responseNewWF = wfAPI.newWorkflow(ProcessID, WorkflowTitle, UserID);
                    string WorkflowID_gerado = responseNewWF.RecordID;
                    int codigoNewWorkFlow = responseNewWF.Code;
                    SoftExpert.SoftExpertResponse.STATUS sucessoNewWorkFlow = responseNewWF.Status;
                    string detalhesNewWorkflow = responseNewWF.Detail;
                    WorkflowID = WorkflowID_gerado;
                }
                catch (Exception erro)
                {
                    Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
                }
                #endregion
                break;

            case Teste.NewAttachment:
                #region Anexar arquivos no menu de anexo
                List<Anexo> Arquivos = new List<Anexo>(){ 
                    new Anexo()
                    {
                        FileName = "teste.txt",  //Nome do arquivo com a extensão
                        Content = Encoding.Unicode.GetBytes("Teste 123")   //Binário do arquivo em byte[]
                    },
                    new Anexo()
                    {
                        FileName = "",
                        Content = File.ReadAllBytes("largeLogo4.jpg")
                    }
                };


                try
                {
                    newAttachmentResponse newAttachment = wfAPI.newAttachment(WorkflowID, ActivityID, Arquivos);
                }
                catch (Exception erro)
                {
                    Console.WriteLine($"Não foi possivel anexar o arquivo a instancia de Workflow. Erro: {erro.Message}");
                }
                #endregion
                break;
            


            case Teste.ExecuteActivity:
                #region Executar uma atividade
                try
                {
                    int ActionSequence = 1;
                    executeActivityResponse executeResponse = wfAPI.executeActivity(WorkflowID, ActivityID, ActionSequence, UserID);
                    var houveSucesso = executeResponse.Code;
                var detalhes = executeResponse.Detail;
                }
                catch (Exception erro)
                {
                    Console.WriteLine($"Não foi possivel executar a atividade. Erro: {erro.Message}");
                    return;
                }
                
                #endregion
                break;
            default: break;
        }

        


        

        
                

        /*



        //Editar um formulário
        editEntityRecordExample editEntityRecordExample = new editEntityRecordExample(wfAPI);
        editEntityRecordExample.Main();

        //Adiciona um item em uma grid de um formulário
        newChildEntityRecordExample newChildEntityRecordExample = new newChildEntityRecordExample(wfAPI);
        newChildEntityRecordExample.Main();



        




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

    }
}
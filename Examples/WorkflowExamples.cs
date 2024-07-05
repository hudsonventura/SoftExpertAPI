using System.Text;
using Examples;
using Microsoft.Extensions.Configuration;
using SoftExpert;
using SoftExpert.Workflow;

//TODO: anexar arquivo no form
//TODO: Criar a checagem de instancia de banco de dados em todas as funções que dela necessitarem.
/*if (db is null) {
            throw new SoftExpertException("Uma instancia de banco de dados não foi informada na criação deste objeto. Crie forneça uma conexão com seu banco de dados implementando a interface SoftExpertAPI.Interfaces.IDataBase");
        }*/
        
class WorkflowExamples{
    
    IConfiguration _appsettings;
    SoftExpertWorkflowApi wfAPI;



    public WorkflowExamples(IConfiguration appsettings)
    {
        _appsettings = appsettings;

        #region Preparação dos parametros
        string url = _appsettings["url"].ToString();
        string authorization = _appsettings["authorization"].ToString();


        //Implementação OPCIONAL de uma classe para acessar banco de dados. É necessário respeitar a interface SoftExpertAPI.Interfaces.IDataBase
        //Necessário para algumas implementações fora do escopo da API padrão do SoftExpert.
        ExampleOracleImplementation db = new ExampleOracleImplementation(_appsettings);


        //Criar instancia da API para utilizar na injeção de dependecias. Necessário informar a URL completa do SE e o header Authorization ou todos os headers.
        //Se o parâmetro 'db' não for passado, alguns
        wfAPI = new SoftExpertWorkflowApi(
            url, 
            authorization, 
            db: db, //opcional. Necessário para: listAttachmentFromInstance
            login: _appsettings["login"].ToString(),
            pass: _appsettings["pass"].ToString(),
            domain: _appsettings["domain"].ToString()
        );
        #endregion

        
    }

    public enum Teste{
        NewWorkflow,
        NewAttachment,
        EditEntityRecord,
        NewChildEntityRecord,
        EditChildEntityRecord,
        ExecuteActivity,
        getFormData,
        getFormSelectBox,
        getWorkFlowData,
        listAttachmentFromInstance,
        ListGridItems,
        markActivityAsExecuted,
        SetAttachmentSynced,
        GetFileFromOID,
        GetFileFromFormField,
        ChangeWorflowTitle,
        CancelWorkflow,
        AddHistoryComment,
        unlinkActivityFromUser,
        reactivateWorkflow,
        returnWorkflow
    }

    public void Execute(Teste tipo){



        switch (tipo)
        {
            case Teste.NewWorkflow: newWorkflow();
                break;

            case Teste.NewAttachment: NewAttachment();
                break;

            case Teste.ExecuteActivity: ExecuteActivity();
                break;

            case Teste.EditEntityRecord: EditEntityRecord();
                break;

            case Teste.NewChildEntityRecord: NewChildEntityRecord();
                break;

            case Teste.EditChildEntityRecord: EditChildEntityRecord();
                break;

            case Teste.ListGridItems: ListGridItems();
                break;
                
            case Teste.getFormData: getFormData();
                break;

            case Teste.getFormSelectBox:
                break;

            case Teste.getWorkFlowData:
                break;

            case Teste.listAttachmentFromInstance:
                break;

            case Teste.markActivityAsExecuted:
                break;

            case Teste.SetAttachmentSynced: SetAttachmentSynced();
                break;

            case Teste.GetFileFromOID: GetFileFromOID();
                break;

            case Teste.GetFileFromFormField: GetFileFromFormField();
                break;

            case Teste.ChangeWorflowTitle: ChangeWorflowTitle();
                break;

            case Teste.CancelWorkflow: CancelWorkflow();
                break;
            
            case Teste.AddHistoryComment: AddHistoryComment();
                break;

            case Teste.unlinkActivityFromUser: unlinkActivityFromUser();
                break;

            case Teste.reactivateWorkflow: reactivateWorkflow();
                break;

            case Teste.returnWorkflow: returnWorkflow();
                break;

            default:
                throw new Exception("Tipo ainda não implementado");
                break;
        }
    }



    private void AddHistoryComment()
    {
        string WorkflowID = "CQN202400096";
        string Comment = "Téste com acento barras / e especiais $%&¨  e 'aspas simples' e \"aspas duplas\"";
        int CdUser = 9;

        try
        {
            wfAPI.addHistoryComment(WorkflowID, Comment);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void SetAttachmentSynced()
    {
        int cdattachment = 2153826;

        try
        {
            int number_rows_affected = wfAPI.setAttachmentSynced(cdattachment);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void CancelWorkflow()
    {
        string WorkflowID = "VBG202002801";
        string Explanation = "Fluxo com mais de 4 anos";
        try
        {
            wfAPI.cancelWorkflow(WorkflowID, Explanation);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void ChangeWorflowTitle()
    {
        string WorkflowID = "CCF202400005";
        var title = "Título de testes";
        try
        {
            wfAPI.changeWorflowTitle(WorkflowID, title);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void GetFileFromFormField()
    {
        string WorkflowID = "CCF202400005";
        string EntityID = "SOLCLIENTEFORNE";
        string FormField = "comprovante";

        try
        {
            var anexo =  wfAPI.getFileFromFormField(WorkflowID, EntityID, FormField);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void GetFileFromOID()
    {
        string oid = "ca0d26bcb6d294c48933e719f1959b86";

        try
        {
            var anexo =  wfAPI.getFileFromOID(oid);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void getFormData()
    {
        string WorkflowID = "CCF202400005";
        string EntityID = "SOLCLIENTEFORNE";

        try
        {
            var form =  wfAPI.getFormData(WorkflowID, EntityID);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }


    private void ListGridItems()
    {
        string WorkflowID = "IR090867";
        string MainEntityID = "IR";
        string ChildEntityID = "IRCOMENTARIO";
        string ChildOID = "OIDABCX0LIPROHT4H2";


        try
        {
            List<dynamic> itens_grid =  wfAPI.listGridItems(WorkflowID, MainEntityID, ChildEntityID, ChildOID);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void EditChildEntityRecord()
    {
        string MainEntityID = "IR";                                     //ID da tabela principal (entidade)
        string WorkflowID = "IR088482";                                 //ID da instancia
        string ChildRelationshipID = "ircomentariorel";                 //ID do relacionamento
        string ChildRecordOID = "7898431bf32fd35d5636146ce502d057";     //OID do registro   



        //os campos do fomrulário devem ser um dictionay de strings/strings, sendo nomeCampo/valorCampo
        Dictionary<string, string> formulario = new Dictionary<string, string>();
        formulario.Add("synced", "1"); //id do campo do formulário e valor (em string)
        formulario.Add("usuario", "123 teste"); //id do campo do formulário e valor (em string)


        try
        {
            wfAPI.editChildEntityRecord(WorkflowID, MainEntityID, ChildRelationshipID, ChildRecordOID, formulario);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void EditEntityRecord()
    {
        //os campos do fomrulário devem ser um dictionay de strings/strings, sendo nomeCampo/valorCampo
        Dictionary<string, string> formulario = new Dictionary<string, string>();
        formulario.Add("possuiendereco", "1"); //id do campo do formulário e valor (em string)
        formulario.Add("ramal", "N/A");
        //

        //os relacionamentos (selectbox) a serem prenchidos
        Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>();
        relacionamentos.Add("tipocliente", //idrelacionamento
            new Dictionary<string, string>() {
            //{ "campodoformdorelacionamento", "valor" },
            { "tipo", "PESSOA JURIDICA (CNPJ)" },
            }
        );


        //em caso de adicionar arquivos no formulário
        string filePath = "120.png";
        string FileName = "logo.png";                       //Nome do arquivo com a extensão
        byte[] FileContent = File.ReadAllBytes(filePath);   //Binário do arquivo
        Dictionary<string, Anexo> arquivos = new Dictionary<string, Anexo>();
        arquivos.Add("al5termoassinad", new Anexo() { FileName = FileName, Content = FileContent });


        try
        {
            string WorkflowID = "CCF202403731";
            string EntityID = "SOLCLIENTEFORNE";    //ID da tabela (entidade)

            wfAPI.editEntityRecord(WorkflowID, EntityID, formulario, relacionamentos, arquivos);
        }
        catch (SoftExpert.SoftExpertException erro)
        {
            throw;
        }
        catch(Exception erro){
            throw;
        }
    }

    private void ExecuteActivity()
    {
        try
        {
            string WorkflowID = "VBG202002800";
            string ActivityID = "Descarga";
            int ActionSequence = 1;
            string UserID = "sistema.automatico";

            wfAPI.executeActivity(WorkflowID, ActivityID, ActionSequence, UserID);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void NewAttachment()
    {
        var arquivo1 = new Anexo()
                            {
                                FileName = "teste.txt",  //Nome do arquivo com a extensão
                                Content = Encoding.UTF8.GetBytes("Teste 123")   //Binário do arquivo em byte[]
                            };

        var arquivo2 = new Anexo()
                            {
                                FileName = "largeLogo4.jpg",
                                Content = File.ReadAllBytes("largeLogo4.jpg")
                            };
        


        try
        {
            string WorkflowID = "SM2022030046";
            string ActivityID = "atvsolicitarmiro";

            wfAPI.newAttachment(WorkflowID, ActivityID, arquivo1);
            wfAPI.newAttachment(WorkflowID, ActivityID, arquivo2);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void newWorkflow()
    {
        try
        {
            string ProcessID = "CFTVs";
            string WorkflowTitle = "O titulo da instancia de workflow";
            string UserID = "sistema.automatico";

            string WorkflowID_gerado = wfAPI.newWorkflow(ProcessID, WorkflowTitle, UserID);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void NewChildEntityRecord(){
        //os campos do fomrulário devem ser um dictionay de strings/strings, sendo nomeCampo/valorCampo
        Dictionary<string, string> formulario = new Dictionary<string, string>();
        formulario.Add("iban", "1"); //id do campo do formulário e valor (em string)
        formulario.Add("banco", "N/A");


        try
        {
            string WorkflowID = "CCF202403731";
            string EntityID = "SOLCLIENTEFORNE";    //ID da tabela (entidade)
            string ChildRelationshipID = "invoices"; //ID da tabela da grid

            wfAPI.newChildEntityRecord(WorkflowID, EntityID, ChildRelationshipID,  formulario);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void unlinkActivityFromUser()
    {
        try
        {
            string WorkflowID = "PRO20240518";
            string ActivityID = "AnalisarDemanda";

            wfAPI.unlinkActivityFromUser(WorkflowID, ActivityID);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    private void reactivateWorkflow()
    {
        try
        {
            string WorkflowID = "CA2024000378";
            string Explanation = "Just a test";
            string ActivityID = "Usuário17125165826853";
            string UserID = "MATRICULA";

            wfAPI.reactivateWorkflow(WorkflowID, ActivityID, Explanation, UserID);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }


    private void returnWorkflow()
    {
        try
        {
            string WorkflowID = "PRO20240564";
            string Explanation = "Just a test";
            string ActivityID = "CriarSolicitacao";
            string UserID = "MATRICULA";

            wfAPI.returnWorkflow(WorkflowID, ActivityID, Explanation, UserID);
        }
        catch (SoftExpertException erro)
        {
            throw;
        }
        catch (Exception erro)
        {
            throw;
        }
    }
}
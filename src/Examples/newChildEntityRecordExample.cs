using SoftExpert.Workflow;

internal class newChildEntityRecordExample
{
    private SoftExpertWorkflowApi wfAPI;
    public newChildEntityRecordExample(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }

    

    internal void Main()
    {
        string EntityID = "SOLCLIENTEFORNE";    //ID da tabela (entidade)
        string WorkflowID = "CCF202300012";     //ID da instancia
        string ChildRelationshipID = "invoices";        //ID do relacionamento da grid



        //os campos do fomrulário devem ser um dictionay de strings/strings, sendo nomeCampo/valorCampo
        Dictionary<string, string> formulario = new Dictionary<string, string>();
        formulario.Add("iban", "1"); //id do campo do formulário e valor (em string)
        //formulario.Add("ramal", "N/A");
        

        //os relacionamentos (selectbox) a serem prenchidos
        /*Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>();
        relacionamentos.Add("tipocliente", //idrelacionamento
            new Dictionary<string, string>() {
            //{ "campodoformdorelacionamento", "valor" },
            { "tipo", "PESSOA JURIDICA (CNPJ)" },
            }
        );
        */


        //em caso de adicionar arquivos no formulário
        /*string filePath = "120.png";
        string FileName = "logo.png";                       //Nome do arquivo com a extensão
        byte[] FileContent = File.ReadAllBytes(filePath);   //Binário do arquivo
        Dictionary<string, Anexo> arquivos = new Dictionary<string, Anexo>();
        arquivos.Add("al5termoassinad", new Anexo() { FileName = FileName, Content = FileContent });
        */




        newChildEntityRecordResponse entityResponse;
        try
        {
            entityResponse = wfAPI.newChildEntityRecord(WorkflowID, EntityID, ChildRelationshipID,  formulario);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel editar a grid. Erro: {erro.Message}");
            return;
        }
        int sucessoEntity = entityResponse.Code;
        string detalhesEntity = entityResponse.Detail;
    }
}
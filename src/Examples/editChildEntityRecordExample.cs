using SoftExpert.Workflow;

internal class editChildEntityRecordExample
{
    private SoftExpertWorkflowApi wfAPI;
    public editChildEntityRecordExample(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }

    

    internal void Main()
    {
        string MainEntityID = "IR";                                     //ID da tabela principal (entidade)
        string WorkflowID = "IR088482";                                 //ID da instancia
        string ChildRelationshipID = "ircomentariorel";                 //ID do relacionamento
        string ChildRecordOID = "7898431bf32fd35d5636146ce502d057";     //OID do registro   



        //os campos do fomrulário devem ser um dictionay de strings/strings, sendo nomeCampo/valorCampo
        Dictionary<string, string> formulario = new Dictionary<string, string>();
        formulario.Add("synced", "1"); //id do campo do formulário e valor (em string)
        formulario.Add("usuario", "teste"); //id do campo do formulário e valor (em string)




        editChildEntityRecordResponse entityResponse;
        try
        {
            entityResponse = wfAPI.editChildEntityRecord(WorkflowID, MainEntityID, ChildRelationshipID, ChildRecordOID, formulario);
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
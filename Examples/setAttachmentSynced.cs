using SoftExpert;
using SoftExpert.Workflow;


namespace Examples;

internal class setAttachmentSynced
{
    private SoftExpertWorkflowApi wfAPI;

    public setAttachmentSynced(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        Anexo anexo = new Anexo();
        anexo.cdattachment = 2153826;


        try
        {
            int number_rows_affected = wfAPI.setAttachmentSynced(anexo);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
            return;
        }


    }
}

using SoftExpert;
using SoftExpert.Workflow;

namespace Examples;

internal class listAttachmentFromInstanceExample
{
    private SoftExpertWorkflowApi wfAPI;

    public listAttachmentFromInstanceExample(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string WorkflowID = "IR099727";

        List<Anexo> anexos;
        try
        {
            anexos =  wfAPI.listAttachmentFromInstance(WorkflowID);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
            return;
        }

        foreach (var anexo in anexos)
        {
            File.WriteAllBytes($"{Environment.CurrentDirectory}/{anexo.FileName}", anexo.Content);
        }
    }
}

using SoftExpert.Workflow;
using SoftExpertAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        string WorkflowID = "TESTE2023000001";

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
            File.WriteAllBytes($"{Environment.CurrentDirectory}{anexo.FileName}", anexo.Content);
        }
    }
}

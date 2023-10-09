using SoftExpert.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class markActivityAsExecuted
{
    private SoftExpertWorkflowApi wfAPI;

    public markActivityAsExecuted(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string WorkflowID = "IR154585";       //identificador do processo
        string ActivityID = "envioIntegracaoComentario"; //titulo da instancia a ser criado

        try
        {
            int response = wfAPI.markActivityAsExecuted(WorkflowID, ActivityID);
            return;
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
            return;
        }

    }
}

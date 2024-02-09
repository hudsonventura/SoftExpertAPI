using SoftExpert.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class newWorkflowExample
{
    private SoftExpertWorkflowApi wfAPI;

    public newWorkflowExample(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string ProcessID = "SPF";       //identificador do processo
        string WorkflowTitle = "17/10/2023 14:08 - hudson.ventura@outlook.com - Nome do solicitante"; //titulo da instancia a ser criado
        string UserID = "sistema.automatico";  //matricula do usuario

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
    }
}

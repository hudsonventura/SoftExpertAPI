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
        string ProcessID = "CCF";       //identificador do processo
        string WorkflowTitle = "Teste"; //titulo da instancia a ser criado
        string UserID = "00000000000";  //matricula do usuario

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

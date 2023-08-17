using SoftExpert.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class executeActivityExample
{
    private SoftExpertWorkflowApi wfAPI;

    public executeActivityExample(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string ActivityID = "ATIV-SOLCCF";      //ID da atividade do fluxograma
        int ActionSequence = 3;                 //Sequence da ação da atividade. Veja na lista de ações da atividade
        string WorkflowID = "CCF202323106";     //ID da instancia
        string UserID = "";

        executeActivityResponse executeResponse;
        try
        {
            executeResponse = wfAPI.executeActivity(WorkflowID, ActivityID, ActionSequence, UserID);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel executar a atividade. Erro: {erro.Message}");
            return;
        }
        var houveSucesso = executeResponse.Code;
        var detalhes = executeResponse.Detail;
    }
}

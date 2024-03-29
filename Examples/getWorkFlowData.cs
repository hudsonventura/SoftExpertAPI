﻿using SoftExpert.Workflow;

namespace Examples;

internal class getWorkFlowData
{
    private SoftExpertWorkflowApi wfAPI;

    public getWorkFlowData(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string WorkflowID = "IR090867";

        try
        {
            var wf = wfAPI.getWorkFlowData(WorkflowID);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
            return;
        }


    }
}

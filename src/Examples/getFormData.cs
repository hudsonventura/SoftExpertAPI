﻿using SoftExpert.Workflow;
using SoftExpertAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class getFormData
{
    private SoftExpertWorkflowApi wfAPI;

    public getFormData(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string WorkflowID = "IR090867";
        string EntityID = "IR";


        try
        {
            var form =  wfAPI.getFormData(WorkflowID, EntityID);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
            return;
        }


    }
}

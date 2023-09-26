using SoftExpert.Workflow;
using SoftExpertAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class listGridItems
{
    private SoftExpertWorkflowApi wfAPI;

    public listGridItems(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string WorkflowID = "IR090867";
        string MainEntityID = "IR";
        string ChildEntityID = "IRCOMENTARIO";
        string ChildOID = "OIDABCX0LIPROHT4H2";


        List<dynamic> itens_grid;
        try
        {
            itens_grid =  wfAPI.listGridItems(WorkflowID, MainEntityID, ChildEntityID, ChildOID);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
            return;
        }


    }
}

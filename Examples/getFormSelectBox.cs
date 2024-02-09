using SoftExpert.Workflow;
using SoftExpertAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class getFormSelectBox
{
    private SoftExpertWorkflowApi wfAPI;

    public getFormSelectBox(SoftExpertWorkflowApi wfAPI)
    {
        this.wfAPI = wfAPI;
    }


    internal void Main()
    {
        string oid = "6a6af8402eb7a9ec91486d2e3bd23f78";
        string EntityID = "irsubcategoria";


        try
        {
            var form =  wfAPI.getFormSelectBox(oid, EntityID);
        }
        catch (Exception erro)
        {
            Console.WriteLine($"Não foi possivel criar o workflow. Erro: {erro.Message}");
            return;
        }


    }
}

using Microsoft.Extensions.Configuration;
using SoftExpert.Workflow;
using SoftExpertAPI.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit.Abstractions;

namespace Testes_Worflow;

public class Testes_Workflow
{
    ITestOutputHelper console;
    IConfiguration _appsettings;
    SoftExpert.Workflow.SoftExpertWorkflowApi _softExpertApi;

    //parametros ficticios utilizados apenas para os testes
    string ProcessID = "CCF";
    string WorkflowID = "CCF202300011";
    string EntityID = "SOLCLIENTEFORNE";
    string ActivityID = "ATIV-SOLCCF";

    public Testes_Workflow(ITestOutputHelper output){
        _appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string url = _appsettings["url"];
        _softExpertApi = new SoftExpert.Workflow.SoftExpertWorkflowApi(url, _appsettings["Authorization"]);
        console = output;
    }

    /// <summary>
    /// Criar uma instancia de um processo
    /// </summary>
    [Fact]
    public void WF_01_newWorkflow_Success()
    {
        var a = _softExpertApi.newWorkflow(ProcessID, "Teste de unidade automatizado da biblioteca SoftExpertAPI");
            
        if (a == null) {
            Assert.Fail("O retorno foi nulo");
        }
        console.WriteLine(a);

        Assert.IsType<string>(a);
    }

    /// <summary>
    /// Criar uma instancia de um processo inexistente
    /// </summary>
    [Fact]
    public void WF_01_newWorkflow_Error()
    {
        try
        {
            var a = _softExpertApi.newWorkflow("XPTOSSS", "Teste de unidade automatizado da biblioteca SoftExpertAPI");
        }
        catch (SoftExpert.SoftExpertException)
        {
            Assert.True(1==1);
        }
    }



    [Fact]
    public void WF_02_editEntityRecord_SemRelacionamento_SemAnexo()
        {
            Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
                { "observacoes", "Ordem02_editEntityRecord_SemRelacionamento_SemAnexo"},
                //{ "solempresa", "0004 - teste nome empresa"}
            };
            editEntityRecordResponse b = _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList);


            if (b == null)
            {
                Assert.Fail("O retorno foi nulo");
            }

            Console.WriteLine(b.Detail);
            Console.WriteLine(b.Code);

            if (b.Code == 1)
            {
                //Assert.True(b.Detail);
            }

            Assert.Fail(b.Detail);
        }



}
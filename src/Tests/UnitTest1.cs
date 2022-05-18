using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SoftExpert.Workflow;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    public class Tests
    {
        IConfiguration _configs;

        SoftExpert.Workflow.SoftExpertWorkflowApi _softExpertApi;

        public Tests()
        {
            _configs = new ConfigurationBuilder()
               .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory.ToString())
               .AddJsonFile(@"appsettings.json", false, false)
               .AddEnvironmentVariables()
               .Build();
        }

        [SetUp]
        public void Setup()
        {
            string url = _configs["url"];
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", _configs["Authorization"]);
            _softExpertApi = new SoftExpert.Workflow.SoftExpertWorkflowApi(url, _configs["Authorization"]);
        }

        [Test]
        public void newWorkflow()
        {
            var a = _softExpertApi.newWorkflow("SM", "Teste de unidade automatizado da biblioteca SoftExpertAPI");
            
            if (a == null) {
                Assert.Fail("O retorno foi nulo");
            }

            if (a.Code == 1) {
                Assert.Pass($"{a.Detail} - {a.RecordID}");
            }

            Assert.Fail(a.Detail);
        }

        [Test]
        public void editEntityRecord()
        {
            string WorkflowID = "FI2022005656";

            string EntityID = "SOLINDFISCAL";

            Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>();
            EntityAttributeList.Add("supervisornome", "teste 4");
            EntityAttributeList.Add("solempresa", "0004 - teste nome empresa");

            Dictionary<string, Dictionary<string, string>> RelationshipList = new Dictionary<string, Dictionary<string, string>>();
            RelationshipList.Add("soltipo", 
                new Dictionary<string, string>() {
                { "tipo", "Processamento de Romaneio" },
                }
            );

            editEntityRecordResponse b = _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList, RelationshipList);

            if (b == null)
            {
                Assert.Fail("O retorno foi nulo");
            }

            Console.WriteLine(b.Detail);
            Console.WriteLine(b.Code);

            if (b.Code == 1)
            {
                Assert.Pass(b.Detail);
            }

            Assert.Fail(b.Detail);
        }


        [Test]
        public void executeActivity_SolicitarSM()
        {
            string WorkflowID = "SM2022029975";
            string ActivityID = "atvsolicitarmiro";
            int ActionSequence = 2;

            executeActivityResponse a = null;
            try
            {
                a = _softExpertApi.executeActivity(WorkflowID, ActivityID, ActionSequence);
            }
            catch (Exception erro)
            {
                Assert.Fail(erro.Message);
            }

            if (a == null)
            {
                Assert.Fail("O retorno foi nulo");
            }

            if (a.Code == 1)
            {
                Assert.Pass(a.Detail);
            }

            Assert.Fail(a.Detail);
        }
        
        [Test]
        public void executeActivity_RetornarSM()
        {
            string WorkflowID = "SM2022029975";
            string ActivityID = "atvlancarmiro";
            int ActionSequence = 1;

            executeActivityResponse a = null;
            try
            {
                a = _softExpertApi.executeActivity(WorkflowID, ActivityID, ActionSequence);
            }
            catch (Exception erro)
            {
                Assert.Fail(erro.Message);
            }



            if (a == null)
            {
                Assert.Fail("O retorno foi nulo");
            }

            if (a.Code == 1)
            {
                Assert.Pass(a.Detail);
            }

            Assert.Fail(a.Detail);
        }

    }
}
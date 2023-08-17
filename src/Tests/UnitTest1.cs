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
        string WorkflowID;
        string EntityID;
        string ProcessID;
        string ActivityID;
        int ActionSequence;
        string UserID;

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
            _softExpertApi = new SoftExpert.Workflow.SoftExpertWorkflowApi(url, _configs["Authorization"]);

            ProcessID = "CCF";
            WorkflowID = "CCF202300011";
            EntityID = "SOLCLIENTEFORNE";
            ActivityID = "ATIV-SOLCCF";
            ActionSequence = 2;
            UserID = "00000000000";
        }

        /// <summary>
        /// Teste
        /// </summary>
        [Test]
        public void Ordem01_newWorkflow()
        {
            var a = _softExpertApi.newWorkflow(ProcessID, "Teste de unidade automatizado da biblioteca SoftExpertAPI");
            
            if (a == null) {
                Assert.Fail("O retorno foi nulo");
            }

            if (a.Code == 1) {
                Assert.Pass($"{a.Detail} - {a.RecordID}");
            }

            Assert.Fail(a.Detail);
        }


        [Test]
        public void Ordem02_editEntityRecord_SemRelacionamento_SemAnexo()
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
                Assert.Pass(b.Detail);
            }

            Assert.Fail(b.Detail);
        }

        [Test]
        public void Ordem03_editEntityRecord_ComRelacionamentoSemAnexo()
        {
            Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
                { "observacoes", "Ordem03_editEntityRecord_ComRelacionamentoSemAnexo"},
                //{ "solempresa", "0004 - teste nome empresa"}
            };


            Dictionary<string, Dictionary<string, string>> RelationshipList = new Dictionary<string, Dictionary<string, string>>();
             RelationshipList.Add("empresa", 
                new Dictionary<string, string>() {
                 { "razao", "0032 - AMAGGI INSUMOS E AGRÍCOLA" },
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
        public void Ordem04_editEntityRecord_SemRelacionamentoComAnexo()
        {
            Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
                { "observacoes", "teste Ordem04_editEntityRecord_SemRelacionamentoComAnexo_LA"},
            };

            byte[] fileBytes = File.ReadAllBytes("120.png");

            Dictionary<string, Anexo> arquivos = new Dictionary<string, Anexo>();
            arquivos.Add("comprovante", new Anexo()
            {
                FileName = "120.png",
                Content = fileBytes
            });




            editEntityRecordResponse b = _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList, null, arquivos);

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
        public void Ordem05_editEntityRecord_ComRelacionamentoComAnexo()
        {
            Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
                { "observacoes", "teste Ordem05_editEntityRecord_ComRelacionamentoComAnexo_LA"},
            };

            Dictionary<string, Dictionary<string, string>> RelationshipList = new Dictionary<string, Dictionary<string, string>>();
            RelationshipList.Add("empresa",
               new Dictionary<string, string>() {
                 { "razao", "0032 - AMAGGI INSUMOS E AGRÍCOLA" },
               }
           );

            byte[] fileBytes = File.ReadAllBytes("120.png");
            Dictionary<string, Anexo> arquivos = new Dictionary<string, Anexo>();
            arquivos.Add("comprovante", new Anexo()
            {
                FileName = "120.png",
                Content = fileBytes
            });




            editEntityRecordResponse b = _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList, RelationshipList, arquivos);

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
        public void Ordem06_newAttachment_SemCPF()
        {

            byte[] fileBytes = File.ReadAllBytes("120.png");
            var arquivo = new Anexo()
            {
                FileName = "120.png",
                Content = fileBytes
            };


            newAttachmentResponse b = _softExpertApi.newAttachment(WorkflowID, ActivityID, arquivo);

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
        public void Ordem07_newAttachment_ComCPF()
        {

            byte[] fileBytes = File.ReadAllBytes("120.png");
            var arquivo = new Anexo()
            {
                FileName = "120.png",
                Content = fileBytes
            };


            newAttachmentResponse b = _softExpertApi.newAttachment(WorkflowID, ActivityID, arquivo, UserID);

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
        public void Ordem08_executeActivity()
        {
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
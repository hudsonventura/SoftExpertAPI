using Microsoft.Extensions.Configuration;
using SoftExpert;
using Examples;
using System.Text;
using Xunit.Abstractions;

namespace Testes_Worflow;

public class Testes_Workflow
{
    ITestOutputHelper console;
    IConfiguration _appsettings;
    SoftExpert.Workflow.SoftExpertWorkflowApi _softExpertApi;

    //parametros ficticios utilizados apenas para os testes
    string ProcessID = "CCF";
    string WorkflowID = "CCF202400005";
    string EntityID = "SOLCLIENTEFORNE";
    string ActivityID = "ATIV-SOLCCF";

    public Testes_Workflow(ITestOutputHelper output){
        _appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        ExampleOracleImplementation db = new ExampleOracleImplementation(_appsettings);

        string baseURL = _appsettings["url"];
        string authorization = _appsettings["Authorization"];
        _softExpertApi = new SoftExpert.Workflow.SoftExpertWorkflowApi(baseURL, authorization, db);
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
    public void WF_02_editEntityRecord_Simples()
    {
        Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
            { "observacoes", "Teste de unidade automatizado da biblioteca SoftExpertAPI"},
        };

        try
        {
            _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }


    [Fact]
    public void WF_03_editEntityRecord_ComRelacionamento()
    {
        Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
            { "observacoes", "Teste de unidade automatizado da biblioteca SoftExpertAPI"},
        };

        Dictionary<string, Dictionary<string, string>> relacionamentos = new Dictionary<string, Dictionary<string, string>>(){
            {
                "tipocliente", //idrelacionamento
                    new Dictionary<string, string>() {
                        //{ "campodoformdorelacionamento", "valor" },
                        { "tipo", "PESSOA JURIDICA (CNPJ)" },
                    }
            },
            {
                "empresa", //idrelacionamento
                    new Dictionary<string, string>() {
                        { "razao", "FERE HOLDINGS GESTORA RURAL LTDA" },
                    }
            }
        };


        try
        {
            _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList, relacionamentos);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }

    [Fact]
    public void WF_04_editEntityRecord_ComAnexo()
    {
        Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
            { "observacoes", "Teste de unidade automatizado da biblioteca SoftExpertAPI"},
        };

        Dictionary<string, Anexo> arquivos = new Dictionary<string, Anexo>();
        arquivos.Add("comprovante", new Anexo() { FileName = "Teste.txt", Content =  Encoding.UTF8.GetBytes("Conteúdo deve ser um array de bytes (byte[])")});

        try
        {
            _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList, null, arquivos);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }

    [Fact]
    public void WF_05_GetFile_FromFormField()
    {
        string WorkflowID = "CCF202400005";
        string EntityID = "SOLCLIENTEFORNE";
        string FormField = "comprovante";

        try
        {
            var anexo =  _softExpertApi.getFileFromFormField(WorkflowID, EntityID, FormField);

            Assert.NotNull(anexo.FileName);
            Assert.NotNull(anexo.Content);

            Assert.IsType<string>(anexo.FileName);
            Assert.IsType<byte[]>(anexo.Content);
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    [Fact]
    public void WF_05_GetFile_FromOID()
    {
        string oid = "ca0d26bcb6d294c48933e719f1959b86";

        try
        {
            var anexo =  _softExpertApi.getFileFromOID(oid);

            Assert.NotNull(anexo.FileName);
            Assert.NotNull(anexo.Content);

            Assert.IsType<string>(anexo.FileName);
            Assert.IsType<byte[]>(anexo.Content);
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    [Fact]
    public void WF_06_listAttachmentFromInstance()
    {
        string WorkflowID = "IR088482";

        try
        {
            var arquivos =  _softExpertApi.listAttachmentFromInstance(WorkflowID);


            Assert.NotNull(arquivos);
            Assert.True(arquivos.Count > 0);

            Assert.IsType<string>(arquivos[0].FileName);
            Assert.IsType<byte[]>(arquivos[0].Content);
        }
        catch (Exception erro)
        {
            throw;
        }
    }
}
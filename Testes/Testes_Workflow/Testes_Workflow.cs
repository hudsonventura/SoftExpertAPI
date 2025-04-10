using Microsoft.Extensions.Configuration;
using SoftExpertAPI;
using Examples;
using System.Text;
using Xunit.Abstractions;

namespace Testes_Worflow;

public class Testes_Workflow
{
    ITestOutputHelper console;
    IConfiguration _appsettings;
    SoftExpertAPI.SoftExpertWorkflowApi _softExpertApi;

    //parametros ficticios utilizados apenas para os testes
    string ProcessID = "CCF";
    string WorkflowID = "CCF202400006";
    string EntityID = "SOLCLIENTEFORNE";
    string ActivityID = "ATIV-SOLCCF";

    string ChieldEntityID = "invoices";

    int ActionSequence_Error = 1;
    int ActionSequence_Success = 2;

    int cduser = 9;

    string oidFile = "ca0d26bcb6d294c48933e719f1959b86";

    public Testes_Workflow(ITestOutputHelper output){
        _appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        ExampleOracleImplementation _db = new ExampleOracleImplementation(_appsettings);

        SoftExpertAPI.Configurations configs = new Configurations(){
            baseUrl = _appsettings["url"],
            token = _appsettings["authorization"],

            //OPCIONAIS
            db = _db,                 //Necessário para funções que acessam o banco de dados. Implementar a interface SoftExpert.IDataBase
            //downloader = _downloader, //Necessário para caso os arquivos do SE fiquem em um diretório controlado. Implementar a interface SoftExpert.IFileDownload
        };

        _softExpertApi = new SoftExpertAPI.SoftExpertWorkflowApi(configs);
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
        catch (SoftExpertAPI.SoftExpertException)
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
            var anexo =  _softExpertApi.GetFileFromFormField(WorkflowID, EntityID, FormField);

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
        try
        {
            var anexo =  _softExpertApi.GetFileFromOID(oidFile);

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
            var arquivos =  _softExpertApi.ListAttachmentFromInstance(WorkflowID);


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

    [Fact]
    public void WF_07_ChangeWorflowTitle()
    {
        string WorkflowID = "CCF202400005";
        var title = "Título de testes";

        try
        {
            var value =  _softExpertApi.ChangeWorflowTitle(WorkflowID, title);


            Assert.NotNull(value);
            Assert.IsType<int>(value);
            Assert.True(value == 1);
        }
        catch (Exception erro)
        {
            throw;
        }
    }

    [Fact]
    public void WF_08_SetAttachmentSynced()
    {
        int cdattachment = 2153826;

        int number_rows_affected = _softExpertApi.SetAttachmentSynced(cdattachment);

        Assert.NotNull(number_rows_affected);
        Assert.IsType<int>(number_rows_affected);
    }

    



    [Fact]
    public void WF_09_newChildEntityRecord()
    {
        Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
            { "pais", "Brazuca"},
            { "chavedobanco", "101020203030"},
            { "contabancaria", "10203040506070"},
            { "iban", "4654897892510321654897897451004510780417891561984"},
        };
        try
        {
            _softExpertApi.newChildEntityRecord(WorkflowID, EntityID, ChieldEntityID, EntityAttributeList, null);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }


    [Fact]
    public void WF_10_newAttachment()
    {
        Anexo arquivo = new Anexo() { FileName = "Teste.txt", Content =  Encoding.UTF8.GetBytes("Conteúdo deve ser um array de bytes (byte[])")};

        try
        {
            _softExpertApi.newAttachment(WorkflowID, ActivityID, arquivo);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }


    [Fact]
    public void WF_11_addHistoryComment()
    {
        try
        {
            _softExpertApi.addHistoryComment("NOVAEMP001292", "Comentário de testes com $%@ caractestes especiais, 'aspas simples' e \"aspas duplas\"", cduser);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }




    [Fact]
    public void WF_99_excuteActivity_Error()
    {
        try
        {
            _softExpertApi.executeActivity(WorkflowID, ActivityID, ActionSequence_Error);
        }
        catch (System.Exception error)
        {
            Assert.True(1==1);
        }
    }

    [Fact]
    public void WF_99_excuteActivity_Success()
    {
        try
        {
            _softExpertApi.executeActivity(WorkflowID, ActivityID, ActionSequence_Success);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }
}
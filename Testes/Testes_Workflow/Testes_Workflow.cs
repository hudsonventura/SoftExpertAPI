using Microsoft.Extensions.Configuration;
using SoftExpertAPI;
using Domain;
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
    string WorkflowID = "CCF202614358";
    string EntityID = "SOLCLIENTEFORNE";
    string ActivityID = "ATIV-centralCadastro";

    string ChieldEntityID = "invoices";

    int ActionSequence_Error = 1;
    int ActionSequence_Success = 2;

    string iduser = "sistema.teste";

    string oidFile = "0e6fbe048a635aaf00deea99b9f3bbc3";

    public Testes_Workflow(ITestOutputHelper output){
        _appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();


        SoftExpertAPI.Configurations configs = new Configurations(){
            baseUrl = _appsettings["url"],
            login = _appsettings["user"],
            pass = _appsettings["pass"],
            domain = _appsettings["domain"],
            token = _appsettings["token"],
            downloader = new MockFileDownload(),
        };

        if (!string.IsNullOrWhiteSpace(_appsettings["authorization"]))
        {
            configs.token = _appsettings["authorization"];
        }

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
        Console.WriteLine(a);

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
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tenta editar formulário de instância inexistente e espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_02_editEntityRecord_Error()
    {
        Dictionary<string, string> EntityAttributeList = new Dictionary<string, string>() {
            { "observacoes", "Teste de unidade SoftExpertAPI"},
        };

        try
        {
            _softExpertApi.editEntityRecord("INSTANCIA_INEXISTENTE_XYZ", EntityID, EntityAttributeList);
            Assert.Fail("Era esperado SoftExpertException para instância inexistente");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
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
            // {
            //     "empresa", //idrelacionamento
            //         new Dictionary<string, string>() {
            //             { "razao", "FERE HOLDINGS GESTORA RURAL LTDA" },
            //         }
            // }
        };


        try
        {
            _softExpertApi.editEntityRecord(WorkflowID, EntityID, EntityAttributeList, relacionamentos);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
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
            _softExpertApi.editEntityRecord("CCF202314173", EntityID, EntityAttributeList, null, arquivos);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
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
            console.WriteLine($"Erro: {erro.Message}");
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
            _softExpertApi.addHistoryComment(WorkflowID, "Comentário de testes com $%@ caractestes especiais, 'aspas simples' e \"aspas duplas\"", iduser, ActivityID);
            Assert.True(1==1);
        }
        catch (System.Exception error)
        {
            throw;
        }
    }

    /// <summary>
    /// Comentário no histórico usando sobrecarga com iduser (int)
    /// </summary>
    [Fact]
    public void WF_11_addHistoryComment_ByCdUser()
    {
        int cduser = 88;
        try
        {
            _softExpertApi.addHistoryComment(WorkflowID, "Comentário via cduser (int)", cduser, ActivityID);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// addHistoryComment com instância inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_11_addHistoryComment_Error()
    {
        try
        {
            _softExpertApi.addHistoryComment("INSTANCIA_INEXISTENTE_XYZ", "teste", iduser, ActivityID);
            Assert.Fail("Era esperado SoftExpertException");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
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

    /// <summary>
    /// Altera o iniciador de uma instância em andamento via SOAP editWorkflowData
    /// </summary>
    [Fact]
    public void WF_12_AlterUserStart_Success()
    {
        string workflowID = "PRO20260407";
        //string explanation = "Teste unitário SoftExpertAPI - AlterUserStart";
        string userID = "sistema.automatico";

        try
        {
            _softExpertApi.AlterUserStart(workflowID, userID, explanation: null);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tenta alterar iniciador de instância inexistente e espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_12_AlterUserStart_InvalidWorkflow_Error()
    {
        string workflowID = "INSTANCIA_INEXISTENTE_XYZ";
        string userID = "sistema.automatico";

        try
        {
            _softExpertApi.AlterUserStart(workflowID, userID, explanation: null);
            Assert.Fail("Era esperado SoftExpertException para instância inexistente");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Tenta alterar iniciador com usuário inexistente e espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_12_AlterUserStart_InvalidUser_Error()
    {
        string workflowID = "PRO20250002";
        string userID = "USUARIO_INEXISTENTE_XYZ";

        try
        {
            _softExpertApi.AlterUserStart(workflowID, userID, explanation: null);
            Assert.Fail("Era esperado SoftExpertException para usuário inexistente");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }



    /// <summary>
    /// Edita registro de tabela via SOAP editTableRecord
    /// </summary>
    [Fact]
    public void WF_14_editTableRecord_Success()
    {
        Dictionary<string, string> fields = new Dictionary<string, string>()
        {
            { "responsavelnome", "Teste unitário SoftExpertAPI - editTableRecord" },
        };
        string tableFieldOID = "79c34d0ec771902834cc271ee08c01d";

        try
        {
            _softExpertApi.editTableRecord(iduser, "SOLPROCESSO", tableFieldOID, fields);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// editTableRecord com OID inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_14_editTableRecord_Error()
    {
        Dictionary<string, string> fields = new Dictionary<string, string>()
        {
            { "observacoes", "Teste unitário SoftExpertAPI - editTableRecord" },
        };

        try
        {
            _softExpertApi.editTableRecord(iduser, EntityID, "OID_INEXISTENTE", fields);
            Assert.Fail("Era esperado SoftExpertException");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Reativa uma instância de workflow suspensa
    /// </summary>
    [Fact]
    public void WF_15_reactivateWorkflow_Success()
    {
        string workflowID = "SA202514268";
        string activityID = "ATIV-SOLACESSO";
        string explanation = "Teste unitário SoftExpertAPI - reactivateWorkflow";
        string userID = iduser;

        try
        {
            _softExpertApi.reactivateWorkflow(workflowID, activityID, explanation, userID);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tenta reativar instância inexistente — espera Exception
    /// </summary>
    [Fact]
    public void WF_15_reactivateWorkflow_Error()
    {
        string workflowID = "INSTANCIA_INEXISTENTE_XYZ";
        string activityID = "ATIVIDADE_INEXISTENTE";
        string explanation = "Teste unitário SoftExpertAPI - reactivateWorkflow";
        string userID = iduser;

        try
        {
            _softExpertApi.reactivateWorkflow(workflowID, activityID, explanation, userID);
            Assert.Fail("Era esperado Exception para instância inexistente");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Delega a atividade de uma instância para outro usuário
    /// </summary>
    [Fact]
    public void WF_16_delegateWorkflow_Success()
    {
        string workflowID = "SM2026108124";
        string activityID = "atvsolicitarmiro";
        string explanation = "Teste unitário SoftExpertAPI - delegateWorkflow";
        int cduser = 9;

        try
        {
            _softExpertApi.delegateWorkflow(workflowID, activityID, explanation, cduser);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tenta delegar instância inexistente — espera Exception
    /// </summary>
    [Fact]
    public void WF_16_delegateWorkflow_Error()
    {
        string workflowID = "INSTANCIA_INEXISTENTE_XYZ";
        string activityID = "ATIVIDADE_INEXISTENTE";
        string explanation = "Teste unitário SoftExpertAPI - delegateWorkflow";
        int cduser = 9;

        try
        {
            _softExpertApi.delegateWorkflow(workflowID, activityID, explanation, cduser);
            Assert.Fail("Era esperado Exception para instância inexistente");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Encerra uma instância de workflow em andamento
    /// </summary>
    [Fact]
    public void WF_17_finishWorkflow_Success()
    {
        string workflowID = "SM2026108124";
        string explanation = "Teste unitário SoftExpertAPI - finishWorkflow";
        string userID = iduser;

        try
        {
            _softExpertApi.finishWorkflow(workflowID, explanation, userID);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tenta encerrar instância inexistente — espera Exception
    /// </summary>
    [Fact]
    public void WF_17_finishWorkflow_Error()
    {
        string workflowID = "INSTANCIA_INEXISTENTE_XYZ";
        string explanation = "Teste unitário SoftExpertAPI - finishWorkflow";
        string userID = iduser;

        try
        {
            _softExpertApi.finishWorkflow(workflowID, explanation, userID);
            Assert.Fail("Era esperado Exception para instância inexistente");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Retorna uma instância de workflow para uma atividade anterior
    /// </summary>
    [Fact]
    public void WF_18_returnWorkflow_Success()
    {
        string workflowID = "PRO20240564";
        string activityID = "CriarSolicitacao";
        string explanation = "Teste unitário SoftExpertAPI - returnWorkflow";
        string userID = iduser;

        try
        {
            _softExpertApi.returnWorkflow(workflowID, activityID, explanation, userID);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tenta retornar instância inexistente — espera Exception
    /// </summary>
    [Fact]
    public void WF_18_returnWorkflow_Error()
    {
        string workflowID = "INSTANCIA_INEXISTENTE_XYZ";
        string activityID = "ATIVIDADE_INEXISTENTE";
        string explanation = "Teste unitário SoftExpertAPI - returnWorkflow";
        string userID = iduser;

        try
        {
            _softExpertApi.returnWorkflow(workflowID, activityID, explanation, userID);
            Assert.Fail("Era esperado Exception para instância inexistente");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Edita item de grid (child entity) via SOAP editChildEntityRecord
    /// </summary>
    [Fact]
    public void WF_19_editChildEntityRecord_Success()
    {
        string workflowID = "IR088482";
        string mainEntityID = "IR";
        string childRelationshipID = "ircomentariorel";
        string childRecordOID = "7898431bf32fd35d5636146ce502d057";
        Dictionary<string, string> fields = new Dictionary<string, string>()
        {
            { "synced", "1" },
            { "usuario", "teste SoftExpertAPI" },
        };

        try
        {
            _softExpertApi.editChildEntityRecord(workflowID, mainEntityID, childRelationshipID, childRecordOID, fields);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// editChildEntityRecord com instância inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_19_editChildEntityRecord_Error()
    {
        Dictionary<string, string> fields = new Dictionary<string, string>()
        {
            { "synced", "1" },
        };

        try
        {
            _softExpertApi.editChildEntityRecord("INSTANCIA_INEXISTENTE_XYZ", "IR", "ircomentariorel", "OID_INEXISTENTE", fields);
            Assert.Fail("Era esperado SoftExpertException");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Consulta status da instância via dataset
    /// </summary>
    [Fact]
    public void WF_20_GetWorflowStatus_Success()
    {
        try
        {
            var status = _softExpertApi.GetWorflowStatus(WorkflowID);
            Assert.True(Enum.IsDefined(typeof(WFStruct.WFStatus), status));
            console.WriteLine($"Status: {status}");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// GetWorflowStatus com instância inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_20_GetWorflowStatus_Error()
    {
        try
        {
            _softExpertApi.GetWorflowStatus("INSTANCIA_INEXISTENTE_XYZ");
            Assert.Fail("Era esperado SoftExpertException");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Lista atividades em andamento da instância
    /// </summary>
    [Fact]
    public void WF_21_GetCurrentActivities_Success()
    {
        try
        {
            var activities = _softExpertApi.GetCurrentActivities(WorkflowID);
            Assert.NotNull(activities);
            console.WriteLine($"Atividades em andamento: {activities.Count}");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// GetCurrentActivities com instância inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_21_GetCurrentActivities_Error()
    {
        try
        {
            _softExpertApi.GetCurrentActivities("INSTANCIA_INEXISTENTE_XYZ");
            Assert.Fail("Era esperado SoftExpertException");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Lista todas as atividades da instância
    /// </summary>
    [Fact]
    public void WF_22_GetActivitiesFromWorkflow_Success()
    {
        try
        {
            var activities = _softExpertApi.GetActivitiesFromWorkflow(WorkflowID);
            Assert.NotNull(activities);
            Assert.True(activities.Count > 0);
            Assert.False(string.IsNullOrWhiteSpace(activities[0].idstruct));
            console.WriteLine($"Atividades: {activities.Count}");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// GetActivitiesFromWorkflow com instância inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_22_GetActivitiesFromWorkflow_Error()
    {
        try
        {
            _softExpertApi.GetActivitiesFromWorkflow("INSTANCIA_INEXISTENTE_XYZ");
            Assert.Fail("Era esperado SoftExpertException");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Cancela uma instância de workflow
    /// </summary>
    [Fact]
    public void WF_23_cancelWorkflow_Success()
    {
        string workflowID = "VBG202002801";
        string explanation = "Teste unitário SoftExpertAPI - cancelWorkflow";

        try
        {
            _softExpertApi.cancelWorkflow(workflowID, explanation, iduser);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// cancelWorkflow com instância inexistente — espera Exception
    /// </summary>
    [Fact]
    public void WF_23_cancelWorkflow_Error()
    {
        try
        {
            _softExpertApi.cancelWorkflow("INSTANCIA_INEXISTENTE_XYZ", "teste", iduser);
            Assert.Fail("Era esperado Exception");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Desassocia atividade do usuário executor
    /// </summary>
    [Fact]
    public void WF_24_unlinkActivityFromUser_Success()
    {
        string workflowID = "PRO20240518";
        string activityID = "AnalisarDemanda";

        try
        {
            _softExpertApi.unlinkActivityFromUser(workflowID, activityID);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// unlinkActivityFromUser com instância inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void WF_24_unlinkActivityFromUser_Error()
    {
        try
        {
            _softExpertApi.unlinkActivityFromUser("INSTANCIA_INEXISTENTE_XYZ", "ATIVIDADE_INEXISTENTE");
            Assert.Fail("Era esperado SoftExpertException");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }
}
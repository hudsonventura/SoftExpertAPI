using Domain;
using Microsoft.Extensions.Configuration;
using SoftExpertAPI;
using Xunit.Abstractions;

namespace Testes_DataSet;

public class Testes_DataSet
{
    ITestOutputHelper console;
    SoftExpertDataSetApi api;

    string WorkflowID = "CCF202614358";

    const string SqlQueryGetWorkflowInstanceData = @"select p.idprocess
, p.IDOBJECT
, P.FGSTATUS
, p.cduserstart
, p.nmprocess
, p.cdprocessmodel
, p.idprocessmodel
, p.nmprocessmodel
, p.idrevision
, p.dtstart
, p.tmstart
, dhstart
, p.dtfinish
, p.tmfinish
, dhfinish
, gnf.OIDENTITYREG
from softexpert.WFPROCESS p
JOIN softexpert.GNASSOCFORMREG GNF on p.cdassocreg = GNF.cdassoc
where p.IDPROCESS = :workflowID";

    public Testes_DataSet(ITestOutputHelper output)
    {
        console = output;

        var _appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        Configurations configs = new Configurations()
        {
            baseUrl = _appsettings["url"],
            login = _appsettings["user"],
            pass = _appsettings["pass"],
            domain = _appsettings["domain"],
            token = _appsettings["token"],
        };

        if (!string.IsNullOrWhiteSpace(_appsettings["authorization"]))
        {
            configs.token = _appsettings["authorization"];
        }

        api = new SoftExpertDataSetApi(configs);
    }

    /// <summary>
    /// Consulta o conjunto queryGetWorkflowInstanceData com um workflow existente
    /// </summary>
    [Fact]
    public void DS_01_Query_Success()
    {
        try
        {
            var rows = api.Query<ManageInstanceObject>(
                "queryGetWorkflowInstanceData",
                new Dictionary<string, string> { { "workflowID", WorkflowID } },
                SqlQueryGetWorkflowInstanceData
            );

            Assert.NotNull(rows);
            Assert.NotEmpty(rows);
            Assert.Equal(WorkflowID, rows[0].idprocess);
            console.WriteLine($"Instância: {rows[0].idprocess} | Status: {rows[0].Status}");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Query com conjunto inexistente — a exception deve citar o ID e o SQL informado
    /// </summary>
    [Fact]
    public void DS_01_Query_Error()
    {
        const string idDataset = "queryDoesNotExistSoftExpertAPI";
        const string sql = "select 1 from dual where 1 = :dummy";

        try
        {
            api.Query<ManageInstanceObject>(
                idDataset,
                new Dictionary<string, string> { { "dummy", "1" } },
                sql
            );
            Assert.Fail("Era esperado erro HTTP para conjunto de dados inexistente");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.Contains(idDataset, error.Message);
            Assert.Contains(sql, error.Message);
        }
    }
}

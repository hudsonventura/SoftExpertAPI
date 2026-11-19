using Microsoft.Extensions.Configuration;
using SoftExpertAPI;
using Testes;
using Xunit.Abstractions;

namespace Testes_Generico;

public class Testes_Generico
{
    ITestOutputHelper console;
    SoftExpertGenericApi api;

    // Componentes do SoftExpert (códigos separados por vírgula)
    string Component = "109";

    // Equipe existente no ambiente para teste de permissões (ajustar conforme necessário)
    int CdTeam = 67062;
    string IdTeam = "TST20261117134505";
    string NmTeam = "teste";
    string CdTeamEditor = "8";

    // addUserToTeam / removeUserFromTeam — substituir por equipe e matrícula reais no ambiente
    string IdTeamForUser = "EQUIPE_FICTICIA_001";
    string IdUserForTeam = "usuario.ficticio";

    public Testes_Generico(ITestOutputHelper output)
    {
        console = output;

        var _appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        Configurations configs = ConfigurationsFromAppSettings.Create(_appsettings);

        api = new SoftExpertGenericApi(configs);
    }

    /// <summary>
    /// Cria uma nova equipe via SOAP newTeam
    /// </summary>
    [Fact]
    public void GN_01_newTeam_Success()
    {
        string idteam = $"TST{DateTime.Now:yyyyMMddHHmmss}";
        string nmteam = $"Equipe teste SoftExpertAPI {idteam}";

        try
        {
            string recordId = api.newTeam(idteam, nmteam, Component);

            Assert.False(string.IsNullOrWhiteSpace(recordId));
            Console.WriteLine($"Equipe criada: {idteam} | RecordID: {recordId}");
        }
        catch (Exception error)
        {
            Console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// newTeam com identificador vazio — espera SoftExpertException
    /// </summary>
    [Fact]
    public void GN_01_newTeam_Error()
    {
        try
        {
            api.newTeam("", "Equipe inválida", Component);
            Assert.Fail("Era esperado SoftExpertException para IDTEAM vazio");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// newTeam com identificador duplicado — espera SoftExpertException
    /// </summary>
    [Fact]
    public void GN_01_newTeam_Duplicate_Error()
    {
        string idteam = $"DUP{DateTime.Now:yyyyMMddHHmmss}";
        string nmteam = $"Equipe duplicada SoftExpertAPI {idteam}";

        try
        {
            string recordId = api.newTeam(idteam, nmteam, Component);
            Assert.False(string.IsNullOrWhiteSpace(recordId));
            console.WriteLine($"Primeira criação OK: {idteam} | RecordID: {recordId}");

            api.newTeam(idteam, nmteam, Component);
            Assert.Fail("Era esperado SoftExpertException para equipe duplicada");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Define permissões de segurança de uma equipe existente via REST
    /// </summary>
    [Fact]
    public void GN_02_setTeamPermissions_Success()
    {
        try
        {
            api.setTeamPermissions(
                cdteam: CdTeam,
                idteam: IdTeam,
                nmteam: NmTeam,
                public_read: true,
                idteam_editor: CdTeamEditor
            );

            Assert.True(true);
            Console.WriteLine($"Permissões definidas para equipe {IdTeam} (cdteam={CdTeam})");
        }
        catch (Exception error)
        {
            Console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// setTeamPermissions com equipe inexistente — espera SoftExpertException
    /// </summary>
    [Fact]
    public void GN_02_setTeamPermissions_Error()
    {
        try
        {
            api.setTeamPermissions(
                cdteam: 999999999,
                idteam: "EQUIPE_INEXISTENTE_XYZ",
                nmteam: "Equipe inválida",
                public_read: true,
                idteam_editor: CdTeamEditor
            );

            Assert.Fail("Era esperado SoftExpertException para equipe inexistente");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Adiciona usuário à equipe via SOAP addUserToTeam
    /// </summary>
    [Fact]
    public void GN_03_addUserToTeam_Success()
    {
        try
        {
            api.addUserToTeam(IdTeamForUser, IdUserForTeam);

            Assert.True(true);
            Console.WriteLine($"Usuário {IdUserForTeam} adicionado à equipe {IdTeamForUser}");
        }
        catch (Exception error)
        {
            Console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// addUserToTeam com equipe/usuário inválidos — espera SoftExpertException
    /// </summary>
    [Fact]
    public void GN_03_addUserToTeam_Error()
    {
        try
        {
            api.addUserToTeam("EQUIPE_INEXISTENTE_XYZ", "usuario_inexistente_xyz");
            Assert.Fail("Era esperado SoftExpertException para equipe ou usuário inválido");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Remove usuário da equipe via SOAP removeUserFromTeam
    /// </summary>
    [Fact]
    public void GN_04_removeUserFromTeam_Success()
    {
        try
        {
            api.removeUserFromTeam(IdTeamForUser, IdUserForTeam);

            Assert.True(true);
            console.WriteLine($"Usuário {IdUserForTeam} removido da equipe {IdTeamForUser}");
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// removeUserFromTeam com equipe/usuário inválidos — espera SoftExpertException
    /// </summary>
    [Fact]
    public void GN_04_removeUserFromTeam_Error()
    {
        try
        {
            api.removeUserFromTeam("EQUIPE_INEXISTENTE_XYZ", "usuario_inexistente_xyz");
            Assert.Fail("Era esperado SoftExpertException para equipe ou usuário inválido");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }
}

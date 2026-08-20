using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Examples;
using SoftExpertAPI;

namespace Testes_Administracao;

public class Testes_Administracao
{
    ITestOutputHelper console;
    SoftExpertAdminApi api;

    // Matrícula usada nos testes de sucesso (deve existir no ambiente)
    string UserID = "01234567891";

    public Testes_Administracao(ITestOutputHelper output)
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
        };

        if (!string.IsNullOrWhiteSpace(_appsettings["authorization"]))
        {
            configs.token = _appsettings["authorization"];
        }

        api = new SoftExpertAdminApi(configs);
    }

    /// <summary>
    /// Habilita um usuário existente via SOAP changeUserStatus (UserStatus = 1)
    /// </summary>
    [Fact]
    public void ADM_01_EnableUser_Success()
    {
        try
        {
            api.enableUser(UserID);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Desabilita um usuário existente via SOAP changeUserStatus
    /// </summary>
    [Fact]
    public void ADM_02_DisableUser_Success()
    {
        try
        {
            api.disableUser(UserID);
            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }

    /// <summary>
    /// Tenta habilitar um usuário inexistente e espera SoftExpertException
    /// </summary>
    [Fact]
    public void ADM_03_EnableUser_InvalidUser_Error()
    {
        try
        {
            api.enableUser("USUARIO_INEXISTENTE_XYZ");
            Assert.Fail("Era esperado SoftExpertException para usuário inexistente");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Tenta desabilitar um usuário inexistente e espera SoftExpertException
    /// </summary>
    [Fact]
    public void ADM_04_DisableUser_InvalidUser_Error()
    {
        try
        {
            api.disableUser("USUARIO_INEXISTENTE_XYZ");
            Assert.Fail("Era esperado SoftExpertException para usuário inexistente");
        }
        catch (SoftExpertException error)
        {
            console.WriteLine($"Erro esperado: {error.Message}");
            Assert.True(true);
        }
    }

    /// <summary>
    /// Ciclo completo: desabilita e depois habilita o usuário, deixando-o ativo ao final
    /// </summary>
    [Fact]
    public void ADM_05_ChangeUserStatus_Cycle()
    {
        try
        {
            api.disableUser(UserID);
            console.WriteLine($"Usuário {UserID} desabilitado");

            api.enableUser(UserID);
            console.WriteLine($"Usuário {UserID} habilitado");

            Assert.True(true);
        }
        catch (Exception error)
        {
            console.WriteLine($"Erro: {error.Message}");
            throw;
        }
    }
}

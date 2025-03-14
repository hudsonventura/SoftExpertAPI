using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Examples;
using SoftExpertAPI;

namespace Testes_Administracao;

public class Testes_Administracao
{
    ITestOutputHelper console;

    SoftExpertAPI.Admin.SoftExpertAdminApi api;


    public Testes_Administracao(ITestOutputHelper output){
        console = output;


        var _appsettings = new ConfigurationBuilder()
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

        api = new SoftExpertAPI.Admin.SoftExpertAdminApi(configs);
        
    }




    [Fact]
    public void ADM_01_DisableUser()
    {
        try
        {
            api.disableUser("01234567891");
            Assert.True(1==1);
        }
        catch (SoftExpertAPI.SoftExpertException)
        {
            Assert.Fail("Erro na execução");
            throw;
        }
    }

    [Fact]
    public void ADM_01_EnableUser()
    {
        try
        {
            api.enableUser("01234567891");
            Assert.True(1==1);
        }
        catch (SoftExpertAPI.SoftExpertException)
        {
            Assert.Fail("Erro na execução");
            throw;
        }
    }


}
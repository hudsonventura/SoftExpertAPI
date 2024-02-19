using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Examples;

namespace Testes_Administracao;

public class Testes_Administracao
{
    ITestOutputHelper console;

    SoftExpert.Admin.SoftExpertAdminApi api;


    public Testes_Administracao(ITestOutputHelper output){
        console = output;


        var appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string urlBase = appsettings["url"].ToString();
        string authorization = appsettings["authorization"].ToString();

        ExampleOracleImplementation db = new ExampleOracleImplementation(appsettings);


        api = new SoftExpert.Admin.SoftExpertAdminApi(urlBase, authorization, db);
        
    }




    [Fact]
    public void ADM_01_EnableDisableUser()
    {
        try
        {
            api.disableUser("01234567891");
            Assert.True(1==1);
        }
        catch (SoftExpert.SoftExpertException)
        {
            Assert.Fail("Erro na execução");
            throw;
        }
    }


}
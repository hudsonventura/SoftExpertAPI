using Microsoft.Extensions.Configuration;
using SoftExpert.Admin;

namespace Examples;

public class AdminExamples
{

    IConfiguration _appsettings;
    public AdminExamples(IConfiguration appsettings)
    {
        _appsettings = appsettings;
    }

    public void Execute(){

        
        #region Preparação
        string urlBase = _appsettings["url"].ToString();
        string authorization = _appsettings["authorization"].ToString();

        ExampleOracleImplementation oracle = new ExampleOracleImplementation(_appsettings);
        SoftExpertAdminApi api = new SoftExpertAdminApi(urlBase, authorization, oracle);

        #endregion


        #region Habilitar / Desabilitar usuário
        try
        {
            api.enableUser("01234567891");
            api.disableUser("01234567891");
        }
        catch (System.Exception error)
        {
            throw;
        }
        #endregion
    }
    
}

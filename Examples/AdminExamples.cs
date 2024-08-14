using Microsoft.Extensions.Configuration;
using SoftExpertAPI;
using SoftExpertAPI.Admin;

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
        string url = _appsettings["url"].ToString();
        string authorization = _appsettings["authorization"].ToString();

        IDataBase oracle = new ExampleOracleImplementation(_appsettings);

        Configurations configs = new Configurations(){
            baseUrl = url,
            db = oracle,
            headers = new Dictionary<string, string>(){{"Authorization", ""}}
        };
        SoftExpertAdminApi api = new SoftExpertAdminApi(configs);

        

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

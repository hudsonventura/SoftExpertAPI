using Microsoft.Extensions.Configuration;
using SoftExpertAPI;

namespace Examples;

public class AdminExamples
{

    IConfiguration _appsettings;
    public AdminExamples(IConfiguration appsettings)
    {
        _appsettings = appsettings;
    }

    public void Execute()
    {
        #region Preparação dos parametros


        //Implementação OPCIONAL de uma classe para acessar banco de dados. É necessário respeitar a interface SoftExpertAPI.Interfaces.IDataBase
        //Necessário para algumas implementações fora do escopo da API padrão do SoftExpert.
        ExampleOracleImplementation _db = new ExampleOracleImplementation(_appsettings);


        //Necessário em casos em que os arquivos do SE não ficam no banco de dados
        IFileDownload _downloader = new ExampleFileDownloadImplementation(_appsettings);


        Configurations configs = new Configurations(){
            baseUrl = _appsettings["url"].ToString(),
            db = _db,
            login = _appsettings["user"].ToString(),
            pass = _appsettings["pass"].ToString(),
            //domain = _appsettings["domain"].ToString(),
            //token = _appsettings["authorization"].ToString()
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

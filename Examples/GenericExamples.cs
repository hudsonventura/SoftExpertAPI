using Microsoft.Extensions.Configuration;
using SoftExpertAPI;

namespace Examples;

public class GenericExamples
{
    IConfiguration _appsettings;
    public GenericExamples(IConfiguration appsettings)
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
        SoftExpertGenericApi api = new SoftExpertGenericApi(configs);

        

        #endregion




        #region Adicinar e remover usuario de equipe
        try
        {
            api.addUserToTeam("GESTAOPROCESSOS-excecoes", "123456789");
            api.removeUserFromTeam("GESTAOPROCESSOS-excecoes*", "123456789");
        }
        catch(SoftExpertException error){
            throw;
        }
        catch (System.Exception error)
        {
            throw;
        }
        #endregion
    }
}

using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Examples;

public class ExampleOracleImplementation : SoftExpert.IDataBase
{
    private readonly string dbConnection;
    

    public ExampleOracleImplementation(IConfiguration appsettings)
    {
        dbConnection = $"Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL=TCP)(HOST={appsettings["Oracle:host"]})(PORT={appsettings["Oracle:port"]})))(CONNECT_DATA = (SERVICE_NAME={appsettings["Oracle:dbname"]})));User Id={appsettings["Oracle:user"]};Password={appsettings["Oracle:pass"]};";
        db_name = appsettings["Oracle:dbowner"];
    }

    public string db_name { get; set; }

    public int Execute(string sql, Dictionary<string, dynamic> parametros = null)
    {
        using (OracleConnection con = new OracleConnection(dbConnection))
        {
            try
            {
                con.Open();
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = sql; 

                        if (parametros != null && parametros.Count() > 0)
                        {
                            foreach (var item in parametros)
                            {
                                cmd.Parameters.Add(new OracleParameter(item.Key, item.Value));
                            }
                        }

                        return cmd.ExecuteNonQuery();
                    }
                
                
            }
            catch (Exception erro)
            {
                string msg = erro.Message;
                if(erro.InnerException != null){
                    msg += erro.InnerException.Message;
                }
                throw new Exception($"{msg}{Environment.NewLine}SQL: {sql}");
            }
            finally
            {
                con.Close();
            }
        }
    }

    public DataTable Query(string sql, Dictionary<string, dynamic> parametros = null)
    {
        using (OracleConnection con = new OracleConnection(dbConnection))
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    List<Dictionary<string, dynamic>> retorno = new List<Dictionary<string, dynamic>>();

                    con.Open();
                    cmd.CommandText = sql;
                    if (parametros is not null && parametros.Count() > 0)
                    {
                        foreach (var item in parametros)
                        {
                            cmd.Parameters.Add(new OracleParameter(item.Key, item.Value));
                        }

                    }

                    var reader = cmd.ExecuteReader();

                    DataTable datatable = new DataTable();
                    datatable.Load(reader);
                    return datatable;
                }
                catch (Exception erro)
                {
                    throw erro;
                }
                finally
                {
                    con.Close();
                }
            };
        };
    }

    
}

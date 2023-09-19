using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples;

internal class ExampleOracleImplement : SoftExpertAPI.Interfaces.IDataBase
{
    private readonly IConfiguration appsettings;

    public ExampleOracleImplement(IConfiguration appsettings)
    {
        this.appsettings = appsettings;
    }

    public void Execute(string sql, Dictionary<string, dynamic> parametros = null)
    {
        throw new NotImplementedException();
    }

    public DataTable Query(string sql, Dictionary<string, dynamic> parametros = null)
    {
        string dbConnection = $"Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL=TCP)(HOST={appsettings["Oracle:host"]})(PORT={appsettings["Oracle:port"]})))(CONNECT_DATA = (SERVICE_NAME={appsettings["Oracle:dbname"]})));User Id={appsettings["Oracle:user"]};Password={appsettings["Oracle:pass"]};";
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

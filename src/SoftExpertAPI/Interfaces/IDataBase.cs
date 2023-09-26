using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftExpertAPI.Interfaces;

public interface IDataBase
{
    DataTable Query(string sql, Dictionary<string, dynamic> parametros = null);
    int Execute(string sql, Dictionary<string, dynamic> parametros = null);
}

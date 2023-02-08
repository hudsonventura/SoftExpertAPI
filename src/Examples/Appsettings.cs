using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

internal class Appsettings
{
    public static JToken GetSettings(string file = "appsettings.json") {
        string appsettings = $"{System.AppDomain.CurrentDomain.BaseDirectory.ToString()}/{file}";
        bool exists = File.Exists(appsettings);
        if (!exists)
        {
            throw new Exception($"Unknow file {file} at the same directory to this app");
        }

        string json = "";
        try
        {
            json = System.IO.File.ReadAllText(appsettings);
        }
        catch (Exception error)
        {
            throw new Exception($"The file {file} is incorrect. Error: {error.Message}");
        }

        try
        {
            return JArray.Parse(json);
        }
        catch (Exception)
        {
            try
            {
                var test = JArray.Parse($"[{json}]");
                if (test.Count() == 0)
                {
                    throw new Exception($"The JSON is null or empty");
                }
                return test[0];
            }
            catch (Exception error)
            {
                throw new Exception($"The file {file} is incorrect. Error: {error.Message}");
            }
        }
        

    }
}

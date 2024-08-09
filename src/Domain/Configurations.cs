using System.Collections.Generic;

namespace SoftExpert;

public class Configurations
{
    public IDataBase db { get; set; }

    public string db_name = null;

    //usado para gestão de instancias não implementadas na API original do SE, como reativar e retornar instâncias de workflow
    public string baseUrl { get; set; } = string.Empty;
    public string login { get; set; } = string.Empty;
    public string pass { get; set; } = string.Empty;
    public string domain { get; set; } = string.Empty;

    public string authorization { get; set; } = string.Empty;

    public Dictionary<string, string> headers { get; set; } = new Dictionary<string, string>();


    public StorageFiles storage { get; private set; } = StorageFiles.Database;



    public enum StorageFiles
    {
        Database = 1,
        Directory = 2
    }
}

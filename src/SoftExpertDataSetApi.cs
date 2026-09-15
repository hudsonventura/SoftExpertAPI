using System.Collections.Generic;


namespace SoftExpertAPI;

public class SoftExpertDataSetApi : SoftExpertBaseAPI
{
    public SoftExpertDataSetApi(Configurations configs) : base(configs)
    {
    }

    protected override void SetUriModule()
    {
        _uriModule = "/apigateway/v1/dataset-integration/";
    }

    /// <summary>
    /// Consulta um conjunto de dados do SoftExpert via REST.
    /// O parâmetro <paramref name="query"/> não é enviado na requisição: serve apenas para orientar, em caso de erro HTTP, que o conjunto de dados deve ser criado com aquele SQL.
    /// </summary>
    /// <typeparam name="T">Tipo de cada linha retornada pelo conjunto de dados</typeparam>
    /// <param name="id_dataset">ID do conjunto de dados no SoftExpert</param>
    /// <param name="parameters">Parâmetros (binds) enviados no body JSON. Pode ser nulo</param>
    /// <param name="query">SQL opcional do conjunto de dados. Usado somente na mensagem de erro HTTP</param>
    /// <returns>Lista de registros desserializados para <typeparamref name="T"/></returns>
    public List<T> Query<T>(string id_dataset, Dictionary<string, string> parameters = null, string query = null)
    {
        return SendRequestRest_DataSet<T>(id_dataset, parameters, query);
    }
}

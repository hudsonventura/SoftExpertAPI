using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SoftExpertAPI;
using SoftExpertAPI.Interfaces;


namespace SoftExpert.Admin;

public class SoftExpertAdminApi : SoftExpertBaseAPI
{
    public SoftExpertAdminApi(string baseUrl, Dictionary<string, string> headers, IDataBase db = null) : base(baseUrl, headers, db)
    {
    }

    public SoftExpertAdminApi(string baseUrl, string authorization, IDataBase db = null) : base(baseUrl, authorization, db)
    { 
    }

    protected override void SetUriModule()
    {
        _uriModule = "/apigateway/se/ws/adm_ws.php";
    }


    /// <summary>
    /// Esta função da API do Softexpert não funciona. Utilizar no lugar a db_enableUser e db_disableUser
    /// </summary>
    /// <param name="ProcessID">ID do processo</param>
    /// <param name="WorkflowTitle">Titulo da instância</param>
    /// <param name="UserID">Matrícula do usuário iniciador da instância</param>
    /// <returns>newWorkflowResponse, objeto com os campos Status, Code, Detail, RecordKey e RecordID. Se Code = 1 entao RecordID conterá o ID da intância gerada. Se Code != 1, uma SoftExpertException é gerada</returns>
    /// <exception cref="SoftExpertException"></exception>
    public async Task disableUser(string UserID)
    {
        throw new NotImplementedException();
        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:admin'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:disableUser>
                                    <urn:cd>
                                        <urn:codes>{UserID}</urn:codes>
                                    </urn:cd>
                                </urn:disableUser>
                            </soapenv:Body>
                            </soapenv:Envelope>"
        ;

        try
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _uriModule);
            request.Headers.Add("SOAPAction", "urn:admin#disableUser");
            request.Content = new StringContent(body.Trim(), Encoding.UTF8, "application/xml");

            HttpResponseMessage  response = restClient.SendAsync(request).Result;

            var body_response = await response.Content.ReadAsStringAsync();

            return;

        }
        catch (SoftExpertException error)
        {
            error.setXMLSoapSent(body);
            throw error;
        }
        catch (Exception error2)
        {
            var erro = new SoftExpertException(error2.Message);
            erro.setXMLSoapSent(body);
            throw erro;
        }

    }

    public void db_enableUser(string UserID){
        ValidateDB();
        
        string sql = $"update {db_name}.aduser set fguserenabled = 1 where iduser = :UserID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":UserID", UserID);

        try
        {
            db.Execute(sql, parametros);
            return;
        }
        catch (Exception erro)
        {
            throw new Exception($"Falha executar SQL no banco de dados. Erro: {erro.Message}");
        }
        
    }

    public void db_disableUser(string UserID){
        ValidateDB();
        
        string sql = $"update {db_name}.aduser set fguserenabled = 0 where iduser = :UserID";

        Dictionary<string, dynamic> parametros = new Dictionary<string, dynamic>();
        parametros.Add(":UserID", UserID);

        try
        {
            db.Execute(sql, parametros);
            return;
        }
        catch (Exception erro)
        {
            throw new Exception($"Falha executar SQL no banco de dados. Erro: {erro.Message}");
        }
    }

    
}



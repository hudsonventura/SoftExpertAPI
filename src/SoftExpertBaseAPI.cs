using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoftExpert;

namespace SoftExpert;

public abstract class SoftExpertBaseAPI
{
    protected string _uriModule = null;
    protected readonly HttpClient restClient;

    protected IDataBase db = null;

    public string db_name = null;

    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e os headers. Header Authorization é necessário
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="headers">Passar os headers a serem enviados na requisição. Não esqueça do header Authorization</param>
    /// <param name="db">Classe concreta que implemente a inteface SoftExpertAPI.Interfaces.IDataBase</param>
    public SoftExpertBaseAPI(string baseUrl, Dictionary<string, string> headers, SoftExpert.IDataBase db = null)
    {
        restClient = new HttpClient();
        restClient.BaseAddress = new Uri(baseUrl);
        foreach (var head in headers)
        {
            restClient.DefaultRequestHeaders.Add(head.Key, head.Value);
        }
        //restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);
        this.db = db;
        this.db_name = db.db_name;
        SetUriModule();
    }

    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e a string to Authorization incluindo o 'Basic ....'
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="authorization">Basic no formato base64("dominio\usuario:senha") Ex.: Basic dmMgPyB1bSBjdXJpb3Nv</param>
    public SoftExpertBaseAPI(string baseUrl, string authorization, SoftExpert.IDataBase db = null)
    {
        restClient = new HttpClient();
        restClient.BaseAddress = new Uri(baseUrl);
        restClient.DefaultRequestHeaders.Add("Authorization", authorization);
        //restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);

        if(db != null ){
            this.db = db;
            this.db_name = db.db_name;
        }
        
        SetUriModule();
    }

    protected virtual void SetUriModule()
    {
        throw new NotImplementedException("Atribuir a propriedade _uriModule");
    }

    


    protected void ValidateDB(){
        if (db is null) {
            throw new SoftExpertException("Uma instancia de banco de dados não foi informada na criação deste objeto. Crie forneça uma conexão com seu banco de dados implementando a interface SoftExpertAPI.Interfaces.IDataBase");
        }

        if (db_name is null) {
            throw new SoftExpertException("Uma instancia de banco de dados foi informada mas o nome do seu banco precisa ser informado na propriedade db_name");
        }
    }

    protected JToken Parse(string xml)
    {
        
        // em caso de ter retornado um XML
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        string json = JsonConvert.SerializeXmlNode(doc);
        
        return JsonConvert.DeserializeObject<dynamic>(json);
    }


    protected JToken SendRequestSOAP(string function, string xmlbody, string urimodule = null){
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, (urimodule is null) ? _uriModule : urimodule);
            request.Headers.Add("SOAPAction", $"urn:workflow#{function}");
            request.Content = new StringContent(xmlbody.Trim(), Encoding.UTF8, "text/xml");

            HttpResponseMessage  response = restClient.SendAsync(request).Result;
            if(!response.IsSuccessStatusCode){
                if(response.ReasonPhrase != ""){
                    throw new Exception(response.ReasonPhrase);
                }
                throw new Exception($"Falha ao conectar ao servidor {restClient.BaseAddress.AbsoluteUri}");
            }
            var body_response = response.Content.ReadAsStringAsync().Result;

            var json = Parse(body_response);
            
            var se_response = json.SelectToken("SOAP-ENV:Envelope")
                                .SelectToken("SOAP-ENV:Body")
                                .SelectToken($"{function}Response");
            var status = se_response.SelectToken("Status").ToString();
            var code = se_response.SelectToken("Code").ToString();
            if(status == "FAILURE"){
                var error = new SoftExpertException(se_response.SelectToken("Detail").ToString(), int.Parse(code));
                error.setXMLSoapReceived(body_response);
                throw error;
            }
            return se_response;
        }
        catch (SoftExpertException error)
        {
            error.setXMLSoapSent(xmlbody);
            throw error;
        }
        catch (Exception)
        {
            throw;
        }
        
    }


    protected void SendRequestRest(HttpRequestMessage request)
    {
        HttpResponseMessage  response = restClient.SendAsync(request).Result;

        string json_response = null;
        
        try
        {
            var bodyResponse = response.Content.ReadAsStringAsync().Result;
            using (JsonDocument document = JsonDocument.Parse(bodyResponse))
            {
                JsonElement root = document.RootElement;
                if (root.TryGetProperty("response", out JsonElement responseElement))
                {
                    json_response = responseElement.GetString();
                }
            }
        }
        catch (System.Exception erro)
        {
            
        }

        if(response.IsSuccessStatusCode ){
            return; //Success!
        }
        
        if(!response.IsSuccessStatusCode && json_response is not null){
            throw new Exception($"SoftExpert error: {json_response}");
        }


        if(!response.IsSuccessStatusCode){
            throw new Exception($"Falha ao conectar ao servidor {restClient.BaseAddress.AbsoluteUri}. {response.StatusCode} -> {response.Content.ReadAsStringAsync().Result}");
        }



        return;
    }
}


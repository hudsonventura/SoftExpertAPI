using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SoftExpertAPI;

public abstract class SoftExpertBaseAPI
{
    protected string _uriModule = null;
    protected readonly HttpClient restClient;

    protected IDataBase db = null;
    protected IFileDownload downloader = null;


    public string db_name = null;

    //usado para gestão de instancias não implementadas na API original do SE, como reativar e retornar instâncias de workflow
    public string login { get; private set; } = string.Empty;
    public string pass { get; private set; } = string.Empty;
    public string domain { get; private set; } = string.Empty;



    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e os headers. Header Authorization é necessário
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="headers">Passar os headers a serem enviados na requisição. Não esqueça do header Authorization</param>
    /// <param name="db">Classe concreta que implemente a inteface SoftExpertAPI.Interfaces.IDataBase</param>
    public SoftExpertBaseAPI(Configurations configs)
    {
        restClient = new HttpClient();
        var uri = new Uri(configs.baseUrl);
        restClient.BaseAddress = uri;
        restClient.DefaultRequestHeaders.Add("Host", uri.Host);

        //headers e authorization
        if(configs.headers.Count() > 0){
            foreach (var head in configs.headers)
            {
                restClient.DefaultRequestHeaders.Add(head.Key, head.Value);
            }
        }
        if(configs.token != string.Empty){
            restClient.DefaultRequestHeaders.Add("Authorization", configs.token);
        }else{
            try
            {
                string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{configs.domain}\\{configs.login}:{configs.pass}"));
                restClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64}");
            }
            catch (System.Exception)
            {
                throw new Exception("Necessário passar via configs login, senha e domain, ou passar o token do usuário");
            }
            
        }

        if(configs.downloader != null){
            this.downloader = configs.downloader;
        }
        
        if(configs.db != null){
            this.db = configs.db;
        }
        
        this.db_name = configs.db.db_name;
        this.login = configs.login;
        this.pass = configs.pass;
        this.domain = configs.domain;
        SetUriModule();

    }

    /// <summary>
    /// Construtor. Necessário passar a URL completa do ambiente do SE e a string to Authorization incluindo o 'Basic ....'
    /// </summary>
    /// <param name="url">URL completa do ambiente. Ex.: https://se.example.com.br</param>
    /// <param name="authorization">Basic no formato base64("dominio\usuario:senha") Ex.: Basic dmMgPyB1bSBjdXJpb3Nv</param>
    public SoftExpertBaseAPI(string baseUrl, string authorization, SoftExpertAPI.IDataBase db = null,string login = null, string pass = null, string domain = null)
    {
        restClient = new HttpClient();
        restClient.BaseAddress = new Uri(baseUrl);
        restClient.DefaultRequestHeaders.Add("Authorization", authorization);
        //restClient.AddDefaultHeader("Host", url.Split("://")[1].Split(":")[0]);

        this.login = login;
        this.pass = pass;
        this.domain = domain;

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
                if(response.StatusCode != 0){
                    throw new SoftExpertException($"{restClient.BaseAddress.AbsoluteUri} respondeu {response.StatusCode}");
                }
                throw new Exception($"Falha ao conectar ao servidor {restClient.BaseAddress.AbsoluteUri}");
            }
            var body_response = response.Content.ReadAsStringAsync().Result;
            if(body_response == null || body_response == ""){
                var error = new SoftExpertException("Resposta vazia do servidor");
                throw error;
            }

            var json = Parse(body_response);
            
            var se_response = json.SelectToken("SOAP-ENV:Envelope")
                                .SelectToken("SOAP-ENV:Body")
                                .SelectToken($"{function}Response");
            try
            {
            var status = se_response.SelectToken("Status").ToString();
            var code = se_response.SelectToken("Code").ToString();
            if(status == "FAILURE"){
                var error = new SoftExpertException(se_response.SelectToken("Detail").ToString(), int.Parse(code));
                error.setResponseReceived(body_response);
                throw error;
            }
            return se_response;
            }
            catch (System.Exception)
            {
                var returno = se_response.SelectToken("return").ToString();
                if(returno != "1"){
                    var error = new SoftExpertException(returno);
                    error.setResponseReceived(body_response);
                    throw error;
                }
                return se_response;
            }
            
        }
        catch (SoftExpertException error)
        {
            error.setRequestSent(xmlbody);
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



    protected string se_authentication_token { get; private set; } = null;
    public string jwt_token { get; private set; } = null;
    public DateTime token_expires { get; private set; } = DateTime.MinValue;



    protected string GetToken(){

        //primeiro valida o token sobre o tempo de expiração
        if(se_authentication_token != null && DateTime.Now < token_expires.AddMinutes(-5)){
            
            //Realiza uma validação do token no backend do SE. Se não for válido, entao gera um novo token
            HttpRequestMessage validation = new HttpRequestMessage(HttpMethod.Get, "/katana/chartengine/configuration");
            validation.Headers.Add("Authorization", jwt_token);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(restClient.BaseAddress.ToString());
                HttpResponseMessage  response = client.SendAsync(validation).Result;
                if(response.IsSuccessStatusCode ){
                    return se_authentication_token; //o token atual ainda é valido em tempo de expiração e no backend do SE
                }
            }
            /* Essa parte foi construída para casos do SE cair e o token ainda estar válido por tempo de expiração nesta API, mas não no backend do SE
             */
            
        }

        Console.WriteLine($"Renewal token process has started. Now: {DateTime.Now}, Token expires: {token_expires}");

        if(this.login == null || this.pass == null || this.domain == null){
            throw new Exception("Adicione o login, pass e domain na injeção de dependendias do objeto SoftExpertWorkflowApi");
        }


        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/softexpert/selogin");
        var headers = new Dictionary<string, string>
        {
            { "Accept", "*/*" },
            { "Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7" },
            { "Connection", "keep-alive" },
            { "Content-Type", "application/x-www-form-urlencoded; charset=UTF-8" },
            { "Origin", restClient.BaseAddress.ToString() },
            { "Referer", $"{restClient.BaseAddress.ToString()}/softexpert/external-login" }
        };
        foreach (var header in headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        string jsonBody = JsonConvert.SerializeObject(new 
            {
                AuthenticationParameter = new
                {
                    language = 2,
                    hashGUID = (string)null,
                    domain = "Amaggi Corporativo",
                    accessType = "DESKTOP",
                    login = "robo.serverino@amaggi.com.br",
                    password = "SazV&BzkLZZ@fsr3$PN2PC",
                    externalUser = true,
                    logout = true
                }
            }
        );
        string urlEncodedBody = $"json={HttpUtility.UrlEncode(jsonBody)}";
        request.Content = new StringContent(urlEncodedBody, Encoding.UTF8, "application/x-www-form-urlencoded");


        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri(restClient.BaseAddress.ToString());
            HttpResponseMessage  response = client.SendAsync(request).Result;
            if(!response.IsSuccessStatusCode ){
                Console.WriteLine();
            }

            // Verifique o status da resposta
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception($"Houve um erro ao tentar realizar login com o usuário externo");
            }

            // Obtenha os cookies da resposta
            IEnumerable<string> cookies;
            if (response.Headers.TryGetValues("Set-Cookie", out cookies))
            {
                var plLine = cookies.FirstOrDefault(part => part.Trim().StartsWith("pl="));
                var ktLine = cookies.FirstOrDefault(part => part.Trim().StartsWith("kt="));

                var authToken = plLine.Split(';')
                                        .FirstOrDefault()
                                        .Split('=').Last();
                var jwt = ktLine.Split(';')
                                        .FirstOrDefault()
                                        .Split('=').Last();

                //var authExpireStr = plLine.Split(';')[2].Split(",")[1].Trim();
                //DateTime teste = DateTime.ParseExact(authExpireStr, "dd-MMM-yyyy HH:mm:ss 'GMT'", System.Globalization.CultureInfo.InvariantCulture);
                DateTime authExpire = DateTime.Now.AddMinutes(60); 
                Console.WriteLine($"Renewal token process has finished. Now: {DateTime.Now}, Token expires: {authExpire}");

                if (authToken != null)
                {
                    se_authentication_token = authToken;
                    token_expires = authExpire;
                    jwt_token = jwt;
                    return se_authentication_token;
                }
                else
                {
                    throw new Exception("se-authentication-token não encontrado nos cookies");
                }
            }
            else
            {
                throw new Exception("Nenhum cookie encontrado na resposta");
            }
        }

    }

}


using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace SoftExpertAPI;
public class SoftExpertGenericApi : SoftExpertBaseAPI
{
    public SoftExpertGenericApi(Configurations configs) : base(configs)
    {
    }


    protected override void SetUriModule()
    {
        _uriModule = "/apigateway/se/ws/gn_ws.php";
    }


    /// <summary>
    /// Adiciona um usuário a uma equipe
    /// </summary>
    /// <param name="idteam"></param>
    /// <param name="iduser"></param> <summary>
    /// 
    /// </summary>
    /// <param name="idteam"></param>
    /// <param name="iduser"></param> <summary>
    /// 
    /// </summary>
    /// <param name="idteam"></param>
    /// <param name="iduser"></param>
    public void addUserToTeam(string idteam, string iduser){
        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:addUserToTeam>
                                <urn:IDTEAM>{idteam}</urn:IDTEAM>
                                <urn:USERS>{iduser}</urn:USERS>
                            </urn:addUserToTeam>
                            </soapenv:Body>
                        </soapenv:Envelope>"
        ;

        SendRequestSOAP("addUserToTeam", body);
        return;
    }


    /// <summary>
    /// Insere uma nova equipe no SoftExpert
    /// </summary>
    /// <param name="idteam">Identificador da equipe</param>
    /// <param name="nmteam">Nome da equipe</param>
    /// <param name="component">Códigos dos componentes separados por vírgula (ex.: "109,107,73")</param>
    /// <returns>RecordID retornado pelo SoftExpert</returns>
    /// <exception cref="SoftExpertException"></exception>
    public string newTeam(string idteam, string nmteam, string component)
    {
        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:generic'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:newTeam>
                                    <urn:IDTEAM>{idteam}</urn:IDTEAM>
                                    <urn:NMTEAM>{nmteam}</urn:NMTEAM>
                                    <urn:COMPONENT>{component}</urn:COMPONENT>
                                </urn:newTeam>
                            </soapenv:Body>
                        </soapenv:Envelope>";

        var se_response = SendRequestSOAP("newTeam", body, soapUrn: "generic");
        return se_response.SelectToken("RecordID").ToString();
    }


    /// <summary>
    /// Remove um usuário de uma equipe
    /// </summary>
    /// <param name="idteam"></param>
    /// <param name="iduser"></param> <summary>
    /// 
    /// </summary>
    /// <param name="idteam"></param>
    /// <param name="iduser"></param> <summary>
    /// 
    /// </summary>
    /// <param name="idteam"></param>
    /// <param name="iduser"></param>
    public void removeUserFromTeam(string idteam, string iduser){
        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:workflow'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:removeUserFromTeam>
                                <urn:IDTEAM>{idteam}</urn:IDTEAM>
                                <urn:USERS>{iduser}</urn:USERS>
                            </urn:removeUserFromTeam>
                            </soapenv:Body>
                        </soapenv:Envelope>"
        ;

        SendRequestSOAP("removeUserFromTeam", body);
        return;
    }


    /// <summary>
    /// Define as permissões de segurança de uma equipe. Necessário permissão de acesso na gestão e equipe e licença de gestão disponível.
    /// </summary>
    /// <param name="cdteam">Código numérico da equipe no SoftExpert</param>
    /// <param name="idteam">Identificador da equipe</param>
    /// <param name="nmteam">Nome da equipe</param>
    /// <param name="public_read">Leitura pública (fgPublicRead)</param>
    /// <param name="idteam_editor">Código da equipe editora (cdTeam) nas permissões</param>
    /// <param name="cdprod">Código do produto (padrão: 153)</param>
    /// <param name="modules">Códigos dos módulos (padrão: 39 — Workflow)</param>
    /// <exception cref="SoftExpertException"></exception>
    public void setTeamPermissions(int cdteam, string idteam, string nmteam, bool public_read, string idteam_editor, int cdprod = 153, List<int> modules = null)
    {
        if (modules == null || modules.Count == 0)
        {
            modules = new List<int>() { 39 };
        }

        var permission = new
        {
            Permission = new
            {
                fgPublicEdit = false,
                fgPublicRead = public_read,
                fgPublicReadExternalUser = false,
                teamPermissions = new
                {
                    HashSet = new[]
                    {
                        new
                        {
                            TeamPermission = new
                            {
                                read = true,
                                edit = true,
                                team = new
                                {
                                    ADTeam = new
                                    {
                                        cdTeam = idteam_editor
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        string token = GetToken();

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "/se/exp/generic/team/team.php");
        request.Headers.Add("Cookie", $"se-authentication-token={token}");

        var payload = new Dictionary<string, string>
        {
            { "cdteam", cdteam.ToString() },
            { "idteam", idteam },
            { "nmteam", nmteam },
            { "cdprod", cdprod.ToString() },
            { "modulecodes", string.Join(",", modules) },
            { "permission", JsonConvert.SerializeObject(permission) },
            { "fgcptpair", "0" },
        };
        request.Content = new FormUrlEncodedContent(payload);

        HttpResponseMessage response = _restClient.SendAsync(request).Result;
        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new SoftExpertException($"Houve um problema ao definir permissões da equipe '{idteam}'. Não autorizado. Verifique o token do usuário e se ele realmente possui permissão para definir permissões de equipe.");
                case HttpStatusCode.NotFound:
                    throw new SoftExpertException($"Houve um problema ao definir permissões da equipe '{idteam}'. Equipe não encontrada.");
                case HttpStatusCode.Forbidden:
                    throw new SoftExpertException($"Houve um problema ao definir permissões da equipe '{idteam}'. Não autorizado. Verifique o token do usuário e se ele realmente possui permissão para definir permissões de equipe.");
                default:
                    throw new SoftExpertException($"Houve um problema ao definir permissões da equipe '{idteam}'. {response.StatusCode}");
            }
        }

        string responseBody = response.Content.ReadAsStringAsync().Result;
        if (responseBody.Contains("softexpert/login"))
        {
            throw new SoftExpertException($"Houve um problema ao definir permissões da equipe '{idteam}'");
        }

        if (responseBody.Contains("Ocorreu um erro ao tentar processar informações"))
        {
            throw new SoftExpertException($"Houve um problema ao definir permissões da equipe '{idteam}'");
        }
    }
}
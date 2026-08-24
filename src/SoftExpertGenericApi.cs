

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
}

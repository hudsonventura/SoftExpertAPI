

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

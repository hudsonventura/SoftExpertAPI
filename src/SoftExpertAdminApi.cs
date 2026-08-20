using System;


namespace SoftExpertAPI;

public class SoftExpertAdminApi : SoftExpertBaseAPI
{
    public SoftExpertAdminApi(Configurations configs) : base(configs)
    {
    }

    protected override void SetUriModule()
    {
        _uriModule = "/apigateway/se/ws/adm_ws.php";
    }




    /// <summary>
    /// Habilita um usuário no SoftExpert
    /// </summary>
    /// <param name="UserID">Matrícula do usuário</param>
    /// <exception cref="Exception"></exception>
    public void enableUser(string UserID)
    {
        changeUserStatus(UserID, 1);
    }

    /// <summary>
    /// Desabilita um usuário no SoftExpert
    /// </summary>
    /// <param name="UserID">Matrícula do usuário</param>
    /// <exception cref="Exception"></exception>
    public void disableUser(string UserID)
    {
        changeUserStatus(UserID, 2);
    }

    /// <summary>
    /// Altera o status de um usuário no SoftExpert (1 = habilitado, 0 = desabilitado)
    /// </summary>
    /// <param name="UserID">Matrícula do usuário</param>
    /// <param name="UserStatus">1 para habilitar, 0 para desabilitar</param>
    private void changeUserStatus(string UserID, int UserStatus)
    {
        string body = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:admin'>
                            <soapenv:Header/>
                            <soapenv:Body>
                                <urn:changeUserStatus>
                                    <urn:IdUser>{UserID}</urn:IdUser>
                                    <urn:UserStatus>{UserStatus}</urn:UserStatus>
                                </urn:changeUserStatus>
                            </soapenv:Body>
                        </soapenv:Envelope>";

        SendRequestSOAP("changeUserStatus", body, soapUrn: "admin");
    }

    
}

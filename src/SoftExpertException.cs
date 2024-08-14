using System;

namespace SoftExpertAPI;

public class SoftExpertException : Exception
{
    public new string Message { get; private set; }
    public new int Code { get; private set; }
    public SoftExpertResponse Response { get; private set; }
    public new string XMLSoapSent { get; protected set; }

    public new string XMLSoapReceived { get; protected set; }
    public SoftExpertException(string message, int code = 0, SoftExpertResponse RetornoSE = null) : base(message)
    {
        Message = message;
        Response = RetornoSE;
        Code = code;
    }

    internal void setXMLSoapSent(string xml)
    {
        XMLSoapSent = xml;
    }

    internal void setXMLSoapReceived(string xml)
    {
        XMLSoapReceived = xml;
    }
}

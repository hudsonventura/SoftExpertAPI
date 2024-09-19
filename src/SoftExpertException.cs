using System;

namespace SoftExpertAPI;

public class SoftExpertException : Exception
{
    public new string Message { get; private set; }
    public new int Code { get; private set; }
    public SoftExpertResponse Response { get; private set; }
    public new string RequestSent { get; protected set; }

    public new string ResponseReceived { get; protected set; }
    public SoftExpertException(string message, int code = 0, SoftExpertResponse RetornoSE = null) : base(message)
    {
        Message = message;
        Response = RetornoSE;
        Code = code;
    }

    internal void setRequestSent(string xml)
    {
        RequestSent = xml;
    }

    internal void setResponseReceived(string xml)
    {
        ResponseReceived = xml;
    }
}

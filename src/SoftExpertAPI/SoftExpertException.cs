using System;

namespace SoftExpert
{
    public class SoftExpertException : Exception
    {
        public new string Message { get; private set; }
        public SoftExpertResponse Response { get; private set; }
        public new string XMLSoapSent { get; protected set; }
        public SoftExpertException(string message, SoftExpertResponse RetornoSE = null) : base(message)
        {
            Message = message;
            Response = RetornoSE;
        }

        internal void setXMLSoapSent(string xml)
        {
            XMLSoapSent = xml;
        }
    }
}

using System;

namespace SoftExpert
{
    public class SoftExpertException : Exception
    {
        public new string Message { get; private set; }
        public SoftExpertResponse Response { get; private set; }
        public SoftExpertException(string message, SoftExpertResponse RetornoSE) : base(message)
        {
            Message = message;
            Response = RetornoSE;
        }
    }
}

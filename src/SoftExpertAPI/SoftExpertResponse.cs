using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Xml;

namespace SoftExpert
{
    public abstract class SoftExpertResponse
    {

        public STATUS Status { get; internal set; }
        public int Code { get; internal set; }
        public string Detail { get; internal set; }

        public new string XMLSoapSent { get; protected set; }

        public enum STATUS { 
            SUCCESS,
            FAILURE
        }

        public void setXMLSoapSent(string xml)
        {
            XMLSoapSent = xml;
        }


        protected JToken PreParse(string xmljson)
        {
            if (xmljson.Substring(0, 5) == "<?xml")
            {
                // em caso de ter retornado um XML
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmljson);
                xmljson = JsonConvert.SerializeXmlNode(doc);
            }
            var type = this.GetType().Name;

            JToken response = JToken.Parse(xmljson).SelectToken(type);
            if (response == null)
            {
                response = JToken.Parse(xmljson).SelectToken($"SOAP-ENV:Envelope.SOAP-ENV:Body.{type}");
            }

            if (response == null)
            {
                response = JToken.Parse(xmljson).SelectToken("basicErrorResponse");
            }

            if (response.Count() == 1)
            {
                response = response[0];
            }

            Status = (STATUS) Enum.Parse(typeof(STATUS), response.SelectToken("Status").ToString());
            Code = Int32.Parse(response.SelectToken("Code").ToString());
            Detail = response.SelectToken("Detail").ToString();

            if (Status is not STATUS.SUCCESS)
            {
                throw new SoftExpertException(Detail, this);
            }

            return response;
        }
    }
}

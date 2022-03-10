using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Xml;

namespace SoftExpert
{
    public abstract class SoftExpertResponse
    {

        public string Status { get; protected set; }
        public int Code { get; protected set; }
        public string Detail { get; protected set; }


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

            Status = response.SelectToken("Status").ToString();
            Code = Int32.Parse(response.SelectToken("Code").ToString());
            Detail = response.SelectToken("Detail").ToString();

            if (Code is not 1)
            {
                throw new SoftExpertException($"{type} - {Detail}", this);
            }

            return response;
        }
    }
}

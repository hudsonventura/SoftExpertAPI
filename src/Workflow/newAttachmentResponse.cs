using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Nodes;
using System.Xml;

namespace SoftExpert.Workflow;

public class newAttachmentResponse : SoftExpertResponse
{
    public int AttachmentID { get; private set; }

    internal static newAttachmentResponse Parse(string xmljson)
    {
        newAttachmentResponse parsed = new newAttachmentResponse();
        parsed.PreParse(xmljson);


        if (xmljson.Substring(0, 5) == "<?xml")
        {
            // em caso de ter retornado um XML
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmljson);
            xmljson = JsonConvert.SerializeXmlNode(doc);
        }
        string type = "newAttachmentResponse";

        JObject jsonObject = JObject.Parse(xmljson);
        parsed.AttachmentID = (int)jsonObject.SelectToken("SOAP-ENV:Envelope.SOAP-ENV:Body.newAttachmentResponse.RecordKey");

        return parsed;
    }
}
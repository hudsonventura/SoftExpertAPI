using System;
using System.Text.Json.Nodes;
using System.Xml;

namespace SoftExpertAPI;

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


    protected JsonNode PreParse(string xmljson)
    {
        JsonNode root;
        if (xmljson.StartsWith("<?xml", StringComparison.Ordinal))
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmljson);
            root = JsonNode.Parse(XmlToJsonConverter.Serialize(doc));
        }
        else
        {
            root = JsonNode.Parse(xmljson);
        }

        var type = this.GetType().Name;

        JsonNode response = root.SelectToken(type);
        if (response == null)
        {
            response = root.SelectTokenPath($"SOAP-ENV:Envelope.SOAP-ENV:Body.{type}");
        }

        if (response == null)
        {
            response = root.SelectToken("basicErrorResponse");
        }

        if (response is JsonArray array && array.Count == 1)
        {
            response = array[0];
        }

        Status = (STATUS)Enum.Parse(typeof(STATUS), response.SelectToken("Status").GetScalarString());
        Code = Int32.Parse(response.SelectToken("Code").GetScalarString());
        Detail = response.SelectToken("Detail").GetScalarString();

        if (Status is not STATUS.SUCCESS)
        {
            throw new SoftExpertException(Detail, 0, this);
        }

        return response;
    }
}

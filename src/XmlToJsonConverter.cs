using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace SoftExpertAPI;

/// <summary>
/// Converte XML no formato compatível com o antigo JsonConvert.SerializeXmlNode (Newtonsoft).
/// </summary>
internal static class XmlToJsonConverter
{
    public static string Serialize(XmlDocument document)
    {
        if (document?.DocumentElement == null)
        {
            return "{}";
        }

        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WritePropertyName(document.DocumentElement.Name);
            WriteElementValue(writer, document.DocumentElement);
            writer.WriteEndObject();
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void WriteElementValue(Utf8JsonWriter writer, XmlElement element)
    {
        var childElements = element.ChildNodes
            .Cast<XmlNode>()
            .Where(n => n.NodeType == XmlNodeType.Element)
            .Cast<XmlElement>()
            .ToList();

        var attributes = element.Attributes?.Cast<XmlAttribute>().ToList() ?? new List<XmlAttribute>();

        string text = string.Concat(
            element.ChildNodes
                .Cast<XmlNode>()
                .Where(n => n.NodeType == XmlNodeType.Text || n.NodeType == XmlNodeType.CDATA)
                .Select(n => n.Value ?? string.Empty)).Trim();

        bool hasAttributes = attributes.Count > 0;
        bool hasChildElements = childElements.Count > 0;
        bool hasText = text.Length > 0;

        if (!hasAttributes && !hasChildElements && hasText)
        {
            writer.WriteStringValue(text);
            return;
        }

        if (!hasAttributes && !hasChildElements && !hasText)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        foreach (var attr in attributes)
        {
            writer.WriteString("@" + attr.Name, attr.Value);
        }

        if (hasText && (hasAttributes || hasChildElements))
        {
            writer.WriteString("#text", text);
        }

        foreach (var group in childElements.GroupBy(e => e.Name))
        {
            writer.WritePropertyName(group.Key);
            if (group.Count() == 1)
            {
                WriteElementValue(writer, group.First());
            }
            else
            {
                writer.WriteStartArray();
                foreach (var child in group)
                {
                    WriteElementValue(writer, child);
                }
                writer.WriteEndArray();
            }
        }

        writer.WriteEndObject();
    }
}

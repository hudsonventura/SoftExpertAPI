using System.Linq;
using System.Text.Json.Nodes;

namespace SoftExpertAPI;

internal static class JsonNodeExtensions
{
    public static JsonNode SelectToken(this JsonNode node, string propertyName)
    {
        if (node is JsonObject obj && obj.TryGetPropertyValue(propertyName, out JsonNode child))
        {
            return child;
        }

        return null;
    }

    public static JsonNode SelectTokenPath(this JsonNode node, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return node;
        }

        JsonNode current = node;
        foreach (string segment in path.Split('.'))
        {
            current = current?.SelectToken(segment);
        }

        return current;
    }

    public static string GetScalarString(this JsonNode node)
    {
        if (node is null)
        {
            return null;
        }

        if (node is JsonValue value)
        {
            if (value.TryGetValue<string>(out string s))
            {
                return s;
            }

            if (value.TryGetValue<bool>(out bool b))
            {
                return b ? "true" : "false";
            }

            if (value.TryGetValue<long>(out long l))
            {
                return l.ToString();
            }

            if (value.TryGetValue<double>(out double d))
            {
                return d.ToString();
            }

            return value.ToString();
        }

        return node.ToString();
    }
}

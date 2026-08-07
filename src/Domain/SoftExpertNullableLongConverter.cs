using System;
using Newtonsoft.Json;

namespace Domain;

/// <summary>
/// Converte número SoftExpert ou string vazia para long?.
/// </summary>
public class SoftExpertNullableLongConverter : JsonConverter<long?>
{
    public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Null:
                return null;

            case JsonToken.Integer:
            case JsonToken.Float:
                return Convert.ToInt64(reader.Value);

            case JsonToken.String:
                {
                    string value = reader.Value?.ToString();
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return null;
                    }
                    return long.TryParse(value, out long parsed) ? parsed : null;
                }

            default:
                return null;
        }
    }

    public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }
        writer.WriteValue(value.Value);
    }
}

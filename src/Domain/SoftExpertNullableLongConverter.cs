using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain;

/// <summary>
/// Converte número SoftExpert ou string vazia para long?.
/// </summary>
public class SoftExpertNullableLongConverter : JsonConverter<long?>
{
    public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.Number:
                if (reader.TryGetInt64(out long number))
                {
                    return number;
                }
                return Convert.ToInt64(reader.GetDouble());

            case JsonTokenType.String:
                {
                    string value = reader.GetString();
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

    public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteNumberValue(value.Value);
    }
}

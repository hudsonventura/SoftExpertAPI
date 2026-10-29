using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain;

/// <summary>
/// Converte timestamps SoftExpert (epoch ms) ou string vazia para DateTime?.
/// </summary>
public class SoftExpertEpochDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.Number:
                {
                    long epochMs = reader.TryGetInt64(out long l) ? l : Convert.ToInt64(reader.GetDouble());
                    if (epochMs <= 0)
                    {
                        return null;
                    }
                    return DateTimeOffset.FromUnixTimeMilliseconds(epochMs).LocalDateTime;
                }

            case JsonTokenType.String:
                {
                    string value = reader.GetString();
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return null;
                    }

                    if (long.TryParse(value, out long epochMs) && epochMs > 0)
                    {
                        return DateTimeOffset.FromUnixTimeMilliseconds(epochMs).LocalDateTime;
                    }

                    if (DateTime.TryParse(value, out DateTime parsed))
                    {
                        return parsed;
                    }

                    return null;
                }

            default:
                return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        long epochMs = new DateTimeOffset(value.Value).ToUnixTimeMilliseconds();
        writer.WriteNumberValue(epochMs);
    }
}

using System;
using Newtonsoft.Json;

namespace Domain;

/// <summary>
/// Converte timestamps SoftExpert (epoch ms) ou string vazia para DateTime?.
/// </summary>
public class SoftExpertEpochDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Null:
                return null;

            case JsonToken.Integer:
            case JsonToken.Float:
                {
                    long epochMs = Convert.ToInt64(reader.Value);
                    if (epochMs <= 0)
                    {
                        return null;
                    }
                    return DateTimeOffset.FromUnixTimeMilliseconds(epochMs).LocalDateTime;
                }

            case JsonToken.String:
                {
                    string value = reader.Value?.ToString();
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

    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            writer.WriteNull();
            return;
        }

        long epochMs = new DateTimeOffset(value.Value).ToUnixTimeMilliseconds();
        writer.WriteValue(epochMs);
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;

namespace SoftExpertAPI;

internal static class SoftExpertJsonOptions
{
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static readonly JsonSerializerOptions DataSet = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
            new SoftExpertNullableLongConverter(),
            new SoftExpertEpochDateTimeConverter(),
        },
    };
}

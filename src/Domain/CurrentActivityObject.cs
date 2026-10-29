using System;
using System.Text.Json.Serialization;

namespace Domain;

public record CurrentActivityObject
{
    [JsonPropertyName("IDPROCESS")]
    public string idprocess { get; init; } = string.Empty;

    [JsonPropertyName("IDOBJECT")]
    public string idobject { get; init; } = string.Empty;

    [JsonPropertyName("IDSTRUCT")]
    public string idstruct { get; init; } = string.Empty;

    [JsonPropertyName("NMSTRUCT")]
    public string nmstruct { get; init; } = string.Empty;

    [JsonPropertyName("FGSTATUS")]
    public int fgstatus { get; init; }

    [JsonPropertyName("DTENABLED")]
    public long? dtenabled { get; init; }

    [JsonPropertyName("DTESTIMATEDFINISH")]
    public long? dtestimatedfinish { get; init; }

    [JsonPropertyName("DTEXECUTION")]
    public long? dtexecution { get; init; }

    public WFStruct ToWFStruct()
    {
        return new WFStruct
        {
            idprocess = idprocess,
            idobject = idobject,
            idstruct = idstruct,
            nmstruct = nmstruct,
            fgstatus = (WFStruct.STStatus)fgstatus,
            dhenabled = FromEpoch(dtenabled),
            dtestimatedfinish = FromEpoch(dtestimatedfinish),
            dtexecution = FromEpoch(dtexecution),
        };
    }

    private static DateTime FromEpoch(long? epochMs)
    {
        if (epochMs is null || epochMs <= 0)
        {
            return DateTime.MinValue;
        }

        return DateTimeOffset.FromUnixTimeMilliseconds(epochMs.Value).LocalDateTime;
    }
}

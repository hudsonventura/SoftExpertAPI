using System;
using Newtonsoft.Json;

namespace Domain;

public record CurrentActivityObject
{
    [JsonProperty("IDPROCESS")]
    public string idprocess { get; init; } = string.Empty;

    [JsonProperty("IDOBJECT")]
    public string idobject { get; init; } = string.Empty;

    [JsonProperty("IDSTRUCT")]
    public string idstruct { get; init; } = string.Empty;

    [JsonProperty("NMSTRUCT")]
    public string nmstruct { get; init; } = string.Empty;

    [JsonProperty("FGSTATUS")]
    public int fgstatus { get; init; }

    [JsonProperty("DTENABLED")]
    public long? dtenabled { get; init; }

    [JsonProperty("DTESTIMATEDFINISH")]
    public long? dtestimatedfinish { get; init; }

    [JsonProperty("DTEXECUTION")]
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

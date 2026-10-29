using System;
using System.Text.Json.Serialization;

namespace Domain;

public record ManageInstanceObject
{
    [JsonPropertyName("IDPROCESS")]
    public string idprocess { get; init; } = string.Empty;

    [JsonPropertyName("IDOBJECT")]
    public string p_idobject { get; init; } = string.Empty;

    [JsonPropertyName("FGSTATUS")]
    public int p_fgstatus { get; init; }

    [JsonPropertyName("CDUSERSTART")]
    public int cduserstart { get; init; }

    [JsonPropertyName("NMPROCESS")]
    public string nmprocess { get; init; } = string.Empty;

    [JsonPropertyName("CDPROCESSMODEL")]
    public int? cdprocessmodel { get; init; }

    [JsonPropertyName("IDPROCESSMODEL")]
    public string idprocessmodel { get; init; } = string.Empty;

    [JsonPropertyName("NMPROCESSMODEL")]
    public string nmprocessmodel { get; init; } = string.Empty;

    [JsonPropertyName("IDREVISION")]
    public string idrevision { get; init; } = string.Empty;

    [JsonPropertyName("DTSTART")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dtstart { get; init; }

    [JsonPropertyName("TMSTART")]
    public string tmstart { get; init; } = string.Empty;

    [JsonPropertyName("DHSTART")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dhstart { get; init; }

    [JsonPropertyName("DTFINISH")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dtfinish { get; init; }

    [JsonPropertyName("TMFINISH")]
    public string tmfinish { get; init; } = string.Empty;

    [JsonPropertyName("DHFINISH")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dhfinish { get; init; }

    [JsonPropertyName("OIDENTITYREG")]
    public string oidentityreg { get; init; } = string.Empty;

    [JsonIgnore]
    public WFStruct.WFStatus Status => (WFStruct.WFStatus)p_fgstatus;
}

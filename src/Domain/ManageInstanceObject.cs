using System;
using Newtonsoft.Json;

namespace Domain;

public record ManageInstanceObject
{
    [JsonProperty("IDPROCESS")]
    public string idprocess { get; init; } = string.Empty;

    [JsonProperty("IDOBJECT")]
    public string p_idobject { get; init; } = string.Empty;

    [JsonProperty("FGSTATUS")]
    public int p_fgstatus { get; init; }

    [JsonProperty("CDUSERSTART")]
    public int cduserstart { get; init; }

    [JsonProperty("NMPROCESS")]
    public string nmprocess { get; init; } = string.Empty;

    [JsonProperty("CDPROCESSMODEL")]
    public int? cdprocessmodel { get; init; }

    [JsonProperty("IDPROCESSMODEL")]
    public string idprocessmodel { get; init; } = string.Empty;

    [JsonProperty("NMPROCESSMODEL")]
    public string nmprocessmodel { get; init; } = string.Empty;

    [JsonProperty("IDREVISION")]
    public string idrevision { get; init; } = string.Empty;

    [JsonProperty("DTSTART")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dtstart { get; init; }

    [JsonProperty("TMSTART")]
    public string tmstart { get; init; } = string.Empty;

    [JsonProperty("DHSTART")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dhstart { get; init; }

    [JsonProperty("DTFINISH")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dtfinish { get; init; }

    [JsonProperty("TMFINISH")]
    public string tmfinish { get; init; } = string.Empty;

    [JsonProperty("DHFINISH")]
    [JsonConverter(typeof(SoftExpertEpochDateTimeConverter))]
    public DateTime? dhfinish { get; init; }

    [JsonProperty("OIDENTITYREG")]
    public string oidentityreg { get; init; } = string.Empty;

    [JsonIgnore]
    public WFStruct.WFStatus Status => (WFStruct.WFStatus)p_fgstatus;
}

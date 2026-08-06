using Newtonsoft.Json;

namespace Domain;

public record ManageInstanceObject
{
    [JsonProperty("P_IDOBJECT")]
    public string p_idobject { get; init; } = string.Empty;

    [JsonProperty("S_IDOBJECT")]
    public string s_idobject { get; init; } = string.Empty;

    [JsonProperty("IDSTRUCT")]
    public string idstruct { get; init; } = string.Empty;

    [JsonProperty("NMSTRUCT")]
    public string nmstruct { get; init; } = string.Empty;

    [JsonProperty("IDPROCESS")]
    public string idprocess { get; init; } = string.Empty;

    [JsonProperty("NRORDER")]
    public int nrorder { get; init; }

    [JsonProperty("DTENABLED")]
    public long dtenabled { get; init; }

    [JsonProperty("FGSTATUS")]
    public int p_fgstatus { get; init; }

    [JsonIgnore]
    public WFStatus Status => (WFStatus)p_fgstatus;
}

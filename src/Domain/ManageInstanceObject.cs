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

    [JsonProperty("OIDENTITYREG")]
    public string oidentityreg { get; init; } = string.Empty;

    [JsonIgnore]
    public WFStruct.WFStatus Status => (WFStruct.WFStatus)p_fgstatus;
}

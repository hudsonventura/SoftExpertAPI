using Newtonsoft.Json;

namespace Domain;

/// <summary>
/// Metadados de arquivo retornados pelo dataset queryGetAttachmentFile.
/// TYPE 1 = formulário (SEBLOB), TYPE 2 = anexo de instância.
/// </summary>
public record AttachmentFileObject
{
    [JsonProperty("TYPE")]
    public int type { get; init; }

    [JsonProperty("IDSTRUCT")]
    public string idstruct { get; init; } = string.Empty;

    [JsonProperty("NMFILE")]
    public string nmfile { get; init; } = string.Empty;

    [JsonProperty("CDFILE")]
    [JsonConverter(typeof(SoftExpertNullableLongConverter))]
    public long? cdfile { get; init; }

    [JsonProperty("CDATTACHMENT")]
    [JsonConverter(typeof(SoftExpertNullableLongConverter))]
    public long? cdattachment { get; init; }

    [JsonProperty("OID")]
    public string oid { get; init; } = string.Empty;

    [JsonIgnore]
    public bool IsFormFile => type == 1;

    [JsonIgnore]
    public bool IsInstanceAttachment => type == 2;

    [JsonIgnore]
    public string Extension
    {
        get
        {
            if (string.IsNullOrWhiteSpace(nmfile))
            {
                return string.Empty;
            }
            int idx = nmfile.LastIndexOf('.');
            return idx >= 0 && idx < nmfile.Length - 1 ? nmfile.Substring(idx + 1) : string.Empty;
        }
    }
}

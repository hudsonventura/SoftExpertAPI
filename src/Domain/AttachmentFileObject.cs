using System.Text.Json.Serialization;

namespace Domain;

/// <summary>
/// Metadados de arquivo retornados pelo dataset queryGetAttachmentFile.
/// TYPE 1 = formulário (SEBLOB), TYPE 2 = anexo de instância.
/// </summary>
public record AttachmentFileObject
{
    [JsonPropertyName("TYPE")]
    public int type { get; init; }

    [JsonPropertyName("IDSTRUCT")]
    public string idstruct { get; init; } = string.Empty;

    [JsonPropertyName("NMFILE")]
    public string nmfile { get; init; } = string.Empty;

    [JsonPropertyName("CDFILE")]
    [JsonConverter(typeof(SoftExpertNullableLongConverter))]
    public long? cdfile { get; init; }

    [JsonPropertyName("CDATTACHMENT")]
    [JsonConverter(typeof(SoftExpertNullableLongConverter))]
    public long? cdattachment { get; init; }

    [JsonPropertyName("OID")]
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

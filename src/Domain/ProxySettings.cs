namespace SoftExpertAPI;

public class ProxySettings
{
    public string url { get; set; } = string.Empty;
    public ProxyType type { get; set; } = ProxyType.Http;
    public string login { get; set; } = string.Empty;
    public string pass { get; set; } = string.Empty;

    public bool IsConfigured => !string.IsNullOrWhiteSpace(url);
}
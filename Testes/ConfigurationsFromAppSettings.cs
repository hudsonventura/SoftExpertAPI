using Microsoft.Extensions.Configuration;
using SoftExpertAPI;

namespace Testes;

public static class ConfigurationsFromAppSettings
{
    public static Configurations Create(IConfiguration appsettings)
    {
        var configs = new Configurations
        {
            baseUrl = appsettings["url"],
            login = appsettings["user"],
            pass = appsettings["pass"],
            domain = appsettings["domain"],
            token = appsettings["token"],
        };

        if (!string.IsNullOrWhiteSpace(appsettings["authorization"]))
        {
            configs.token = appsettings["authorization"];
        }

        var proxySection = appsettings.GetSection("proxy");
        if (proxySection.Exists() && !string.IsNullOrWhiteSpace(proxySection["url"]))
        {
            var proxyType = ProxyType.Http;
            if (!string.IsNullOrWhiteSpace(proxySection["type"])
                && !Enum.TryParse(proxySection["type"], ignoreCase: true, out proxyType))
            {
                throw new InvalidOperationException($"Valor inválido em proxy:type: '{proxySection["type"]}'.");
            }

            configs.proxy = new ProxySettings
            {
                url = proxySection["url"],
                type = proxyType,
                login = proxySection["login"] ?? string.Empty,
                pass = proxySection["pass"] ?? string.Empty,
            };
        }

        return configs;
    }
}
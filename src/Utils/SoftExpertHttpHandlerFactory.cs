using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;

namespace SoftExpertAPI;

public static class SoftExpertHttpHandlerFactory
{
    public static HttpMessageHandler Create(Configurations configs)
    {
        var handler = new SocketsHttpHandler
        {
            SslOptions = new SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = static (_, _, _, _) => true
            }
        };

        var proxy = configs?.proxy;
        if (proxy == null || !proxy.IsConfigured)
        {
            handler.UseProxy = false;
            return handler;
        }

        Uri proxyUri = BuildProxyUri(proxy);
        var webProxy = new WebProxy(proxyUri);

        if (!string.IsNullOrEmpty(proxy.login))
        {
            webProxy.Credentials = new NetworkCredential(proxy.login, proxy.pass ?? string.Empty);
        }

        handler.Proxy = webProxy;
        handler.UseProxy = true;
        return handler;
    }

    private static Uri BuildProxyUri(ProxySettings proxy)
    {
        string url = proxy.url.Trim();
        if (url.Contains("://", StringComparison.Ordinal))
        {
            return new Uri(url);
        }

        string scheme = SchemeForType(proxy.type);
        return new Uri($"{scheme}://{url}");
    }

    private static string SchemeForType(ProxyType type) => type switch
    {
        ProxyType.Http => "http",
        ProxyType.Https => "https",
        ProxyType.Socks4 => "socks4",
        ProxyType.Socks4a => "socks4a",
        ProxyType.Socks5 => "socks5",
        _ => "http"
    };
}
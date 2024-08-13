using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using SoftExpert;

namespace Examples;

public class ExampleFileDownloadImplementation : IFileDownload
{
    private enum AccessType{
        DirectAccess,
        TunneledAccess
    }
    public ExampleFileDownloadImplementation(IConfiguration appsettings)
    {
        if (!string.IsNullOrWhiteSpace(appsettings["SSH:Gateway:host"])){
            GatewayHost = appsettings["SSH:Gateway:host"].ToString();
            GatewayPort = int.Parse(appsettings["SSH:Gateway:port"].ToString());
            GatewayUsername = appsettings["SSH:Gateway:user"].ToString();
            GatewayPassword = appsettings["SSH:Gateway:pass"].ToString();
            Type = AccessType.TunneledAccess;
        }
        

        RemoteHost = appsettings["SSH:Server:host"].ToString();
        RemotePort = int.Parse(appsettings["SSH:Server:port"].ToString());
        RemoteUsername = appsettings["SSH:Server:user"].ToString();
        RemotePassword = appsettings["SSH:Server:pass"].ToString();
        RemoteFilePath = appsettings["SSH:Server:path"].ToString();

    }

    private AccessType Type = AccessType.DirectAccess;

    private string GatewayHost { get; }
    private int GatewayPort { get; }
    private string GatewayUsername { get; }
    private string GatewayPassword { get; }

    private string RemoteHost { get; }
    private int RemotePort { get; }
    private string RemoteUsername { get; }
    private string RemotePassword { get; }
    private string RemoteFilePath { get; }
    public Anexo DownloadFile(Anexo anexo)
    {
        string remoteFilePath = $"{RemoteFilePath}{anexo.cdfile.ToString($"D{8}")}.{anexo.extension}";

        switch (Type)
        {
            case AccessType.DirectAccess: return DownloadDirect(remoteFilePath, anexo, RemoteHost, RemotePort, RemoteUsername, RemotePassword);
            case AccessType.TunneledAccess: return DownloadWithTunelSSH(remoteFilePath, anexo);
        }
        return null;
    }



    private Anexo DownloadDirect(string remoteFilePath, Anexo anexo, string host, int port, string user, string pass){
        using (var sftp = new SftpClient(host, port, user, pass))
        {
            try
            {
                sftp.Connect();
                using (var memoryStream = new MemoryStream())
                {
                    sftp.DownloadFile(remoteFilePath, memoryStream);
                    anexo.Content = memoryStream.ToArray();
                    return anexo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível realizar o download do arquivo de diretório via SSH. Erro: {ex.Message}");
            }
        }
    }

    private Anexo DownloadWithTunelSSH(string remoteFilePath, Anexo anexo){
        using (var client = new SshClient(GatewayHost, GatewayPort, GatewayUsername, GatewayPassword))
        {
            client.Connect();

            var portForwarded = new ForwardedPortLocal("127.0.0.1", 55, RemoteHost, (uint)RemotePort);
            client.AddForwardedPort(portForwarded);
            portForwarded.Start();

            return DownloadDirect(remoteFilePath, anexo, "127.0.0.1", 55, RemoteUsername, RemotePassword);
        }
    }
}

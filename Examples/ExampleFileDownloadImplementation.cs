using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using SoftExpert;

namespace Examples;

public class ExampleFileDownloadImplementation : IFileDownload
{
    public ExampleFileDownloadImplementation(IConfiguration appsettings)
    {
        GatewayHost = appsettings["SSH:Gateway:host"].ToString();
        GatewayPort = int.Parse(appsettings["SSH:Gateway:port"].ToString());
        GatewayUsername = appsettings["SSH:Gateway:user"].ToString();
        GatewayPassword = appsettings["SSH:Gateway:pass"].ToString();

        RemoteHost = appsettings["SSH:Server:host"].ToString();
        RemotePort = uint.Parse(appsettings["SSH:Server:port"].ToString());
        RemoteUsername = appsettings["SSH:Server:user"].ToString();
        RemotePassword = appsettings["SSH:Server:pass"].ToString();
        RemoteFilePath = appsettings["SSH:Server:path"].ToString();

    }

    private string GatewayHost { get; }
    private int GatewayPort { get; }
    private string GatewayUsername { get; }
    private string GatewayPassword { get; }

    private string RemoteHost { get; }
    private uint RemotePort { get; }
    private string RemoteUsername { get; }
    private string RemotePassword { get; }
    private string RemoteFilePath { get; }
    public Anexo DownloadFile(Anexo anexo)
    {
        string remoteFilePath = $"{RemoteFilePath}{anexo.cdfile.ToString($"D{8}")}.{anexo.extension}";

        using (var client = new SshClient(GatewayHost, GatewayPort, GatewayUsername, GatewayPassword))
        {
            client.Connect();

            var portForwarded = new ForwardedPortLocal("127.0.0.1", 55, RemoteHost, RemotePort);
            client.AddForwardedPort(portForwarded);
            portForwarded.Start();

            using (var sftp = new SftpClient("127.0.0.1", 55, RemoteUsername, RemotePassword))
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
                finally
                {
                    sftp.Disconnect();
                    portForwarded.Stop();
                    client.Disconnect();
                }
            }

            
        }
    }
}

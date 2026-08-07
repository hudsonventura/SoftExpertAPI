using System.Text;
using SoftExpertAPI;

namespace Testes_Worflow;

/// <summary>
/// Mock de IFileDownload para testes de workflow (não acessa SSH/SFTP).
/// </summary>
public class MockFileDownload : IFileDownload
{
    public byte[] DownloadFileAttach(string filename)
    {
        return Encoding.UTF8.GetBytes($"mock-attach-content:{filename}");
    }

    public byte[] DownloadFileForm(string filename)
    {
        return Encoding.UTF8.GetBytes($"mock-form-content:{filename}");
    }
}

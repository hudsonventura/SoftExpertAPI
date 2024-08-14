namespace SoftExpertAPI;

public interface IFileDownload
{
    /// <summary>
    /// Realiza o download de um arquivo do tipo anexo de um diretório controlado
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    byte[] DownloadFileAttach(string filename);



    /// <summary>
    /// Realiza o download de um arquivo de formulário de um diretório controlado
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    byte[] DownloadFileForm(string filename);
}

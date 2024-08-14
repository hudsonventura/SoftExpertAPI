using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftExpertAPI;

/// <summary>
/// Objeto que representa uma anexo no SE
/// </summary>
public class Anexo
{
    /// <summary>
    /// Código do arquivo. Numero inteiro
    /// </summary>
    public Int64 cdfile { get; set; }

    /// <summary>
    /// Código de anexo. Numero inteiro
    /// </summary>
    public Int64 cdattachment { get; set; }

    /// <summary>
    /// String contendo o nome do arquivo
    /// </summary>
    public string FileName { get; set; }


    /// <summary>
    /// String contendo apenas a extensão do arquivo. Ex. doc, docx, txt, png, jpg
    /// </summary>
    public string extension { get; set; }

    /// <summary>
    /// Conteudo binário do arquivo
    /// </summary>
    public byte[] Content { get; set; }

    /// <summary>
    /// Nome do usuario que anexou o arquivo no SE
    /// </summary>
    public string nmuserupd { get; set; }


}

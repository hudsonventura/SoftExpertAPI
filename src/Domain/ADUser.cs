using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace src.Domain;

public class ADUser
{
    /// <summary>
    /// Código do usuário
    /// </summary>
    [Column("CDUSER")]
    public int cduser { get; set; }

    /// <summary>
    /// Nome do usuário
    /// </summary>
    [Column("NMUSER")]
    public string? nmuser { get; set; }

    /// <summary>
    /// Login do usuário
    /// </summary>
    [Column("IDLOGIN")]
    public string? idlogin { get; set; }


    /// <summary>
    /// Matrícula do usuário
    /// </summary>
    [Column("IDUSER")]
    public string? iduser { get; set; }

    /// <summary>
    /// Email do usuário
    /// </summary>
    [Column("NMUSEREMAIL")]
    public string? nmuseremail { get; set; }


    /// <summary>
    /// Informação de que está habilitado ou não. 1 para ativo e 2 para inativo
    /// </summary>
    [Column("FGUSERENABLED")]
    public FGUserEnabled? fguserenabled { get; set; }

    /// <summary>
    /// Informação de que o usuário está habilitado ou não em string
    /// </summary>
    [NotMapped]
    public string status
    {
        get
        {
            return fguserenabled.ToString();
        }
        set
        {
            
        }
    }

    public enum FGUserEnabled {
        Ativo = 1,
        Inativo = 2,
        Desativado = 0
    }


    public static ADUser ConvertDataRowToADUser(DataRow row)
    {
        var adUser = new ADUser();

        foreach (DataColumn column in row.Table.Columns)
        {
            var propertyInfo = typeof(ADUser).GetProperty(column.ColumnName.ToLower());

            if (propertyInfo != null && row[column] != DBNull.Value)
            {
                try
                {
                    propertyInfo.SetValue(adUser, Convert.ChangeType(row[column], propertyInfo.PropertyType), null);
                }
                catch (System.Exception)
                {
                    
                }
            }
        }

        return adUser;
    }
}

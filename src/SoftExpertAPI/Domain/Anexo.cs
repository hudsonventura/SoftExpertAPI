using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftExpertAPI.Domain;

public class Anexo
{
    

    public Int64 cdfile { get; set; }
    public Int64 cdattachment { get; set; }
    public string FileName { get; set; }
    public byte[] Content { get; set; }
}

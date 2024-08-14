using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftExpertAPI.Utils;

internal class Zip
{
    public static byte[] UnzipFile(byte[] arquivoZip)
    {
        using (MemoryStream zipStream = new MemoryStream(arquivoZip))
        {
            using (MemoryStream extractedStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (!string.IsNullOrEmpty(entry.Name))
                        {
                            using (Stream entryStream = entry.Open())
                            {
                                entryStream.CopyTo(extractedStream);
                            }
                        }
                    }
                }

                byte[] extractedData = extractedStream.ToArray();
                return extractedData;
            }
        }
    }
}

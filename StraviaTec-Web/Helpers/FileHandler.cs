using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StraviaTec_Web.Helpers
{
    public enum StorageLocation
    {
        ImageUploads,
        InscriptionUploads,
    }
    public static class FileHandler
    {
        public static string Host => "http://127.0.0.1:5001/uploads/";

        public static async Task<string> SaveFileAsync(string file, string name, StorageLocation location) 
        {
            string fileSuffix = location switch
            {
                StorageLocation.ImageUploads => "-pic.png",
                StorageLocation.InscriptionUploads => "-voucher.png",
                _ => "-doc.png",
            };
            
            var uploads = Path.Combine(Environment.CurrentDirectory, "WebUploads");

            try 
            {
                var fileBytes = Convert.FromBase64String(file[(file.IndexOf(",") + 1)..]);

                if (file.Length == 0) return null;

                var fileName = $"{name}{fileSuffix}";
                string filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) 
                {
                    using var memStream = new MemoryStream(fileBytes);
                    await memStream.CopyToAsync(stream);
                }

                return $"{Host}{fileName}";
            } catch (Exception) {
                //Base64 decoding failed
            }

            return null;
        }
        
    }
}
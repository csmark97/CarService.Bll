using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CarService.Dal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarService.Bll.Helper
{
    public class FileHelpers
    {
        public static async Task UploadAsync(string path, FileUpload fileUpload)
        {
            var filePath = path + fileUpload.Upload.FileName;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await fileUpload.Upload.CopyToAsync(fileStream);
            }
        }
    }
}

using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Microsoft.AspNetCore.Http;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Revoow.Services
{
    public class VideoService
    {
        private readonly BlobStorageService storageService;

        //private string location = Environment.GetEnvironmentVariable("TEMP");
        public string fileName;
        public string thumbnailPath;
        public string videoPath;
        public string location = @"D:\local\Temp";

        public VideoService(BlobStorageService storageService)
        {
            //filename is current time stripped of all non numbers
            this.fileName = Regex.Replace(DateTime.Now.ToString(), "[^0-9]", "");
            this.videoPath = Path.Combine(location, fileName + ".webm");
            this.thumbnailPath = Path.Combine(location, fileName + ".jpg");
            this.storageService = storageService;
        }

        public async Task<byte[]> GenerateThumbnail(string fileName)
        {
            var downloadedFile = await storageService.GetFileFromStorage(fileName);
            var inputFile = new MediaFile { Filename =  downloadedFile };

            var outputFile = new MediaFile { Filename = thumbnailPath };

            using (var engine = new Engine(@"D:\home\site\wwwroot\wwwroot\ffmpeg.exe"))
            {
                engine.GetMetadata(inputFile);

                //Grab thumbnail from time video length / 2
                var interval = TimeSpan.FromSeconds(inputFile.Metadata.Duration.TotalSeconds / 2);
                var options = new ConversionOptions { Seek = interval };
                engine.GetThumbnail(inputFile, outputFile, options);
            }

            byte[] imageData;

            using (var fs = new FileStream(thumbnailPath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    imageData = ms.ToArray();
                }
            }

            return imageData;
        }

        public void SaveVideo(IFormFile file)
        {
            using (var stream = new FileStream(videoPath, FileMode.Create))
            {
                file.CopyTo(stream);

            }
        }
    }
}

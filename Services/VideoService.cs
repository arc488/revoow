using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Revoow.Services
{
    public class VideoService
    {
        private string location = Environment.GetEnvironmentVariable("TEMP");
        public string fileName;
        public string thumbnailPath;
        public string videoPath;

        public VideoService()
        {
            //filename is current time stripped of all non numbers
            this.fileName = Regex.Replace(DateTime.Now.ToString(), "[^0-9]", "");
            this.videoPath = Path.Combine(location, fileName + ".webm");
            this.thumbnailPath = Path.Combine(location, fileName + ".jpg");
        }

        public byte[] GenerateThumbnail()
        {
            var inputFile = new MediaFile { Filename =  videoPath};
            var outputFile = new MediaFile { Filename = thumbnailPath};

            using (var engine = new Engine())
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

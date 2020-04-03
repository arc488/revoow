using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Services
{
    public class GenerateThumbnailService
    {
        private string location = Environment.GetEnvironmentVariable("TEMP");

        public void GenerateThumbnail(string videoName)
        {
            var inputFile = new MediaFile { Filename = Path.Combine(location, videoName + ".webm") };
            var outputFile = new MediaFile { Filename = Path.Combine(location, videoName + ".jpg") };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                // Saves the frame located on the 15th second of the video.
                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(inputFile.Metadata.Duration.TotalSeconds/2) };
                engine.GetThumbnail(inputFile, outputFile, options);
            }
        }
    }
}

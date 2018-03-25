using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHAT.BloggerToWyam.ConsoleApp.Model;
using Newtonsoft.Json;

namespace MHAT.BloggerToWyam.ConsoleApp.Bll
{
    public class ImageProcessor
    {
        public ImageProcessor()
        {
            DownloadedImageDict = new Dictionary<string, string>();
        }

        public Dictionary<string,string> DownloadedImageDict { get; set; }

        public void PrepareImageDict(string imageDirctory)
        {
            var allImageFiles = Directory.GetFiles(imageDirctory)
                .Where(x => x.EndsWith(".json") == false).ToList();

            var metaFiles = Directory.GetFiles(imageDirctory, "*.json");

            foreach (var file in metaFiles)
            {
                var model = JsonConvert.DeserializeObject<ImageMeta>(File.ReadAllText(file));

                if (string.IsNullOrEmpty(model.url) == false)
                {
                    DownloadedImageDict.Add(model.url,
                       Path.Combine(imageDirctory, allImageFiles.First(x => x.Contains(model.title))));
                }
            }
        }

        public void DownloadImages(BlogPostModel model, string postPath)
        {
            
        }

        internal void PrepareImageDict(object offLineImagePath)
        {
            throw new NotImplementedException();
        }
    }
}

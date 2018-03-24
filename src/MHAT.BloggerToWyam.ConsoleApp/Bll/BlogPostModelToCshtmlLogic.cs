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
    public class BlogPostModelToCshtmlLogic
    {
        public string ToCshtmlTemplateString(BlogPostModel model)
        {
            var template = GetTemplate();

            template = ReplaceTemplate(template, "Title", model.Title);
            template = ReplaceTemplate(template, "PublishedDate", DateString(model.PublishedDate));
            template = ReplaceTemplate(template, "Modified", DateString(model.ModifyDate));
            template = ReplaceTemplate(template, "Tags", JsonConvert.SerializeObject(model.Tags));
            template = ReplaceTemplate(template, "RedirectFrom", model.Url.TrimStart("http://blog.alantsai.net/".ToCharArray()));

            template = ReplaceTemplate(template, "Content", model.Content);

            return template;
        }

        private string DateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        private string ReplaceTemplate(string template, string v, string title)
        {
            return template.Replace("{{" + v + "}}", title);
        }

        private string GetTemplate()
        {
            return File.ReadAllText("Template\\BlogPostTemplate.cshtml");
        }
    }
}

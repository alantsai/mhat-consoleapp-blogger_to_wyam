using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHAT.BloggerToWyam.ConsoleApp.Model;

namespace MHAT.BloggerToWyam.ConsoleApp.Bll.Helper
{
    public static class FeedEntryExtension
    {
        public static BlogPostModel ToBlogPostModel(this feedEntry entry)
        {
            var link = entry.link.First(x => x.rel == "alternate" && x.type == "text/html");

            var result = new BlogPostModel();

            result.Id = entry.id;
            result.Title = entry.title.Value;
            result.PublishedDate = entry.published;
            result.ModifyDate = entry.updated;
            result.Tags = entry.category.Where(x => x.scheme.EndsWith("ns#")).Select(x => x.term).ToList();
            result.Url = link.href;

            return result;
        }
    }
}

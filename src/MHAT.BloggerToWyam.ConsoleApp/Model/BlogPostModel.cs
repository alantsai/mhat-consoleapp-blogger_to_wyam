﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHAT.BloggerToWyam.ConsoleApp.Model
{
    public class BlogPostModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Content { get; set; }
        public IEnumerable<BlogImage> Images { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"id: {Id}");
            sb.AppendLine($"Title: {Title}");
            sb.AppendLine($"Url: {Url}");
            sb.AppendLine($"PublishedDate: {PublishedDate}");
            sb.AppendLine($"ModifyDate: {ModifyDate}");
            sb.AppendLine($"Tags: {string.Join(",", Tags)}");

            return sb.ToString();
        }
    }
}

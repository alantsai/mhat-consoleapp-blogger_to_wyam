﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using AngleSharp.Parser.Html;
using AngleSharp.XHtml;
using MHAT.BloggerToWyam.ConsoleApp.Bll.Helper;
using MHAT.BloggerToWyam.ConsoleApp.Model;
using MHAT.ConsoleApp.ProcessTemplate;

namespace MHAT.BloggerToWyam.ConsoleApp.Bll.Process
{
    public class XmlBackupProcess : BaseExecuteProcessTemplate<XmlBackupProcessOption>
    {
        protected override void PreProcess()
        {
            base.PreProcess();
        }

        protected override void Process()
        {
            var takeoutImage = ArugemntOption.OffLineImagePath;
            var postPath = ArugemntOption.FinalOutputPostPath;

            Console.WriteLine($"處理檔案：{ArugemntOption.BackupXmlPath}");

            XmlSerializer ser = new XmlSerializer(typeof(feed));
            feed feed;
            using (XmlReader reader = XmlReader.Create(ArugemntOption.BackupXmlPath))
            {
                feed = (feed)ser.Deserialize(reader);
            }

            // 取得文章
            var posts = feed.entry.Where(x => x.category.Any(y => y.scheme.EndsWith("kind") && y.term.EndsWith("post")));

            // ShowAllPostInfo(posts);

            // ShowFirstPostStrongModel(posts);

            var BlogPosts = posts.Select(x => x.ToBlogPostModel()).ToList();

            var imageProcessor = new ImageProcessor();
            imageProcessor.PrepareImageDict(takeoutImage);

            var index = 1;

            var wrongList = new List<BlogPostModel>();

            // 處理每一個文章
            foreach (var post in BlogPosts)
            {
                try
                {
                    imageProcessor.ProcessImage(post, postPath);

                    ProcessContentTag(post);
                    ProcessContent(post);

                    var toCshtmlBll = new BlogPostModelToCshtmlLogic();
                    // Console.WriteLine(toCshtmlBll.ToCshtmlTemplateString(BlogPosts.First()));

                    File.WriteAllText(Path.Combine(postPath, post.NewFileName), toCshtmlBll.ToCshtmlTemplateString(post));

                    Console.WriteLine($"{index}/{BlogPosts.Count} 完成: {post.Title}");

                    index++;
                }
                catch (Exception ex)
                {
                    wrongList.Add(post);
                }
            }

            Console.WriteLine("============");

            foreach (var item in wrongList)
            {
                Console.WriteLine($"{item.Title} - 出錯");
            }

            Console.WriteLine("==============");

            // 列印出應該放在blogger的xml内容
            foreach (var post in BlogPosts)
            {
                var postUrlWithoutExtension = post.UrlWithouDomain.Substring(0, post.UrlWithouDomain.LastIndexOf("."));

                // 修改blogger的名稱為自己的
                var bloggerName = "alantsai2007";
                var template = @"<b:if cond='data:blog.canonicalUrl == ""http://" + bloggerName + ".blogspot.com/" + post.UrlWithouDomain + "\"'/>" +
                                    @"<link rel=""canonical"" href=""http://blog.alantsai.net/posts/" + postUrlWithoutExtension + "\"/>";
                                    // 如果要自動跳轉，加入下面那行
                                    //@"<meta http-equiv=""refresh"" content=""0; url=href=""http://blog.alantsai.net/posts/" + postUrlWithoutExtension + "\"/>"

                Console.WriteLine(template);
                Console.WriteLine();
            }

            Console.WriteLine("完成");
        }

        private static void ProcessContent(BlogPostModel post)
        {
            // Razor需要escape @
            post.Content = post.Content.Replace("@", "@@");

            var parser = new HtmlParser();

            var document = parser.Parse(post.Content);

            AddImageClassResponsive(document);

            var pres = document.QuerySelectorAll("pre");

            foreach (var pre in pres)
            {
                var regex = @"brush\:\s([a-zA-z]+)";

                var match = Regex.Match(pre.ClassName, regex);
                var brushName = match.Groups[1];

                // 預設使用 和brush名稱一樣
                var newlanguageName = brushName.Value;

                switch (brushName.Value)
                {
                    case "c-sharp":
                    case "csharp":
                        newlanguageName = "csharp";
                        break;
                    case "ps":
                    case "powershell":
                        newlanguageName = "powershell";
                        break;
                    case "jscript":
                    case "js":
                        newlanguageName = "javascript";
                        break;
                    case "xml":
                    case "xhtml":
                    case "xslt":
                    case "html":
                        newlanguageName = "markup";
                        break;
                    case "plain":
                        newlanguageName = "none";
                        break;
                }

                var code = document.CreateElement("code");

                code.ClassList.Add("language-" + newlanguageName);
                // prism.js line number class
                code.ClassList.Add("line-numbers");

                code.InnerHtml = pre.InnerHtml.Replace("<br />", Environment.NewLine)
                    .Replace("<br>", Environment.NewLine);
                pre.InnerHtml = string.Empty;
                pre.AppendChild(code);
            }

            post.Content = document.QuerySelector("body").InnerHtml;
        }

        private static void AddImageClassResponsive(AngleSharp.Dom.Html.IHtmlDocument document)
        {
            var imgs = document.QuerySelectorAll("img");

            foreach (var img in imgs)
            {
                img.ClassList.Add("img-responsive");
            }
        }

        private static void ShowFirstPostStrongModel(IEnumerable<feedEntry> posts)
        {
            var post = posts.First();

            Console.WriteLine(post.ToBlogPostModel().ToStringWithContent());
        }

        private static void ShowAllPostInfo(IEnumerable<feedEntry> posts)
        {
            Console.WriteLine($"總比數：{posts.Count()}");

            var index = 1;
            index = 1;
            foreach (var item in posts)
            {
                Console.WriteLine($"{index} - {item.published} - Title：{item.title.Value}");

                Console.WriteLine("------------");

                index++;
            }

            var groupByYear = posts.GroupBy(x => x.published.Year);

            foreach (var key in groupByYear)
            {
                Console.WriteLine($"{key.Key} - {key.Count()}");
                Console.WriteLine("-------");
            }
        }

        public void ProcessContentTag(BlogPostModel model)
        {
            model.Content = model.Content.Replace("http://blog.alantsai.net/search/label", "/tags");
        }
    }
}

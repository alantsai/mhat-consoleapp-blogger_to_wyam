using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
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
            var takeoutImage = @"d:\Library\Downloads\blog\image\";
            var postPath = @"d:\Library\Downloads\blog\post\";

            Console.WriteLine($"處理檔案：{ArugemntOption.BackupXmlPath}");

            XmlSerializer ser = new XmlSerializer(typeof(feed));
            feed feed;
            using (XmlReader reader = XmlReader.Create(ArugemntOption.BackupXmlPath))
            {
                feed = (feed)ser.Deserialize(reader);
            }

            var posts = feed.entry.Where(x => x.category.Any(y => y.scheme.EndsWith("kind") && y.term.EndsWith("post")));

            // ShowAllPostInfo(posts);

            // ShowFirstPostStrongModel(posts);

            var BlogPosts = posts.Select(x => x.ToBlogPostModel()).ToList();

            var imageProcessor = new ImageProcessor();
            imageProcessor.PrepareImageDict(takeoutImage);

            var index = 1;

            var wrongList = new List<BlogPostModel>();

            foreach (var post in BlogPosts)
            {
                try
                {
                    imageProcessor.ProcessImage(post, postPath);

                    ProcessContentTag(post);

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

            Console.WriteLine("完成");
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

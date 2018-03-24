using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MHAT.BloggerToWyam.ConsoleApp.Model;
using MHAT.ConsoleApp.ProcessTemplate;

namespace MHAT.BloggerToWyam.ConsoleApp.Bll.Process
{
    public class XmlBackupProcess : BaseExecuteProcessTemplate<XmlBackupProcessOption>
    {
        protected override void Process()
        {
            Console.WriteLine($"處理檔案：{ArugemntOption.BackupXmlPath}");

            XmlSerializer ser = new XmlSerializer(typeof(feed));
            feed feed;
            using (XmlReader reader = XmlReader.Create(ArugemntOption.BackupXmlPath))
            {
                feed = (feed)ser.Deserialize(reader);
            }

            var posts = feed.entry.Where(x => x.category.Any(y => y.scheme.EndsWith("kind") && y.term.EndsWith("post")));

            ShowAllPostInfo(posts);

            Console.WriteLine("完成");
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
    }
}

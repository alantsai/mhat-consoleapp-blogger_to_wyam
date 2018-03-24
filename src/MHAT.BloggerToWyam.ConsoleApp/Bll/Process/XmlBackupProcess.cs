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

            Console.WriteLine($"總比數：{feed.entry.Length}");

            Console.WriteLine("完成");
        }
    }
}

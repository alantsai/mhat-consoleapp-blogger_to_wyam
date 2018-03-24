using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHAT.BloggerToWyam.ConsoleApp.Model;
using MHAT.ConsoleApp.ProcessTemplate;

namespace MHAT.BloggerToWyam.ConsoleApp.Bll.Process
{
    public class XmlBackupProcess : BaseReadInputProcessTemplate<XmlBackupProcessOption>
    {
        protected override void ProcessLine(string line)
        {
            Console.WriteLine($"處理檔案：{ArugemntOption.BackupXmlPath}");



            Console.WriteLine("完成");
        }
    }
}

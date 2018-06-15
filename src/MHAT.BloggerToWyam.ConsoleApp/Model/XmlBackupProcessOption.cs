using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace MHAT.BloggerToWyam.ConsoleApp.Model
{
    public class XmlBackupProcessOption
    {
        [Option('p', Required = false, HelpText = "匯出的xml路徑")]
        public string BackupXmlPath { get; set; }

        //[Option('o', Required = false, HelpText = "已經下載的圖片位置")]
        //public string OffLineImagePath { get; internal set; }
    }
}

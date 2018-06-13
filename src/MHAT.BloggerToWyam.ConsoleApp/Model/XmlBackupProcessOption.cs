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
        [Option('p', HelpText = "匯出的xml路徑", DefaultValue = @"d:\Library\Downloads\blog-03-22-2018.xml")]
        public string BackupXmlPath { get; set; }

        [Option('i', HelpText = "已經下載的圖片位置", DefaultValue = @"d:\Library\Downloads\blog\image\")]
        public string OffLineImagePath { get; set; }

        [Option('o', HelpText = "最後輸出的位置", DefaultValue = @"d:\Library\Downloads\blog\post\")]
        public string FinalOutputPostPath { get; set; }
    }
}

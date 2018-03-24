using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace MHAT.BloggerToWyam.ConsoleApp.Model
{
    class XmlBackupProcessOption
    {
        [Option('p', Required = true, HelpText = "匯出的xml路徑")]
        public string BackupXmlPath { get; set; }
    }
}

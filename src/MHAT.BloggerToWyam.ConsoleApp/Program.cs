using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MHAT.BloggerToWyam.ConsoleApp.Bll.Process;

namespace MHAT.BloggerToWyam.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new XmlBackupProcess();

            process.StartProcess(args);
        }
    }
}

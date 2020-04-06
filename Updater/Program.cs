using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            Logic _logic = new Logic();

            _logic.ParsePathFromXML();
            _logic.CloseApp("LXM-DS");
            System.Threading.Thread.Sleep(2000);
            _logic.ExtractZip(args[0]);
            _logic.RunApp(_logic._path + @"bin\Debug\LXM-DS.exe");
        }
    }
}

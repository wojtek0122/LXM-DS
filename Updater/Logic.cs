using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Updater
{
    class Logic
    {
        public string _path;

        public void ExtractZip(string File)
        {
            Console.WriteLine("Extract ZIP - " + File);

            //C:\Users\Dell>"C:\Program Files (x86)\7-Zip\7z.exe" x C:\LXM-DS\UPDATES\LXM-DS_2.zip -oC:\LXM-DS\ -aoa

            System.Diagnostics.Process _unzip = new System.Diagnostics.Process();
            _unzip.StartInfo.FileName = @"C:\Program Files (x86)\7-Zip\7z.exe";
            _unzip.StartInfo.Arguments = "x " + _path + @"UPDATES\" + File + " -o" + _path + " -aoa";
            _unzip.Start();
        }

        public void RunApp(string PathToFile)
        {
            try
            {
                System.Diagnostics.Process.Start(PathToFile);
                Console.WriteLine("Open App - " + PathToFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open App ERROR - " + ex.ToString());
            }
        }

        public void CloseApp(string ProcessName)
        {
            try
            {
                foreach (var process in System.Diagnostics.Process.GetProcessesByName(ProcessName))
                {
                    process.Kill();
                }
                Console.WriteLine("Close App - " + ProcessName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Close App ERROR - " + ex.ToString());
            }
        }

        public void ParsePathFromXML()
        {
            string _parsedPath = "";
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\Config.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "CONFIG"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _parsedPath = String.Format(_xmlReader.GetAttribute("PATH"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            _path = _parsedPath;
        }
    }
}

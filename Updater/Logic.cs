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

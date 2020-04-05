using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.AUTOUPDATE
{
    class AutoUpdate
    {
        private static AutoUpdate _autoUpdate = null;
        private AppVersion _appCurrentVersion = null;
        private int _ver = 0;
        private string _path = null;

        MYSQL.MySQLManager _mysqlManager;

        private AutoUpdate()
        {
            _mysqlManager = MYSQL.MySQLManager.CreateManager();
            _ver = ParseVersionFromXML();
            _path = ParsePathFromXML();
        }

        public static AutoUpdate CreateAutoUpdate()
        {
            if (_autoUpdate == null)
            {
                _autoUpdate = new AutoUpdate();
            }
            return _autoUpdate;
        }

        private void UpdateApp(string File)
        {
            //Delete old versions
            DeleteOldZip();

            //Copy .zip file
            CopyZipFileFromServer(@"C:\LXM-DS_UPDATES\" + File + ".zip", _path + @"\UPDATES\" + File + ".zip");

            //Close App
            CloseApp("LXM-DS");

            //Start .bat
            StartBAT(File);
            //Unzip files
            //Unzip(@"C:\LXM-DS\" + File + ".zip");

            //Start App
            //OpenApp(@"C:\LXM-DS\bin\Debug\LXM-DS.exe");
        }

        private void StartBAT(string File)
        {
            string _p1 = _path + @"UPDATES\" + File + ".zip "; //Zip file
            //string _p2 = @"C:\Program Files (x86)\7-Zip\7z.exe "; //7zip extract
            string _p2 = @"C:\Program Files (x86)\7-Zip\7z.exe e " + _p1 + @" -o" + _path + " -aoa";

            string _p3 = _path + @"bin\Debug\LXM-DS.exe"; //Open app

            OpenApp("Update.bat " + _p1 + _p2 + _p3);
        }

        public void DeleteOldZip()
        {
            if (System.IO.Directory.Exists(_path + @"UPDATES"))
            {
                try
                {
                    System.IO.Directory.Delete(_path + @"UPDATES", true);
                    System.IO.Directory.CreateDirectory(_path + @"UPDATES");
                }

                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void CloseApp(string ProcessName)
        {
            try
            {
                foreach (var process in System.Diagnostics.Process.GetProcessesByName(ProcessName))
                {
                    process.Kill();
                }
                LOG("Close App - " + ProcessName);
            }
            catch (Exception ex)
            {
                LOG("Close App ERROR - " + ex.ToString());
            }
        }

        private void OpenApp(string PathToFile)
        {
            try
            {
                System.Diagnostics.Process.Start(PathToFile);
                LOG("Open App - " + PathToFile);
            }
            catch (Exception ex)
            {
                LOG("Open App ERROR - " + ex.ToString());
            }
        }

        private void CopyZipFileFromServer(string Source, string Destination)
        {
            try
            {
                System.IO.File.Copy(Source, Destination, true);
                LOG("Copy ZIP file from " + Source + " to " + Destination);
            }
            catch (Exception ex)
            {
                LOG("Copy ZIP file ERROR - " + ex.ToString());
            }
        }

        private void GetCurrentVersionFromMySQL()
        {
            _appCurrentVersion = _mysqlManager.GetCurrentVersion();
        }

        public void CheckUpdate()
        {
            GetCurrentVersionFromMySQL();

            if(_appCurrentVersion._Version > _ver)
            {
                UpdateApp(_appCurrentVersion._Name);
                _ver = _appCurrentVersion._Version;
            }
        }

        public int GetCurrentVersion()
        {
            return _ver;
        }

        private void Unzip(string Path)
        {
            /*
            try
            {
                ZipArchive oZip = new ZipArchive("TryIt");
                ZipArchive _zip = new ZipArchive();
                _zip.LOAD
                oZip.Load("c:\\test.zip");

                ZipFile[] zs = oZip.Files;
                int count = zs.Length;
                string password = "";
                bool overwrite = true;
                for (int i = 0; i < count; i++)
                {
                    ZipFile z = zs[i];
                    if (!z.IsDirectory)
                        oZip.ExtractTo(z, password, "c:\\unzipped", overwrite);
                }
            }
            catch (Exception ep)
            {
                Console.Write(ep.Message);
            }
            */
        }

        private int ParseVersionFromXML()
        {
            int _parsedVersion = 0;
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\Version.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "VERSION"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            string _parsedInfo = String.Format(_xmlReader.GetAttribute("CURRENT"));
                            Int32.TryParse(_parsedInfo, out _parsedVersion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _parsedVersion;
        }

        private string ParsePathFromXML()
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
            return _parsedPath;
        }

        private void LOG(string Message)
        {
            System.Console.WriteLine("LOG " + DateTime.Now.ToString() + " :: " + Message);
        }
    }
}

using LXM_DS.BUTTON;
using LXM_DS.MYSQL;
using LXM_DS.PRINTER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for ComponentViewFullHD.xaml
    /// </summary>
    public partial class ComponentViewFullHD : Window
    {
        //Managers
        Managers _managers;
        MySQLManager _mysqlManager;
        ButtonListManager _buttonListManager;
        int _testid;
        string _login;
        string _pn;
        int _id;
        string _mt;
        Component _component;
        DispatcherTimer _timer;
        int _count;

        public ComponentViewFullHD(string Login, int TestID, string PN, int ID, string MT)
        {
            //this.Topmost = true;
            InitializeComponent();
            
            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _buttonListManager = _managers.GetButtonListManager();
            _login = Login;
            _testid = TestID;
            _pn = PN;
            _id = ID;
            _mt = MT;
            _component = _mysqlManager.GetComponentByPN(PN.Remove(0,2));
            lblMT.Content = "Komponent: " + _component._PN;
            lblComment.Content = "Komentarz: " + _component._comment;

            int _revInt = 0;
            int.TryParse(_component._REV, out _revInt);

            string _parsedShareFIDPath = ParseSharedFIDPathFromXML();
            if(String.IsNullOrEmpty(_parsedShareFIDPath) || String.IsNullOrWhiteSpace(_parsedShareFIDPath))
            {
                string _path = ParseFIDPathFromXML();
                if (_revInt > 9)
                {
                    _browser.Navigate(_path + @"FID\" + _component._FID + "." + _component._REV + " " + _component._PN + ".pdf");
                }
                else
                {
                    _browser.Navigate(_path + @"FID\" + _component._FID + ".0" + _component._REV + " " + _component._PN + ".pdf");
                }
            }
            else
            {
                if (_revInt > 9)
                {
                    _browser.Navigate(_parsedShareFIDPath + _component._FID + "." + _component._REV + " " + _component._PN + ".pdf");
                }
                else
                {
                    _browser.Navigate(_parsedShareFIDPath + _component._FID + ".0" + _component._REV + " " + _component._PN + ".pdf");
                }
            }

            this.OK.Visibility = Visibility.Visible;
            this.NOK.Visibility = Visibility.Visible;
            this.NONE.Visibility = Visibility.Visible;
            this.BACK.Visibility = Visibility.Visible;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _count++;
            if (_count == 2)
            {
                _timer.Stop();
                switch (_component._type)
                {
                    case COMPONENTTYPE.MB: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Topmost = true; _snWindow.Show(); _snWindow.txtSN.Focus(); break; };
                    case COMPONENTTYPE.OP: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Topmost = true; _snWindow.Show(); _snWindow.txtSN.Focus(); break; };
                    case COMPONENTTYPE.ENG: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Topmost = true; _snWindow.Show(); _snWindow.txtSN.Focus(); break; };
                    default:
                        break;
                }
            }
        }

        private void CloseAdobeReaderProcess()
        {
            foreach (var process in System.Diagnostics.Process.GetProcessesByName("AcroRd32"))
            {
                process.Kill();
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Print label
                BARCODE.Barcode _barcode = new BARCODE.Barcode();
                _barcode.PrintLabel(_component._PN, _component._location, _login, "", _mt);
                /*
                if (_mysqlManager.GetTestStatusFromTestByTestID(_testid) == "OK")
                {
                    _barcode.PrintLabel(_component._PN, _component._location, _login, "");
                }
                else
                {
                    _barcode.PrintLabel(_component._PN, _component._location, _login, "DEF PRT");
                }
                */

                //UpdateStock
                _mysqlManager.UpdateComponentStock(_component._id);

                //Add log
                _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "OK");

                StatusButton _btn;
                foreach (var value in _buttonListManager.GetButtonList())
                {
                    _btn = value.Item1;
                    if (_btn.ID == _id)
                    {
                        _btn.STATUS = "OK";
                    }
                }

                CloseAdobeReaderProcess();
                _browser.Dispose();
                //Close window
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void NOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(ParseSCRLABELFromXML() == "YES")
                {
                    //Print label - SCR
                    BARCODE.Barcode _barcode = new BARCODE.Barcode();
                    _barcode.PrintLabel(_component._PN, "SCR", "", "", _mt);
                }

                //Add log
                _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "NOK");

                StatusButton _btn;
                foreach (var value in _buttonListManager.GetButtonList())
                {
                    _btn = value.Item1;
                    if (_btn.ID == _id)
                    {
                        _btn.STATUS = "NOK";
                    }
                }

                CloseAdobeReaderProcess();
                _browser.Dispose();
                //Close window
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void NONE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Add log
                _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "NONE");

                StatusButton _btn;
                foreach (var value in _buttonListManager.GetButtonList())
                {
                    _btn = value.Item1;
                    if (_btn.ID == _id)
                    {
                        _btn.STATUS = "NONE";
                    }
                }

                CloseAdobeReaderProcess();
                _browser.Dispose();
                //Close window
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void BACK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseAdobeReaderProcess();
                _browser.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public string ParseFIDPathFromXML()
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

        public string ParseSCRLABELFromXML()
        {
            string _value = "";
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\Config.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "CONFIG"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _value = String.Format(_xmlReader.GetAttribute("SCRLABEL"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _value;
        }

        public string ParseSharedFIDPathFromXML()
        {
            string _value = "";
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\Config.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "CONFIG"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _value = String.Format(_xmlReader.GetAttribute("FIDPATH"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _value;
        }
    }
}

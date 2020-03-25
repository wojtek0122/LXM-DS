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
        Component _component;
        DispatcherTimer _timer;
        int _count;

        public ComponentViewFullHD(string Login, int TestID, string PN, int ID)
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
            _component = _mysqlManager.GetComponentByPN(PN.Remove(0,2));
            lblMT.Content = _component._PN;

            string _path = ParseFIDPathFromXML();
            int _revInt = 0;
            int.TryParse(_component._REV, out _revInt);
            if (_revInt > 9)
            {
                _browser.Navigate(_path + @"FID\" + _component._FID + "." + _component._REV + " " + _component._PN + ".pdf");
            }
            else
            {
                _browser.Navigate(_path + @"FID\" + _component._FID + ".0" + _component._REV + " " + _component._PN + ".pdf");
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _count++;
            if (_count == 1)
            {
                switch (_component._type)
                {
                    case COMPONENTTYPE.MB: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Topmost = true; _snWindow.Show(); break; };
                    case COMPONENTTYPE.OP: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Topmost = true; _snWindow.Show(); break; };
                    case COMPONENTTYPE.ENG: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Topmost = true; _snWindow.Show(); break; };
                    default:
                        break;
                }
                _timer.Stop();
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
            //Print label
            BARCODE.Barcode _barcode = new BARCODE.Barcode();
            _barcode.PrintLabel(_component._PN, _component._location, _login);

            //UpdateStock
            _mysqlManager.UpdateComponentStock(_component._id);

            //Add log
            _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "OK");

            StatusButton _btn;
            foreach (var value in _buttonListManager.GetButtonList())
            {
                _btn = value.Item1;
                if(_btn.ID == _id)
                {
                    _btn.STATUS = "OK";
                }
            }

            CloseAdobeReaderProcess();
            //Close window
            this.Close();
        }

        private void NOK_Click(object sender, RoutedEventArgs e)
        {
            //Print label - SCR
            BARCODE.Barcode _barcode = new BARCODE.Barcode();
            _barcode.PrintLabel(_component._PN, "SCR", "");

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
            //Close window
            this.Close();
        }

        private void NONE_Click(object sender, RoutedEventArgs e)
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
            //Close window
            this.Close();
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

        private void BACK_Click(object sender, RoutedEventArgs e)
        {
            CloseAdobeReaderProcess();
            this.Close();
        }
    }
}

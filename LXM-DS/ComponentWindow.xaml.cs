using LXM_DS.MYSQL;
using LXM_DS.PRINTER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for ComponentWindow.xaml
    /// </summary>
    public partial class ComponentWindow : Window
    {
        //Managers
        Managers _managers;
        MySQLManager _mysqlManager;
        PrinterManager _printerManager;
        
        Printer _printer;

        int _testid;
        string _mt;
        string _status;
        string _login;

        List<Component> _dismantledComponentsList;
        Component _component;

        public ComponentWindow(int TestID, string MT, string Status, string Login)
        {
            InitializeComponent();
            //this.Topmost = true;

            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _printerManager = _managers.GetPrinterManager();

            _testid = TestID;
            _mt = MT;
            _status = Status;
            _login = Login;

            _printer = _printerManager.GetPrinterByMT(_mt);
            _dismantledComponentsList = new List<Component>();

            InitializePrinterComponentsToListView(_status);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //Print label
            BARCODE.Barcode _barcode = new BARCODE.Barcode();
            if(_mysqlManager.GetTestStatusFromTestByTestID(_testid)=="OK")
            {
                _barcode.PrintLabel(_component._PN, _component._location, _login, "", "");
            }
            else
            {
                _barcode.PrintLabel(_component._PN, _component._location, _login, "DEF PRT", "");
            }
            

            //DeactivateField
            _dismantledComponentsList.Remove(_component);
            ListViewAddSource();

            //UpdateStock
            _mysqlManager.UpdateComponentStock(_component._id);

            //Add log
            _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "OK");

            //Close window
            if (_dismantledComponentsList.Count == 0)
            {
                this.Close();
            }
        }

        private void NOK_Click(object sender, RoutedEventArgs e)
        {
            //Print label - SCR
            BARCODE.Barcode _barcode = new BARCODE.Barcode();
            _barcode.PrintLabel(_component._PN, "SCR", "", "", "");

            //DeactivateField
            _dismantledComponentsList.Remove(_component);
            ListViewAddSource();

            //Add log
            _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "NOK");

            //Close window
            if (_dismantledComponentsList.Count == 0)
            {
                this.Close();
            }
        }

        private void NONE_Click(object sender, RoutedEventArgs e)
        {
            //DeactivateField
            _dismantledComponentsList.Remove(_component);
            ListViewAddSource();

            //Add log
            _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "NONE");

            //Close window
            if (_dismantledComponentsList.Count == 0)
            {
                this.Close();
            }
        }

        public void InitializePrinterComponentsToListView(string Status)
        {
            foreach (var value in _printer.GetComponentList())
            {
                _dismantledComponentsList.Add(value);
            }
            /*
            if(Status == "OK")
            {
                foreach (var value in _printer.GetComponentList())
                {
                    _dismantledComponentsList.Add(value);
                }
            }
            else
            {
                foreach (var value in _printer.GetComponentList())
                {
                    if(value._type == "E")
                    {
                        break;
                    }
                    else
                    {
                        _dismantledComponentsList.Add(value);
                    }
                }
            }
            */
            ListViewAddSource();
        }

        public void ListViewAddSource()
        {
            this.lview.ItemsSource = _dismantledComponentsList;
            this.lview.Items.Refresh();
        }

        private void lview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lview.Items.Count>0)
            {
                try
                {
                    string _text = lview.SelectedItem.ToString();
                    string _pn = _text.Substring(0, _text.IndexOf("\n"));
                    string _fid = "";
                    string _rev = "";
                    foreach (var value in _printer._componentsList)
                    {
                        if (_pn == value._PN)
                        {
                            _fid = value._FID;
                            _rev = value._REV;
                            _component = value;
                            break;
                        }
                    }

                    switch (_component._type)
                    {
                        case COMPONENTTYPE.MB: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Show(); break; };
                        case COMPONENTTYPE.OP: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Show(); break; };
                        case COMPONENTTYPE.ENG: { SNWindow _snWindow = new SNWindow(_component._type, _testid); _snWindow.Show(); break; };
                        default:
                            break;
                    }
                    string _path = ParseFIDPathFromXML();
                    int _revInt = 0;
                    int.TryParse(_rev, out _revInt);
                    if(_revInt>9)
                    {
                        _browser.Navigate(_path + @"FID\" + _fid + "." + _rev + " " + _pn + ".pdf");
                    }
                    else
                    {
                        _browser.Navigate(_path + @"FID\" + _fid + ".0" + _rev + " " + _pn + ".pdf");
                    }

                }
                catch (Exception)
                {

                }

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
    }
}

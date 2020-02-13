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

        int _id;
        string _mt;
        string _status;

        List<Component> _dismantledComponentsList;

        public ComponentWindow(int ID, string MT, string Status)
        {
            InitializeComponent();
            //this.Topmost = true;

            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _printerManager = _managers.GetPrinterManager();

            _id = ID;
            _mt = MT;
            _status = Status;

            _printer = _printerManager.GetPrinterByMT(_mt);
            _dismantledComponentsList = new List<Component>();

            InitializePrinterComponentsToListView(_status);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //Print label
            //DeactivateField
            //UpdateStock
            //AddLog
            BARCODE.Barcode _barcode = new BARCODE.Barcode();
            _barcode.PrintLabel("40X1234", "A2");

            //this.Close();
        }

        private void NOK_Click(object sender, RoutedEventArgs e)
        {
            //DeactivateField
            //AddLog
            this.Close();
        }

        public void InitializePrinterComponentsToListView(string Status)
        {
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
            ListViewAddSource();
        }

        public void ListViewAddSource()
        {
            this.lview.ItemsSource = _dismantledComponentsList;
        }

        private void lview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string _text = lview.SelectedItem.ToString();
            string _pn = _text.Substring(0, _text.IndexOf("\n"));
            string _fid = "";
            string _rev = "";
            foreach (var value in _printer._componentsList)
            {
                if(_pn == value._PN)
                {
                    _fid = value._FID;
                    _rev = value._REV;
                    break;
                }
            }

            _browser.Navigate(@"C:\LXM-DS\FID\" + _fid + "." + _rev + " " + _pn + ".pdf");
        }
    }
}

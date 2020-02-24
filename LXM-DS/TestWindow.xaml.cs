using LXM_DS.MYSQL;
using LXM_DS.PRINTER;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private struct printer
        {
            public string mt;
            public string sn;
            public string status;
            public bool hdd;
            public string hddsn;
            public bool firmware;
            public bool defaults;
            public bool nvram;
        }

        printer _printer;
        string _login;

        Managers _managers;
        MySQLManager _mysqlManager;

        public TestWindow(string User)
        {
            InitializeComponent();
            //this.Topmost = true;
            _printer = new printer();

            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();

            _login = User;
        }

        private void btnNOK_Click(object sender, RoutedEventArgs e)
        {
            _printer.status = "NOK";
            InsertHDDToMySQL();
            InsertDatatoMySQL();
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            _printer.status = "OK";
            InsertHDDToMySQL();
            InsertDatatoMySQL();
            this.Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtSN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(this.txtSN.Text.Length == 14)
            {
                ParseTextBoxContent(this.txtSN.Text.ToString());
            }
        }

        private void ParseTextBoxContent(string Content)
        {
            _printer.mt = Content.Substring(1, 4);
            this.txtMTlbl.Text = _printer.mt;
            _printer.sn = Content.Substring(5, 9);
            this.txtSNlbl.Text = _printer.sn;
            ChangePrinterFoto();
          
        }

        private void ChangePrinterFoto()
        {
            if (File.Exists(@"..\..\FILES\" + _printer.mt.ToString() + ".png"))
            {
                this.imgFoto.Source = new ImageSourceConverter().ConvertFromString(@"..\..\FILES\" + _printer.mt.ToString() + ".png") as ImageSource;
            }
            else
            {
                this.imgFoto.Source = new ImageSourceConverter().ConvertFromString(@"..\..\FILES\lexmark.png") as ImageSource;
            }
        }

        private void txtHDDSN_TextChanged(object sender, TextChangedEventArgs e)
        {
            _printer.hddsn = this.txtHDDSN.Text.ToString();
        }

        private void tgbtnHDD_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnHDD.IsChecked == true)
            {
                this.lblHDD.Visibility = Visibility.Visible;
                this.txtHDDSN.Visibility = Visibility.Visible;
                _printer.hdd = true;
            }
            else
            {
                this.lblHDD.Visibility = Visibility.Hidden;
                this.txtHDDSN.Visibility = Visibility.Hidden;
                _printer.hdd = false;
            }
        }

        private void tgbtnfirmware_Click(object sender, RoutedEventArgs e)
        {
            if(this.tgbtnfirmware.IsChecked == true)
            {
                _printer.firmware = true;
            }
            else
            {
                _printer.firmware = false;
            }
            ChangeVisibilityOK();
        }

        private void tgbtndefaults_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtndefaults.IsChecked == true)
            {
                _printer.defaults = true;
            }
            else
            {
                _printer.defaults = false;
            }
            ChangeVisibilityOK();
        }

        private void tgbtnnvram_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnnvram.IsChecked == true)
            {
                _printer.nvram = true;
            }
            else
            {
                _printer.nvram = false;
            }
            ChangeVisibilityOK();
        }

        private void ChangeVisibilityOK()
        {
            if(_printer.firmware == true && _printer.nvram == true && _printer.defaults == true)
            {
                this.btnOK.Visibility = Visibility.Visible;
            }
            else
            {
                this.btnOK.Visibility = Visibility.Hidden;
            }
        }

        private void InsertHDDToMySQL()
        {
            if (this.tgbtnHDD.IsChecked == true)
            {
                _mysqlManager.InsertHDD(this.txtHDDSN.Text, this.txtMTlbl.Text, this.txtSNlbl.Text);
            }
        }

        public void InsertDatatoMySQL()
        {
            _mysqlManager.InsertTestDataToMySQL(_printer.mt, _printer.sn, _printer.status, _login, _printer.firmware, _printer.defaults, _printer.nvram);
        }
    }
}
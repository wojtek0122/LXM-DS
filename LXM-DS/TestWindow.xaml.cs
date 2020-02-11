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
        }

        printer _printer;
        string _user;

        Managers _managers;
        MySQLManager _mysqlManager;

        public TestWindow(string User)
        {
            InitializeComponent();
            this.Topmost = true;
            _printer = new printer();

            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();

            _user = User;

        }

        private void btnNOK_Click(object sender, RoutedEventArgs e)
        {
            _printer.status = "NOK";
            SendDataToMySQL();
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            _printer.status = "OK";
            SendDataToMySQL();
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
            //ZMIENIC SCIEZKE!!!
            this.imgFoto.Source = new ImageSourceConverter().ConvertFromString(@"C:\Users\TGCS\source\repos\LXM-DS\LXM-DS\FILES\" + _printer.mt.ToString() + ".png") as ImageSource;
        }

        private void bntTAK_Click(object sender, RoutedEventArgs e)
        {
            this.lblHDD.Visibility = Visibility.Visible;
            this.txtHDDSN.Visibility = Visibility.Visible;
            _printer.hdd = true;
        }

        private void btnNIE_Click(object sender, RoutedEventArgs e)
        {
            this.lblHDD.Visibility = Visibility.Hidden;
            this.txtHDDSN.Visibility = Visibility.Hidden;
            _printer.hdd = false;
        }

        private void txtHDDSN_TextChanged(object sender, TextChangedEventArgs e)
        {
            _printer.hddsn = this.txtHDDSN.Text.ToString();
        }

        public void SendDataToMySQL()
        {
            _mysqlManager.InsertTestData(_printer.mt, _printer.sn, _printer.hdd, _printer.hddsn, _printer.status, _user);
        }
    }
}
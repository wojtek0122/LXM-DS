using LXM_DS.BUTTON;
using LXM_DS.MYSQL;
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
    /// Interaction logic for ChoosePrinterWindowFullHD.xaml
    /// </summary>
    public partial class ChoosePrinterWindowFullHD : Window
    {
        private string _printerMT;
        private string _printerSN;
        private int _testid;
        Managers _managers;
        MySQLManager _mysqlManager;
        private string _login;
        ButtonListManager _buttonListManager;

        public ChoosePrinterWindowFullHD(String Login)
        {
            InitializeComponent();
            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _buttonListManager = _managers.GetButtonListManager();
            _login = Login;
            txtLabel.Focus();
        }

        private void btnWybierz_Click(object sender, RoutedEventArgs e)
        {
            _testid = _mysqlManager.GetTestIDFromTest(_printerSN);
            _buttonListManager.ClearList();
            if (_testid == 0)
            {
                this.lblInfo.Content = "BLAD: Drukarka nie zostala jeszcze przetestowana!";
            }
            else
            {
                _mysqlManager.SetDismantled(_testid);
                ComponentWindowThumbnailsFullHD _componentWindowThumbnailsFullHD = new ComponentWindowThumbnailsFullHD(_testid, _mysqlManager.GetMTFromPrintersWherePrinterID(_mysqlManager.GetPrinterIDFromTest(_printerSN)), _mysqlManager.GetStatusFromTest(_printerSN), _login);
                this.Close();
                _componentWindowThumbnailsFullHD.Show();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtLabel.Text.Length == 14 || this.txtLabel.Text.Length == 12)
            {
                ParseTextBoxContent(this.txtLabel.Text.ToString());
            }
        }

        private void ParseTextBoxContent(string Content)
        {
            _printerMT = Content.Substring(1, 4);
            this.txtLabel.Text = _printerMT;
            if (_printerMT == "7014")
            {
                _printerSN = Content.Substring(5, 7);
            }
            else
            {
                _printerSN = Content.Substring(5, 9);
            }
            this.txtLabel.Text = _printerSN;
            ChangePrinterFoto();
        }

        private void ChangePrinterFoto()
        {
            if (File.Exists(@"..\..\FILES\" + _printerMT.ToString() + ".png"))
            {
                this.imgFoto.Source = new ImageSourceConverter().ConvertFromString(@"..\..\FILES\" + _printerMT.ToString() + ".png") as ImageSource;
            }
            else
            {
                this.imgFoto.Source = new ImageSourceConverter().ConvertFromString(@"..\..\FILES\lexmark.png") as ImageSource;
            }
        }
    }
}

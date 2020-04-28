using LXM_DS.BUTTON;
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
    /// Interaction logic for TestWindowFullHD.xaml
    /// </summary>
    public partial class TestWindowFullHD : Window
    {
        private struct printer
        {
            public string mt;
            public int sub;
            public int ID;
            public string sn;
            public string status;
            public bool hdd;
            public string hddsn;
            public bool firmware;
            public bool defaults;
            public bool nvram;
        }

        private struct STestComp
        {
            public bool F;
            public bool H;
            public bool PSU;
            public bool S;
            public bool A;
            public bool OP;
            public bool MB;
            public bool ENG;
        }

        printer _printer;
        STestComp _testComp;
        string _login;

        Managers _managers;
        MySQLManager _mysqlManager;
        AUTOUPDATE.AutoUpdate _autoUpdate;
        List<StatusButton> _btnList;
        List<SSubmodel> _listSubmodel;

        public TestWindowFullHD(string User)
        {
            InitializeComponent();
            //this.Topmost = true;
            _printer = new printer();

            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _autoUpdate = AUTOUPDATE.AutoUpdate.CreateAutoUpdate();

            _login = User;
            txtSN.Focus();
        }

        private void btnNOK_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSN.Text))
            {
                _printer.status = "NOK";
                InsertHDDToMySQL();
                InsertDatatoMySQL();
                CloseWindow();
            }

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSN.Text))
            {
                _printer.status = "OK";
                InsertHDDToMySQL();
                InsertDatatoMySQL();
                CloseWindow();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            _autoUpdate.CheckUpdate();
            this.Close();
        }

        private void txtSN_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.txtSN.Text.Length == 14 || this.txtSN.Text.Length == 12)
                {
                    ParseTextBoxContent(this.txtSN.Text.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private void ParseTextBoxContent(string Content)
        {
            _printer.mt = Content.Substring(1, 4);
            this.txtMTlbl.Text = _printer.mt;
            if (_printer.mt == "7014")
            {
                _printer.sn = Content.Substring(5, 7);
            }
            else
            {
                _printer.sn = Content.Substring(5, 9);
            }
            this.txtSNlbl.Text = _printer.sn;
            ChangePrinterFoto();
            CheckSubModel(_printer.mt);
        }

        private void CheckSubModel(string MachineType)
        {
            _listSubmodel = new List<SSubmodel>();

            _listSubmodel = _mysqlManager.GetPrinterIDFromPrintersByModel(MachineType);

            if (_listSubmodel.Count != 0)
            {
                if (_listSubmodel.Count > 1)
                {
                    for (int i = 0; i < _listSubmodel.Count; i++)
                    {
                        _btnList = new List<StatusButton>();
                        _btnList.Add(CreateNewButton(_listSubmodel[i].GetPrintersID(), _listSubmodel[i].GetSubModel(), _listSubmodel[i].GetName(), i + 1));
                    }
                    gboxMain.Visibility = Visibility.Hidden;
                    gboxSubmodel.Visibility = Visibility.Visible;
                }
                else
                {
                    _printer.ID = _listSubmodel[0].GetPrintersID();
                    _printer.sub = _listSubmodel[0].GetSubModel();
                    this.txtSUBlbl.Text = _printer.sub.ToString();
                }
                this.txtSUBlbl.Text = _printer.sub.ToString();
            }
            else
            {
                txtSN.Text = "BŁĄD - Nie ma takiego MT!";
                btnNOK.IsEnabled = false;
                btnOK.IsEnabled = false;
            }
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
            if (this.tgbtnfirmware.IsChecked == true)
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
            if (_printer.firmware == true && _printer.nvram == true && _printer.defaults == true)
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
            _mysqlManager.InsertTestDataToMySQL(_printer.mt, _printer.sn, _printer.status, _login, _printer.firmware, _printer.defaults, _printer.nvram, _testComp.F, _testComp.H, _testComp.PSU, _testComp.S, _testComp.A, _testComp.OP, _testComp.MB, _testComp.ENG);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            this.gboxMain.Visibility = Visibility.Hidden;
            this.gboxComponents.Visibility = Visibility.Visible;
        }
        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            this.gboxMain.Visibility = Visibility.Visible;
            this.gboxComponents.Visibility = Visibility.Hidden;
        }

        private void tgbtnFuser_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnFuser.IsChecked == true)
            {
                _testComp.F = true;
            }
            else
            {
                _testComp.F = false;
            }
        }

        private void tgbtnGlowica_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnGlowica.IsChecked == true)
            {
                _testComp.H = true;
            }
            else
            {
                _testComp.H = false;
            }
        }

        private void tgbtnZasilacz_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnZasilacz.IsChecked == true)
            {
                _testComp.PSU = true;
            }
            else
            {
                _testComp.PSU = false;
            }
        }

        private void tgbtnSkaner_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnSkaner.IsChecked == true)
            {
                _testComp.S = true;
            }
            else
            {
                _testComp.S = false;
            }
        }

        private void tgbtnAdf_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnAdf.IsChecked == true)
            {
                _testComp.A = true;
            }
            else
            {
                _testComp.A = false;
            }
        }

        private void tgbtnOperatorPanel_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnOperatorPanel.IsChecked == true)
            {
                _testComp.OP = true;
            }
            else
            {
                _testComp.OP = false;
            }
        }

        private void tgbtnPlytaGlowna_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnPlytaGlowna.IsChecked == true)
            {
                _testComp.MB = true;
            }
            else
            {
                _testComp.MB = false;
            }
        }

        private void tgbtnEngine_Click(object sender, RoutedEventArgs e)
        {
            if (this.tgbtnEngine.IsChecked == true)
            {
                _testComp.ENG = true;
            }
            else
            {
                _testComp.ENG = false;
            }
        }

        private void btnKeyboard_Click(object sender, RoutedEventArgs e)
        {
            string KeyboardPath = @"C:\Program Files\Common Files\Microsoft Shared\Ink\TabTip.exe";
            System.Diagnostics.Process.Start(KeyboardPath);
        }

        public StatusButton CreateNewButton(int PrintersID, int Submodel, string Name, int Row)
        {
            var _converter = new System.Windows.Media.BrushConverter();
            var _brushFore = (Brush)_converter.ConvertFromString("#FFFFFF");
            var _brushBack = (Brush)_converter.ConvertFromString("#b9192c");
            StatusButton _btn = new StatusButton()
            {
                Name = "SUB" + Submodel.ToString(),
                Content = Submodel.ToString() + " - " + Name,
                Height = 80,
                Width = 450,
                FontFamily = new FontFamily("Oxygen-Bold"),
                FontSize = 30,
                Foreground = _brushFore,
                Background = _brushBack,
                Visibility = Visibility.Visible,
            };
            _btn.Click += new RoutedEventHandler(_btn_Click);
            Grid.SetColumn(_btn, 0);
            Grid.SetRow(_btn, Row);

            grdSubmodel.Children.Add(_btn);
            _btn.ID = PrintersID;
            return _btn;
        }

        private void _btn_Click(object sender, RoutedEventArgs e)
        {
            StatusButton _button = sender as StatusButton;
            gboxSubmodel.Visibility = Visibility.Hidden;
            gboxMain.Visibility = Visibility.Visible;

            _printer.ID = _button.ID;
            this.txtSUBlbl.Text = _button.Name.Substring(3);
            int.TryParse(this.txtSUBlbl.Text, out _printer.sub);

            _btnList = null;
        }
    }
}

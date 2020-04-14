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
    /// Interaction logic for MainWindowFullHD.xaml
    /// </summary>
    public partial class MainWindowFullHD : Window
    {
        Managers _managers;
        MYSQL.MySQLManager _mysqlManager;
        AUTOUPDATE.AutoUpdate _autoUpdate;

        private string _login;
        DispatcherTimer _timer;

        public MainWindowFullHD(string Login, int Permission)
        {
            InitializeComponent();
            _managers = Managers.CreateManagers();
            _managers.InitializePrinters();
            _mysqlManager = MYSQL.MySQLManager.CreateManager();
            _autoUpdate = AUTOUPDATE.AutoUpdate.CreateAutoUpdate();

            lblName.Content = _login = Login;
            if (Permission == 9)
            {
                gboxDismantle.Visibility = System.Windows.Visibility.Visible;
            }

            lblVersion.Content = "version: " + _autoUpdate.GetCurrentVersion().ToString();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblName.Content = _login + " " + DateTime.Now.ToString("HH:mm:ss");
            if (_login != "admin")
            {
                if ((DateTime.Now.Hour == 16) && (DateTime.Now.Minute == 1))
                {
                    _timer.Stop();
                    LoginWindow _loginWindow = new LoginWindow();
                    _loginWindow.Topmost = true;
                    _loginWindow.Show();
                    this.Close();
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            TestWindowFullHD _testWindowFullHD = new TestWindowFullHD(_login);
            _testWindowFullHD.Topmost = true;
            _testWindowFullHD.Show();
        }

        private void btnDismantle_Click(object sender, RoutedEventArgs e)
        {
            ChoosePrinterWindowFullHD _choosePrinterWindowFullHD = new ChoosePrinterWindowFullHD(_login);
            //_choosePrinterWindowFullHD.Topmost = true;
            _choosePrinterWindowFullHD.Show();
        }

        private void btnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow _loginWindow = new LoginWindow();
            _loginWindow.Topmost = true;
            _loginWindow.Show();
            this.Close();
        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            NewOrderWindow _newOrderWindow = new NewOrderWindow();
            _newOrderWindow.Topmost = true;
            _newOrderWindow.Show();
        }

        private void btnSupermarket_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

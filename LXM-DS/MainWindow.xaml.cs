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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string Login, int Permission)
        {
            InitializeComponent();
            Managers _managers = Managers.CreateManagers();
            _managers.InitializePrinters();
            
            lblName.Content = Login;
            if(Permission == 9)
            {
                gboxDismantle.Visibility = System.Windows.Visibility.Visible;
            } 
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            TestWindow _testWindow = new TestWindow();
            _testWindow.Topmost = true;
            _testWindow.Show();
        }

        private void btnDismantle_Click(object sender, RoutedEventArgs e)
        {
            ChoosePrinterWindow _choosePrinter = new ChoosePrinterWindow();
            _choosePrinter.Topmost = true;
            _choosePrinter.Show();
        }

        private void btnWyloguj_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow _loginWindow = new LoginWindow();
            _loginWindow.Topmost = true;
            _loginWindow.Show();
            this.Close();
        }
    }
}

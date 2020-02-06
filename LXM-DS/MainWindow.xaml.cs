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
using LXM_DS.PRINTER;
using LXM_DS.USERS;

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(int Permission)
        {
            InitializeComponent();
            //Managers


            //Windows
            //LoginWindow _loginWindow = new LoginWindow();
            //_loginWindow.Topmost = true;
            //_loginWindow.Show();


            //Console.WriteLine();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         //   ComponentWindow _componentWindow1 = new ComponentWindow();
         //   _componentWindow1.Topmost = true;
         //   _componentWindow1.Show();
         //   ComponentWindow _componentWindow2 = new ComponentWindow();
         //   _componentWindow2.Topmost = true;
         //   _componentWindow2.Show();
        }
    }
}

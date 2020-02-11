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
    /// Interaction logic for ComponentWindow.xaml
    /// </summary>
    public partial class ComponentWindow : Window
    {
        //Managers
        Managers _managers;
        MySQLManager _mysqlManager;
        PrinterManager _printerManager;

        public ComponentWindow(int ID)
        {
            InitializeComponent();
            this.Topmost = true;

            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _printerManager = _managers.GetPrinterManager();

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

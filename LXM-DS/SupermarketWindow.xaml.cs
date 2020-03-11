using LXM_DS.MYSQL;
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
    /// Interaction logic for SupermarketWindow.xaml
    /// </summary>
    public partial class SupermarketWindow : Window
    {
        Managers _managers;
        MySQLManager _mysqlManager;
        public SupermarketWindow()
        {
            InitializeComponent();
            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
        }

        private void btnWybierz_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

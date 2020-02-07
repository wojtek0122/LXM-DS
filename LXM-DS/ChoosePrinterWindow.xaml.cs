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
    /// Interaction logic for ChoosePrinterWindow.xaml
    /// </summary>
    public partial class ChoosePrinterWindow : Window
    {
        public ChoosePrinterWindow()
        {
            InitializeComponent();
        }

        private void btnWybierz_Click(object sender, RoutedEventArgs e)
        {
            ComponentWindow _componentWindow = new ComponentWindow();
            _componentWindow.Show();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

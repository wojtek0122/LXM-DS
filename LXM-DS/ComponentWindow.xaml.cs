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
        public ComponentWindow(int ID)
        {
            InitializeComponent();
            this.Topmost = true;
            _browser.Navigate("file:///D:/FID2505.04%2040X5167%20FLATBED%20SCANNER,%20COMPLETE.pdf");
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

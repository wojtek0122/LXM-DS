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
    /// Interaction logic for ChoosePrinterWindow.xaml
    /// </summary>
    public partial class ChoosePrinterWindow : Window
    {
        private string _printerMT;
        private string _printerSN;
        public ChoosePrinterWindow()
        {
            InitializeComponent();
        }

        private void btnWybierz_Click(object sender, RoutedEventArgs e)
        {
            if(1==2)
            {
                ComponentWindow _componentWindow = new ComponentWindow();
                _componentWindow.Show();
            }
            else
            {
                this.lblInfo.Content = "BLAD: Drukarka nie zostala jeszcze przetestowana!";
            }
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtLabel.Text.Length == 14)
            {
                ParseTextBoxContent(this.txtLabel.Text.ToString());
            }
        }

        private void ParseTextBoxContent(string Content)
        {
            _printerMT = Content.Substring(1, 4);
            this.txtLabel.Text = _printerMT;
            _printerSN = Content.Substring(5, 9);
            this.txtLabel.Text = _printerSN;
            ChangePrinterFoto();
        }

        private void ChangePrinterFoto()
        {
            if(File.Exists(@"..\..\FILES\" + _printerMT.ToString() + ".png"))
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

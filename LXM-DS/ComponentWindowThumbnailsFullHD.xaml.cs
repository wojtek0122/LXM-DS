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
    /// Interaction logic for ComponentWindowThumbnailsFullHD.xaml
    /// </summary>
    public partial class ComponentWindowThumbnailsFullHD : Window
    {
        public ComponentWindowThumbnailsFullHD(int TestID, string MT, string Status, string Login)
        {
            InitializeComponent();
            Button b1 = CreateNewButton("40X8017", 1, 0);
            Button b2 = CreateNewButton("40X8161", 1, 1);
            Button b3 = CreateNewButton("40X7678", 1, 2);

        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {

        }

        public Button CreateNewButton(string PN, int Column, int Row)
        {
            Image _img = new Image()
            {
                Width = 255,
                Height = 150,
                VerticalAlignment = VerticalAlignment.Top
            };
            _img.Source = new ImageSourceConverter().ConvertFromString(@"C:\Syncreon\FID\miniaturka\" + PN + ".png") as ImageSource;

            var _converter = new System.Windows.Media.BrushConverter();
            var _brush = (Brush)_converter.ConvertFromString("#FF4C4F5B");

            Label _lab = new Label()
            {
                Content = PN,
                Width = 255,
                Height = 50,
                FontFamily = new FontFamily("Oxygen-Bold"),
                FontSize = 18,
                Foreground = _brush,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            _lab.Margin = new Thickness(0, 160, 0, 0);

            Grid _grid = new Grid()
            {
                Width = 255,
                Height = 200,
            };

            _grid.Children.Add(_img);
            _grid.Children.Add(_lab);

            _brush = (Brush)_converter.ConvertFromString("#FFFFFFFF");
            Button _btn = new Button()
            {
                Name = "PN" + PN,
                Content = _grid,
                Height = 200,
                Width = 255,
                Background = _brush,
            };
            _btn.Click += new RoutedEventHandler(_btn_Click);
            Grid.SetColumn(_btn, Column);
            Grid.SetRow(_btn, Row);

            grdThumbnails.Children.Add(_btn);

            return _btn;
        }

        private void _btn_Click(object sender, RoutedEventArgs e)
        {
            Button _button = sender as Button;
            if (_button.Name == "PN40X8017")
            {

            }
        }
    }
}

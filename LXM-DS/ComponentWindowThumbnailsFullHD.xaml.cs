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
    /// Interaction logic for ComponentWindowThumbnailsFullHD.xaml
    /// </summary>
    public partial class ComponentWindowThumbnailsFullHD : Window
    {
        //Managers
        Managers _managers;
        MySQLManager _mysqlManager;
        PrinterManager _printerManager;

        Printer _printer;

        int _testID;
        string _mt;
        string _status;
        string _login;

        List<Component> _dismantledComponentsList;
        List<Button> _buttonTable;


        public ComponentWindowThumbnailsFullHD(int TestID, string MT, string Status, string Login)
        {
            _testID = TestID;
            _mt = MT;
            _status = Status;
            _login = Login;

            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _printerManager = _managers.GetPrinterManager();

            _printer = _printerManager.GetPrinterByMT(_mt);
            _dismantledComponentsList = new List<Component>();
            _buttonTable = new List<Button>();
            foreach (var value in _printer.GetComponentList())
            {
                _dismantledComponentsList.Add(value);
            }

            InitializeComponent();

            int _maxColumns = 6;
            //int _column = 1;
            int _maxRows = 5;
            //int _row = 0;
            int _index = 0;

            int _count = _dismantledComponentsList.Count;
            if(_count >= 30)
            {
                //Jeżeli więcej niż 30 komponentów
            }
            else
            {
                
                for (int row = 1; row <= _maxRows; row++)
                {
                    for (int col = 1; col < _maxColumns; col++)
                    {
                        _buttonTable.Add(CreateNewButton(_dismantledComponentsList[_index]._PN, col, row - 1));
                        _index++;
                        if (_count == _index)
                            break;
                    }
                    if (_count == _index)
                        break;
                }

            }

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
            ComponentViewFullHD _componentViewFullHD = new ComponentViewFullHD(_login, _testID, _button.Name);
            _componentViewFullHD.Topmost = true;
            _componentViewFullHD.Show();
            _button.IsEnabled = false;
        }
    }
}

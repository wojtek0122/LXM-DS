using LXM_DS.BUTTON;
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
using System.Windows.Threading;

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for ComponentWindowThumbnailsFullHD.xaml
    /// </summary>
    public partial class ComponentWindowThumbnailsFullHD : Window
    {
        //Managers
        Managers _managers;
        PrinterManager _printerManager;
        ButtonListManager _buttonListManager;

        Printer _printer;

        int _testID;
        string _mt;
        string _status;
        string _login;
        int _countDismantled = 0;
        int _sID = 0;

        List<Component> _dismantledComponentsList;

        DispatcherTimer _timer;

        public ComponentWindowThumbnailsFullHD(int TestID, string MT, string Status, string Login)
        {
            _testID = TestID;
            _mt = MT;
            _status = Status;
            _login = Login;

            _managers = Managers.CreateManagers();
            _printerManager = _managers.GetPrinterManager();
            _buttonListManager = _managers.GetButtonListManager();

            _printer = _printerManager.GetPrinterByMT(_mt);
            _dismantledComponentsList = new List<Component>();
            foreach (var value in _printer.GetComponentList())
            {
                _dismantledComponentsList.Add(value);
            }             

            InitializeComponent();
            this.lblMT.Content = "MT: " + _mt;

            int _maxColumns = 7;
            int _maxRows = 5;
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
                        _buttonListManager.AddItemToButtonList((CreateNewButton(_dismantledComponentsList[_index]._PN, col, row - 1), col, row - 1));
                        _index++;
                        if (_count == _index)
                            break;
                    }
                    if (_count == _index)
                        break;
                }

            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            StatusButton _btn;
            foreach(var value in _buttonListManager.GetButtonList())
            {
                _btn = value.Item1;
                if(_countDismantled == _dismantledComponentsList.Count)
                {
                    this.Close();
                }
                if (_btn.STATUS != "" && _btn.ACTIVE == true)
                {
                    if (_btn.STATUS == "OK")
                    {
                        CreateImageStatus("OK", value.Item2, value.Item3);
                        _btn.ACTIVE = false;
                        _countDismantled++;
                    }
                    if (_btn.STATUS == "NOK")
                    {
                        CreateImageStatus("NOK", value.Item2, value.Item3);
                        _btn.ACTIVE = false;
                        _countDismantled++;
                    }
                    if (_btn.STATUS == "NONE")
                    {
                        CreateImageStatus("NONE", value.Item2, value.Item3);
                        _btn.ACTIVE = false;
                        _countDismantled++;
                    }
                }
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {

        }

        public StatusButton CreateNewButton(string PN, int Column, int Row)
        {
            int _column = Column;
            int _row = Row;

            Image _img = new Image()
            {
                Width = 255,
                Height = 150,
                VerticalAlignment = VerticalAlignment.Top
            };
            _img.Source = new ImageSourceConverter().ConvertFromString(ParseFIDPathFromXML() + @"\FID\Thumbnails\" + PN + ".png") as ImageSource;

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
            StatusButton _btn = new StatusButton()
            {
                Name = "PN" + PN,
                Content = _grid,
                Height = 200,
                Width = 255,
                Background = _brush,
                ID = _sID++,
            };
            _btn.Click += new RoutedEventHandler(_btn_Click);
            Grid.SetColumn(_btn, _column);
            Grid.SetRow(_btn, _row);

            grdThumbnails.Children.Add(_btn);

            return _btn;
        }

        private void _btn_Click(object sender, RoutedEventArgs e)
        {
            StatusButton _button = sender as StatusButton;  
            ComponentViewFullHD _componentViewFullHD = new ComponentViewFullHD(_login, _testID, _button.Name, _button.ID);
            _componentViewFullHD.Topmost = true;
            _componentViewFullHD.Show();
            _button.IsEnabled = false;
        }

        public void CreateImageStatus(string Status, int Column, int Row)
        {
            Image _imgState = new Image()
            {
                Height = 200,
                Width = 255,
                VerticalAlignment = VerticalAlignment.Center,
            };

            switch (Status)
            {
                case "OK":
                    {
                        _imgState.Source = new ImageSourceConverter().ConvertFromString("..\\..\\FILES\\OK.png") as ImageSource;
                        break;
                    };
                case "NOK":
                    {
                        _imgState.Source = new ImageSourceConverter().ConvertFromString("..\\..\\FILES\\NOK.png") as ImageSource;
                        break;
                    };
                case "NONE":
                    {
                        _imgState.Source = new ImageSourceConverter().ConvertFromString("..\\..\\FILES\\NONE.png") as ImageSource;
                        break;
                    };
            }
            Grid.SetColumn(_imgState, Column);
            Grid.SetRow(_imgState, Row);
            grdThumbnails.Children.Add(_imgState);
        }

        public string ParseFIDPathFromXML()
        {
            string _parsedPath = "";
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\Config.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "CONFIG"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _parsedPath = String.Format(_xmlReader.GetAttribute("PATH"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _parsedPath;
        }
    }
}

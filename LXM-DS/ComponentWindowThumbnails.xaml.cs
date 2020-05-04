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
    /// Interaction logic for ComponentWindowThumbnails.xaml
    /// </summary>
    public partial class ComponentWindowThumbnails : Window
    {
        //Managers
        Managers _managers;
        PrinterManager _printerManager;
        ButtonListManager _buttonListManager;
        AUTOUPDATE.AutoUpdate _autoUpdate;
        MYSQL.MySQLManager _mysqlManager;

        Printer _printer;
        List<Image> _imageStatusButtonList; 

        int _testID;
        string _mt;
        string _status;
        string _login;
        int _sID = 0;

        int _pageCurrent = 1;
        int _pageMax = 1;

        int _maxColumnsOnPage = 3;
        int _maxRowsOnPage = 3;
        int _maxButtonsOnPage = 0;

        DateTime _dismantleStartDateTime;

        List<Component> _dismantledComponentsList;

        DispatcherTimer _timer;

        public ComponentWindowThumbnails(int TestID, string MT, string Status, string Login)
        {
            //this.Topmost = true;
            _testID = TestID;
            _mt = MT;
            _status = Status;
            _login = Login;

            _managers = Managers.CreateManagers();
            _printerManager = _managers.GetPrinterManager();
            _buttonListManager = _managers.GetButtonListManager();
            _autoUpdate = AUTOUPDATE.AutoUpdate.CreateAutoUpdate();
            _mysqlManager = _managers.GetMySQLManager();

            _printer = _printerManager.GetPrinterByMT(_mt);
            _imageStatusButtonList = new List<Image>();
            _dismantledComponentsList = new List<Component>();
            foreach (var value in _printer.GetComponentList())
            {
                _dismantledComponentsList.Add(value);
            }

            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
            _timer.Start();

            _maxButtonsOnPage = _maxColumnsOnPage * _maxRowsOnPage;
            _pageMax = _dismantledComponentsList.Count / (_maxButtonsOnPage);
            if ((_dismantledComponentsList.Count % _maxButtonsOnPage) > 0)
            {
                _pageMax++;
            }
            if (_dismantledComponentsList.Count < _maxButtonsOnPage)
            {
                _pageMax = 1;
            }
            if (_pageMax > 1)
            {
                btnRight.Visibility = Visibility.Visible;
            }

            //CreateButtons
            CreateButtonList();
            ButtonSetVisibility();
            CreateImageButtonList();

            this.lblMT.Content = "MT: " + _mt + " \tStrona: " + _pageCurrent + " / " + _pageMax;
            _dismantleStartDateTime = DateTime.Now;
        }

        public void ButtonSetVisibility()
        {
            StatusButton _btn;
            int _index = 0;
            int _min = (_pageCurrent * _maxButtonsOnPage) - _maxButtonsOnPage;
            int _max = (_pageCurrent * _maxButtonsOnPage) - 1;
            foreach (var value in _buttonListManager.GetButtonList())
            {
                _btn = value.Item1;
                if (_index >= _min && _index <= _max)
                {
                    _btn.Visibility = Visibility.Visible;
                }
                else
                {
                    _btn.Visibility = Visibility.Hidden;
                }
                _index++;
            }
        }

        public void ImageButtonSetVisibility()
        {
            Image _img;
            int _index = 0;
            int _min = (_pageCurrent * _maxButtonsOnPage) - _maxButtonsOnPage;
            int _max = (_pageCurrent * _maxButtonsOnPage) - 1;

            foreach (var value in _imageStatusButtonList)
            {
                _img = value;
                if (_index >= _min && _index <= _max)
                {
                    ChangeImageStatus(_img, _buttonListManager.GetStatusButton(_index));
                }
                else
                {
                    _img.Visibility = Visibility.Hidden;
                }
                _index++;
            }
        }

        public void CreateButtonList()
        {
            int _index = 0;
            for (int page = 1; page < _pageMax + 1; page++)
            {
                for (int row = 1; row < _maxRowsOnPage + 1; row++)
                {
                    for (int col = 1; col < _maxColumnsOnPage + 1; col++)
                    {
                        if (_index == _dismantledComponentsList.Count)
                        {
                            break;
                        }
                        else
                        {
                            _buttonListManager.AddItemToButtonList((CreateNewButton(_dismantledComponentsList[_index]._PN, col, row - 1), col, row - 1));
                            _index++;
                        }
                    }
                    if (_index == _dismantledComponentsList.Count)
                    {
                        break;
                    }
                }
                if (_index == _dismantledComponentsList.Count)
                {
                    break;
                }
            }
        }

        public void CreateImageButtonList()
        {
            int _index = 0;
            for (int page = 1; page < _pageMax + 1; page++)
            {
                for (int row = 1; row < _maxRowsOnPage + 1; row++)
                {
                    for (int col = 1; col < _maxColumnsOnPage + 1; col++)
                    {
                        if (_index == _dismantledComponentsList.Count)
                        {
                            break;
                        }
                        else
                        {
                            _imageStatusButtonList.Add(CreateImageStatus(col, row - 1));
                            _index++;
                        }
                    }
                    if (_index == _dismantledComponentsList.Count)
                    {
                        break;
                    }
                }
                if (_index == _dismantledComponentsList.Count)
                {
                    break;
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            System.GC.Collect();
            ImageButtonSetVisibility();

            int _count = 0;
            StatusButton _btn;
            foreach (var value in _buttonListManager.GetButtonList())
            {
                _btn = value.Item1;
                if (_btn.STATUS != "")
                {
                    _count++;
                }
            }

            if (_count == _dismantledComponentsList.Count)
            {
                CloseWindow();
            }
        }

        private void CloseWindow()
        {
            _dismantledComponentsList = null;
            _imageStatusButtonList = null;
            _buttonListManager.ClearList();
            _mysqlManager.InsertPrinterDismantleTime(_mysqlManager.GetUserIDByLogin(_login), _testID, _printer._ID, _dismantleStartDateTime, DateTime.Now, DateTime.Now - _dismantleStartDateTime);
            _timer.Stop();
            CloseAdobeReaderProcess();
            _autoUpdate.CheckUpdate();
            this.Close();
        }

        private void CloseAdobeReaderProcess()
        {
            foreach (var process in System.Diagnostics.Process.GetProcessesByName("AcroRd32"))
            {
                process.Kill();
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_pageCurrent == 1 + 1)
            {
                _pageCurrent = 1;
                btnLeft.Visibility = Visibility.Hidden;
            }
            else
            {
                btnRight.Visibility = Visibility.Visible;
                _pageCurrent--;
            }
            if(_pageCurrent == 1 && _pageMax == 2)
            {
                btnRight.Visibility = Visibility.Visible;
            }

            ButtonSetVisibility();
            ImageButtonSetVisibility();
            this.lblMT.Content = "MT: " + _mt + " \tStrona: " + _pageCurrent + " / " + _pageMax;
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            if (_pageCurrent == _pageMax - 1)
            {
                _pageCurrent = _pageMax;
                btnRight.Visibility = Visibility.Hidden;
            }
            else
            {
                btnLeft.Visibility = Visibility.Visible;
                _pageCurrent++;
            }
            if (_pageCurrent == 2)
            {
                btnLeft.Visibility = Visibility.Visible;
            }
            ButtonSetVisibility();
            ImageButtonSetVisibility();
            this.lblMT.Content = "MT: " + _mt + " \tStrona: " + _pageCurrent + " / " + _pageMax;
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
                Visibility = Visibility.Hidden,
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
            ComponentView _componentView = new ComponentView(_login, _testID, _button.Name, _button.ID);
            _componentView.Topmost = true;
            _componentView.Show();
            //_button.IsEnabled = false;
        }

        public Image CreateImageStatus(int Column, int Row)
        {
            Image _imgState = new Image()
            {
                Height = 150,
                Width = 255,
                Visibility = Visibility.Hidden,
            };
            _imgState.Margin = new Thickness(0, -49, 0, 0);

            Grid.SetColumn(_imgState, Column);
            Grid.SetRow(_imgState, Row);
            grdThumbnails.Children.Add(_imgState);
            return _imgState;
        }

        public void ChangeImageStatus(Image ImageButton, StatusButton Button)
        {
            ImageButton.Visibility = Visibility.Visible;
            switch (Button.STATUS)
            {
                case "OK":
                    {
                        ImageButton.Source = new ImageSourceConverter().ConvertFromString("..\\..\\FILES\\OK.png") as ImageSource;
                        break;
                    };
                case "NOK":
                    {
                        ImageButton.Source = new ImageSourceConverter().ConvertFromString("..\\..\\FILES\\NOK.png") as ImageSource;
                        break;
                    };
                case "NONE":
                    {
                        ImageButton.Source = new ImageSourceConverter().ConvertFromString("..\\..\\FILES\\NONE.png") as ImageSource;
                        break;
                    };
                case "":
                    {
                        ImageButton.Visibility = Visibility.Hidden;
                        break;
                    };
            }
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

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if(grdMenu.Visibility == Visibility.Visible)
            {
                grdMenu.Visibility = Visibility.Hidden;
            }
            else
            {
                grdMenu.Visibility = Visibility.Visible;
            }
        }

        private void btnMenuExit_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
    }
}

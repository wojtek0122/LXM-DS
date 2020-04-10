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

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for SubmodelWindow.xaml
    /// </summary>
    public partial class SubmodelWindow : Window
    {
        public int _ID;
        public int _SUB;
        public List<SSubmodel> _subList;
        public List<StatusButton> _btnList;

        public SubmodelWindow(List<SSubmodel> SubList)
        {
            InitializeComponent();
            _subList = SubList;
            _btnList = new List<StatusButton>();

            for(int i = 0; i < _btnList.Count; i++)
            {
                _btnList.Add(CreateNewButton(_subList[i].GetSubModel(), _subList[i].GetName(), i));
            }

        }

        public StatusButton CreateNewButton(int Submodel, string Name, int Row)
        {
            var _converter = new System.Windows.Media.BrushConverter();
            var _brush = (Brush)_converter.ConvertFromString("#FFFFFFFF");
            StatusButton _btn = new StatusButton()
            {
                Name = "SUB" + Submodel.ToString(),
                Content = "Submodel: " + Submodel.ToString() + " - " + Name,
                Height = 80,
                Width = 450,
                Background = _brush,
                Visibility = Visibility.Visible,
            };
            _btn.Click += new RoutedEventHandler(_btn_Click);
            Grid.SetColumn(_btn, 0);
            Grid.SetRow(_btn, Row);

            grd.Children.Add(_btn);

            return _btn;
        }

        private void _btn_Click(object sender, RoutedEventArgs e)
        {
            StatusButton _button = sender as StatusButton;
            _ID = 1;
            _SUB = 2;
            this.Close();
        }

    }
}

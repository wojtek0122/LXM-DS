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
    /// Interaction logic for SNWindow.xaml
    /// </summary>
    public partial class SNWindow : Window
    {
        COMPONENTTYPE _type;
        int _testID;
        Managers _managers;
        MySQLManager _mysqlManager;

        public SNWindow(COMPONENTTYPE Type, int TestID)
        {
            InitializeComponent();
            this.Topmost = true;
            this.Focus();
            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _type = Type;
            _testID = TestID;
            ChangeLabel();
            txtSN.Focus();
        }

        public void ChangeLabel()
        {
            lblTitle.Content = "Podaj numer seryjny ";
            switch (_type)
            {
                case COMPONENTTYPE.MB: { lblTitle.Content += "płyty głównej:"; break; };
                case COMPONENTTYPE.OP: { lblTitle.Content += "panelu:"; break; };
                case COMPONENTTYPE.ENG: { lblTitle.Content += "płyty engine:"; break; };
            }
        }

        private void btnAddSN_Click(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case COMPONENTTYPE.MB: { _mysqlManager.UpsertResets(_testID, txtSN.Text, null, null); break; };
                case COMPONENTTYPE.OP: { _mysqlManager.UpsertResets(_testID, null, txtSN.Text, null); break; };
                case COMPONENTTYPE.ENG: { _mysqlManager.UpsertResets(_testID, null, null, txtSN.Text); break; };
            }
            this.Close();
        }

        private void btnKeyboard_Click(object sender, RoutedEventArgs e)
        {
            string KeyboardPath = @"C:\Windows\System32\osk.exe";
            System.Diagnostics.Process.Start(KeyboardPath);
        }
    }
}

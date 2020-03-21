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
    /// Interaction logic for SNWindow.xaml
    /// </summary>
    public partial class SNWindow : Window
    {
        string _type;
        int _testID;
        Managers _managers;
        MySQLManager _mysqlManager;

        public SNWindow(string Type, int TestID)
        {
            InitializeComponent();
            Topmost = true;
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
                case "MB": { lblTitle.Content += "płyty głównej:"; break; };
                case "OP": { lblTitle.Content += "panelu:"; break; };
                case "ENG": { lblTitle.Content += "płyty engine:"; break; };
            }
        }

        private void btnAddSN_Click(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case "MB": { _mysqlManager.UpsertResets(_testID, txtSN.Text, null, null); break; };
                case "OP": { _mysqlManager.UpsertResets(_testID, null, txtSN.Text, null); break; };
                case "ENG": { _mysqlManager.UpsertResets(_testID, null, null, txtSN.Text); break; };
            }
            this.Close();
        }
    }
}

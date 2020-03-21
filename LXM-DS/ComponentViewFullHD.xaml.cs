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
    /// Interaction logic for ComponentViewFullHD.xaml
    /// </summary>
    public partial class ComponentViewFullHD : Window
    {
        //Managers
        Managers _managers;
        MySQLManager _mysqlManager;
        int _testid;
        string _login;
        Component _component;

        public ComponentViewFullHD(string Login, int TestID, Component Component)
        {
            InitializeComponent();
            
            _managers = Managers.CreateManagers();
            _mysqlManager = _managers.GetMySQLManager();
            _login = Login;
            _testid = TestID;
            _component = Component;

            string _path = ParseFIDPathFromXML();
            _browser.Navigate(_path + @"FID\" + _component._FID + "." + _component._REV + " " + _component._PN + ".pdf");
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //Print label
            BARCODE.Barcode _barcode = new BARCODE.Barcode();
            _barcode.PrintLabel(_component._PN, _component._location);

            //UpdateStock
            _mysqlManager.UpdateComponentStock(_component._id);

            //Add log
            _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "OK");

            //Close window
            this.Close();
        }

        private void NOK_Click(object sender, RoutedEventArgs e)
        {
            //Print label - SCR
            BARCODE.Barcode _barcode = new BARCODE.Barcode();
            _barcode.PrintLabel(_component._PN, "SCR");

            //Add log
            _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "NOK");

            //Close window
            this.Close();
        }

        private void NONE_Click(object sender, RoutedEventArgs e)
        {
            //Add log
            _mysqlManager.InsertComponentLog(_login, _testid, _component._id, "NONE");

            //Close window
            this.Close();
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

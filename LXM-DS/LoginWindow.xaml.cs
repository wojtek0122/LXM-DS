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
using LXM_DS.MYSQL;
using LXM_DS.USERS;

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Managers _managers;
        MYSQL.MySQLManager _mysqlManager;
        DispatcherTimer _timer;
        public LoginWindow()
        {
            InitializeComponent();
            _managers = Managers.CreateManagers();
            _mysqlManager = MYSQL.MySQLManager.CreateManager();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += timer_Tick;
            _timer.Start();

            txtNFC.Focus();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now.Hour == 16) && (DateTime.Now.Minute == 0))
            {
                if (ParseBackupFromXML() == "YES")
                {
                    _mysqlManager.MakeBackup(ParsePathFromXML());
                }
            }
            if ((DateTime.Now.Hour == 5) && (DateTime.Now.Minute == 0))
            {
                System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
            }
        }

        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {           
            //UserManager _userManager = _managers.GetUserManager();
            MySQLManager _mySQLManager = _managers.GetMySQLManager();
            
            //string _login = this.txtLogin.Text.Trim().ToString();
            string _nfc = this.txtNFC.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(_nfc) && !String.IsNullOrWhiteSpace(_nfc))
            {
                try
                {
                    User _user = _mySQLManager.GetUserByNFC(_nfc);//_userManager.GetUserByLogin(_login);
                    if(_user != null)
                    {
                        if(ParseFullHDInfoFromXML() == "YES")
                        {
                            MainWindowFullHD _mainWindowFullHD = new MainWindowFullHD(_user._login, _user.GetPermission());
                            //_mainWindowFullHD.Topmost = true;
                            _mainWindowFullHD.Show();
                            this.Close();
                        }
                        else
                        {
                            MainWindow _mainWindow = new MainWindow(_user._login, _user.GetPermission());
                            //_mainWindow.Topmost = true;
                            _mainWindow.Show();
                            this.Close();
                        }
                        //string _pass = this.txtPassword.Password.ToString().Trim();
                        //if (!String.IsNullOrEmpty(_pass) && !String.IsNullOrWhiteSpace(_pass))
                        //{
                            //if (CalculateMD5(_pass) == _user.GetPassword())
                            //{
                            //}
                        //}
                    }
                    else
                    {
                        this.lblError.Content = "BŁĄD: Brak użytkownika!";
                    }
                }
                catch (Exception)
                {
                    this.lblError.Content = "BŁĄD: Brak połączenia!";
                    Console.WriteLine("BŁĄD:: Użytkownik nie istnieje!!!");
                }  
            }
                
        }

        public string CalculateMD5(string Input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public string ParseBackupFromXML()
        {
            string _parsed = "";
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\Config.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "CONFIG"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _parsed = String.Format(_xmlReader.GetAttribute("BACKUP"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _parsed;
        }

        public string ParsePathFromXML()
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

        public string ParseFullHDInfoFromXML()
        {
            string _parsedInfo = "";
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\Config.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "CONFIG"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _parsedInfo = String.Format(_xmlReader.GetAttribute("FULLHD"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _parsedInfo;
        }

        private void btnWylacz_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

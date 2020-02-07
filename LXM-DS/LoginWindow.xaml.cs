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
using LXM_DS.USERS;

namespace LXM_DS
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Managers _managers;
        public LoginWindow()
        {
            InitializeComponent();
            _managers = Managers.CreateManagers();
        }

        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            UserManager _userManager = _managers.GetUserManager();
            string _login = this.txtLogin.Text.Trim().ToString();
            if (!String.IsNullOrEmpty(_login) || !String.IsNullOrWhiteSpace(_login))
            {
                try
                {
                    User _user = _userManager.GetUserByLogin(_login);
                    if(_user != null)
                    {
                        string _pass = this.txtPassword.Password.ToString().Trim();
                        if (!String.IsNullOrEmpty(_pass) || !String.IsNullOrWhiteSpace(_pass))
                        {
                            if (CalculateMD5(_pass) == _user.GetPassword())
                            {
                                MainWindow _mainWindow = new MainWindow(_user.GetPermission());
                                _mainWindow.Topmost = true;
                                _mainWindow.Show();
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception)
                {
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
    }
}

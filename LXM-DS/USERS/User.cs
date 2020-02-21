using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.USERS
{
    class User
    {
        public string _login;
        private string _password;
        private int _permission;
        public string _nfc;
        public string _nfc2;

        public User(string NFC, string NFC2, string Login, string Password, int Permission)
        {
            _nfc = NFC;
            _nfc2 = NFC2;
            _login = Login;
            _password = Password;
            _permission = Permission;
        }

        public int GetPermission()
        {
            return _permission;
        }

        public string GetPassword()
        {
            return _password;
        }

    }
}

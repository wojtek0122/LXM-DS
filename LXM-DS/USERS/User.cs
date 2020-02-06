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

        public User(string Login, string Password, int Permission)
        {
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

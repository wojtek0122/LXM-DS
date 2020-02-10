using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXM_DS.USERS;
using MySql;

namespace LXM_DS.MYSQL
{
    
    class MySQLManager
    {
        private static MySQLManager _manager;
        private static MySql.Data.MySqlClient.MySqlConnection _conn;
        readonly private static string myConnectionString = "server=hosting1991484.online.pro;uid=00274822_lxm;pwd=Dismantle1@;database=00274822_lxm";

        private MySQLManager()
        {
            _conn = new MySql.Data.MySqlClient.MySqlConnection();
            _conn.ConnectionString = myConnectionString;
        }
        public static MySQLManager CreateManager()
        {
            if (_manager == null)
            {
                _manager = new MySQLManager();
            }
            return _manager;
        }

        public void Connect()
        {
            try
            {
                _conn.Open();

                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public User GetUser(string Login)
        {
            User _user = null;
            try
            {
                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT * FROM users WHERE login='" + Login + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while(_dataReader.Read())
                {
                    int _perm;
                    Int32.TryParse(_dataReader.GetValue(3).ToString(), out _perm);
                    _user = new User(_dataReader.GetValue(1).ToString(), _dataReader.GetValue(2).ToString(), _perm);
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _user;
        }

        public void GetComponents()
        {
            try
            {
                _conn.Open();

                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetPrinters()
        {
            try
            {
                _conn.Open();

                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

}

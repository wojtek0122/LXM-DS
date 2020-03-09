﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXM_DS.PRINTER;
using LXM_DS.USERS;
using MySql;

namespace LXM_DS.MYSQL
{
    
    class MySQLManager
    {
        private static MySQLManager _manager;
        private static MySql.Data.MySqlClient.MySqlConnection _conn;

        private MySQLManager()
        {
            _conn = new MySql.Data.MySqlClient.MySqlConnection();
            _conn.ConnectionString = ParseConnectionStringFromXML();
        }
        public static MySQLManager CreateManager()
        {
            if (_manager == null)
            {
                _manager = new MySQLManager();
            }
            return _manager;
        }

        public string ParseConnectionStringFromXML()
        {
            string _parsedString = "";
            try
            {
                System.Xml.XmlReader _xmlReader = System.Xml.XmlReader.Create("..\\..\\MySQL\\MySQL.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == System.Xml.XmlNodeType.Element) && (_xmlReader.Name == "MYSQL"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _parsedString = String.Format("server={0};uid={1};pwd={2};database={3}", _xmlReader.GetAttribute("SERVER"), _xmlReader.GetAttribute("USER"), _xmlReader.GetAttribute("PASSWORD"), _xmlReader.GetAttribute("DATABASE"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _parsedString;
        }

        public User GetUserByLogin(string Login)
        {
            User _user = null;
            try
            {
                if(_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT * FROM users WHERE login='" + Login + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    int _perm;
                    Int32.TryParse(_dataReader.GetValue(5).ToString(), out _perm);
                    _user = new User(_dataReader.GetValue(1).ToString(), _dataReader.GetValue(2).ToString(), _dataReader.GetValue(3).ToString(), _dataReader.GetValue(4).ToString(), _perm);
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

        public User GetUserByNFC(string NFC)
        {
            User _user = null;
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT * FROM users WHERE NFC='" + NFC + "' OR NFC2='" + NFC + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    int _perm;
                    Int32.TryParse(_dataReader.GetValue(5).ToString(), out _perm);
                    _user = new User(_dataReader.GetValue(1).ToString(), _dataReader.GetValue(2).ToString(), _dataReader.GetValue(3).ToString(), _dataReader.GetValue(4).ToString(), _perm);
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

        public List<Component> GetComponents()
        {
            List<Component> _list = new List<Component>(); 
            
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT * FROM components;";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    int _id;
                    Int32.TryParse(_dataReader.GetValue(0).ToString(), out _id);
                    int _stock;
                    Int32.TryParse(_dataReader.GetValue(8).ToString(), out _stock);
                    int _yield;
                    Int32.TryParse(_dataReader.GetValue(9).ToString(), out _yield);

                    _list.Add(new Component(_id, _dataReader.GetValue(1).ToString(), _dataReader.GetValue(2).ToString(), _dataReader.GetValue(3).ToString(), _dataReader.GetValue(4).ToString(), _dataReader.GetValue(5).ToString(), _dataReader.GetValue(6).ToString(), _dataReader.GetValue(7).ToString(), _stock, _yield, _dataReader.GetValue(10).ToString(), _dataReader.GetValue(11).ToString()));
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _list;

        }
        
        public List<Printer> GetPrinters(List<Component> ListOfComponents)
        {
            List<Printer> _list = new List<Printer>();
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }
                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT * FROM printers;";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    int _id;
                    Int32.TryParse(_dataReader.GetValue(0).ToString(), out _id);

                    string _components = _dataReader.GetValue(3).ToString();

                    _list.Add(new Printer(_id, _dataReader.GetValue(1).ToString(), _dataReader.GetValue(2).ToString(), ParseComponents(_components, ListOfComponents), _dataReader.GetValue(4).ToString()));
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _list;
        }

        private List<Component> ParseComponents(string Components, List<Component> ListOfComponents)
        {
            int _count = 0;

            List<Component> _list = new List<Component>();
            string[] _split = Components.Split(';');

            foreach (string iterator in _split)
            {
                _count = _list.Count;
                foreach (var value in ListOfComponents)
                {
                    if (iterator == value._PN)
                    {
                        _list.Add(value);
                        break;
                    }
                }
                if (_list.Count == _count)
                {
                    Console.WriteLine("BŁĄD:: Nie odnaleziono komponentu: '{0}'!!!", iterator);
                }
            }
            return _list;
        }

        public void InsertHDD(string HDDSN, string PrinterMT, string PrinterSN)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;

                string _sql = String.Format("INSERT INTO hdd (sn, prtmt, prtsn, destroyed) VALUES ('{0}', '{1}', '{2}', '{3}');", HDDSN, PrinterMT, PrinterSN, 0);

                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _mySqlCommand.ExecuteNonQuery();

                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void InsertTestDataToMySQL(string PrinterMT, string PrinterSN, string Status, string Login, bool Firmware, bool Defaults, bool Nvram)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;

                string _sql = String.Format("INSERT INTO test (printersid, sn, hddid, status, userid, date, dismantled, firmware, defaults, nvram) VALUES " +
                    "(" +
                    //printersid
                    "(SELECT printersid FROM printers WHERE mt='{0}'), " +
                    //printer sn
                    "'{1}', " +
                    //hddid
                    "(SELECT hddid FROM hdd WHERE prtsn='{1}'), " +
                    //status
                    "'{2}', " +
                    //userid
                    "(SELECT usersid FROM users WHERE login='{3}'), " +
                    //date
                    "'{4}', " +
                    //dismantled
                    "'{5}', " +
                    //firmware
                    "'{6}', " +
                    //defaults
                    "'{7}', " +
                    //nvram
                    "'{8}'" +
                    ");", PrinterMT, PrinterSN, Status, Login, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, (Firmware==true?1:0), (Defaults==true?1:0), (Nvram==true?1:0));

                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _mySqlCommand.ExecuteNonQuery();

                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int GetTestIDFromTest(string SN)
        {
            int _id = 0;
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT testid FROM test WHERE sn='" + SN + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    if (_dataReader.GetValue(0).Equals(null))
                    {
                        _id = 0;
                    }
                    else
                    {
                        Int32.TryParse(_dataReader.GetValue(0).ToString(), out _id);
                    }
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _id;
        }

        public int GetPrinterIDFromTest(string SN)
        {
            int _id = 0;
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT printersid FROM test WHERE sn='" + SN + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    if (_dataReader.GetValue(0).Equals(null))
                    {
                        _id = 0;
                    }
                    else
                    {
                        Int32.TryParse(_dataReader.GetValue(0).ToString(), out _id);
                    }
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _id;
        }

        public string GetStatusFromTest(string SN)
        {
            string _status = "";
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT status FROM test WHERE sn='" + SN + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    _status = _dataReader.GetValue(0).ToString();
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _status;
        }

        public string GetMTFromPrintersWherePrinterID(int PrinterID)
        {
            string _mt = "";
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT mt FROM printers WHERE printersid='" + PrinterID + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    _mt = _dataReader.GetValue(0).ToString();
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _mt;
        }

        public void SetDismantled(int TestID)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;

                string _sql = String.Format("UPDATE test SET dismantled='1' WHERE testid='{0}'", TestID);

                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _mySqlCommand.ExecuteNonQuery();
               
                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public string GetFIDFromComponents(string SN)
        {
            string _status = "";
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = "SELECT status FROM test WHERE sn='" + SN + "';";
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    _status = _dataReader.GetValue(0).ToString();
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _status;
        }

        public void InsertComponentLog(string Login, int TestID, int ComponentID, string Status)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                string _sql = ";";


                _sql = String.Format("" +
                    "INSERT INTO dismantled (usersid, testid, componentid, status, date) VALUES " +
                    "(" +
                    "(SELECT usersid FROM users WHERE login='{0}'), " +
                    "'{1}', " +
                    "'{2}', " +
                    "'{3}', " +
                    "'{4}'" +
                    ");", Login, TestID, ComponentID, Status, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _mySqlCommand.ExecuteNonQuery();
                _mySqlCommand.Dispose();

                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateComponentStock(int ComponentID)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;

                string _sql = String.Format("UPDATE components SET stock = stock + 1 WHERE componentsid='{0}'", ComponentID);

                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _mySqlCommand.ExecuteNonQuery();

                _mySqlCommand.Dispose();
                _conn.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }

}

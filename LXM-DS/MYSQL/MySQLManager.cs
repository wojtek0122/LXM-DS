using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXM_DS.PRINTER;
using LXM_DS.USERS;
using LXM_DS.AUTOUPDATE;
using MySql;

namespace LXM_DS.MYSQL
{
    
    class MySQLManager
    {
        private static MySQLManager _manager;
        private static MySql.Data.MySqlClient.MySqlConnection _conn;
        private static MySql.Data.MySqlClient.MySqlBulkLoader _bulkLoader;

        private MySQLManager()
        {
            _conn = new MySql.Data.MySqlClient.MySqlConnection(ParseConnectionStringFromXML());
            _bulkLoader = new MySql.Data.MySqlClient.MySqlBulkLoader(_conn);
        }
        public static MySQLManager CreateManager()
        {
            if (_manager == null)
            {
                _manager = new MySQLManager();
            }
            return _manager;
        }

        public AppVersion GetCurrentVersion()
        {
            AppVersion _version = null;
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = String.Format("SELECT * FROM versions WHERE current='1';");
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();

                while (_dataReader.Read())
                {
                    int _ver;
                    Int32.TryParse(_dataReader.GetValue(1).ToString(), out _ver);
                    _version = new AppVersion(_ver , _dataReader.GetValue(2).ToString(), _dataReader.GetValue(3).ToString(), _dataReader.GetValue(4).ToString(), 1);
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _version;
        }

        public void MakeBackup(string Path) //Install-Package MySqlBackup.NET
        {
            string _file = Path + "MYSQL\\";
            string _datetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(ParseConnectionStringFromXML()))
                {
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand())
                    {
                        using (MySql.Data.MySqlClient.MySqlBackup mb = new MySql.Data.MySqlClient.MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(_file + _datetime + ".sql");
                            conn.Close();
                        }
                    }
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void UpdateResets(int TestID, string MBSN, string OPSN, string ENGINESN)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;

                string _sql = String.Format(
                    "UPDATE resets SET ", TestID);
                if (MBSN != null)
                {
                    _sql += String.Format("mbsn='{0}',", MBSN);
                }
                if (OPSN != null)
                {
                    _sql += String.Format("opsn='{0}',", OPSN);
                }
                if (ENGINESN != null)
                {
                    _sql += String.Format("enginesn='{0}',", ENGINESN);
                }
                _sql += String.Format(
                    " date='{0}'" +
                    " WHERE testid='{1}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), TestID);

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

        public void InsertResets(int TestID, string MBSN, string OPSN, string ENGINESN)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;

                string _sql = String.Format(
                    " INSERT INTO resets (testid, date, ");
                if (MBSN != null)
                {
                    _sql += String.Format("mbsn");
                }
                if (OPSN != null)
                {
                    _sql += String.Format("opsn");
                }
                if (ENGINESN != null)
                {
                    _sql += String.Format("enginesn");
                }
                _sql += String.Format(") VALUES('{0}', '{1}', ", TestID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (MBSN != null)
                {
                    _sql += String.Format("'{0}'", MBSN);
                }
                if (OPSN != null)
                {
                    _sql += String.Format("'{0}'", OPSN);
                }
                if (ENGINESN != null)
                {
                    _sql += String.Format("'{0}'", ENGINESN);
                }
                _sql += String.Format(");");

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

        public void UpsertResets(int TestID, string MBSN, string OPSN, string ENGINESN)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                _conn.Open();

                string _sql = String.Format("SELECT resetsid FROM resets WHERE testid='{0}';", TestID);
                _mySqlCommand = new MySql.Data.MySqlClient.MySqlCommand(_sql, _conn);
                _dataReader = _mySqlCommand.ExecuteReader();
                int _value = 0;

                while (_dataReader.Read())
                {
                    Int32.TryParse(_dataReader.GetValue(0).ToString(), out _value);
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();

                if (_value == 0)
                {
                    InsertResets(TestID, MBSN, OPSN, ENGINESN);
                }
                else
                {
                    UpdateResets(TestID, MBSN, OPSN, ENGINESN);
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddNewOrderFromCSV(string Path)
        {
            _bulkLoader.TableName = "orders";
            _bulkLoader.FieldTerminator = ";";
            _bulkLoader.LineTerminator = "\n";
            _bulkLoader.FileName = Path;

            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();

                // Upload data from file
                int count = _bulkLoader.Load();
                Console.WriteLine(count + " lines uploaded.");

                _conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
                            //XAMPP
                            //datasource=127.0.0.1;port=3306;username=root;password=;database=test;
                            _parsedString = String.Format("datasource={0};port={1};username={2};password={3};database={4};", _xmlReader.GetAttribute("DATASOURCE"), _xmlReader.GetAttribute("PORT"), _xmlReader.GetAttribute("USERNAME"), _xmlReader.GetAttribute("PASSWORD"), _xmlReader.GetAttribute("DATABASE"));
                            //_parsedString = String.Format("server={0};uid={1};pwd={2};database={3}", _xmlReader.GetAttribute("SERVER"), _xmlReader.GetAttribute("USER"), _xmlReader.GetAttribute("PASSWORD"), _xmlReader.GetAttribute("DATABASE"));
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

        public Component GetComponentByPN(string PN)
        {
            Component _component = null;
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;
                MySql.Data.MySqlClient.MySqlDataReader _dataReader;
                string _sql = String.Format("SELECT * FROM components WHERE pn='{0}';", PN);
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

                    _component = new Component(_id, _dataReader.GetValue(1).ToString(), _dataReader.GetValue(2).ToString(), _dataReader.GetValue(3).ToString(), _dataReader.GetValue(4).ToString(), _dataReader.GetValue(5).ToString(), _dataReader.GetValue(6).ToString(), _dataReader.GetValue(7).ToString(), _stock, _yield, _dataReader.GetValue(10).ToString(), _dataReader.GetValue(11).ToString());
                }

                _dataReader.Close();
                _mySqlCommand.Dispose();
                _conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _component;

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
                    ");", PrinterMT, PrinterSN, Status, Login, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, (Firmware == true ? 1 : 0), (Defaults == true ? 1 : 0), (Nvram == true ? 1 : 0));

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

        public void InsertTestDataToMySQL(string PrinterMT, string PrinterSN, string Status, string Login, bool Firmware, bool Defaults, bool Nvram, bool Fuser, bool Head, bool PSU, bool Scanner, bool ADF, bool OP, bool MB, bool ENG)
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Open)
                {
                    _conn.Close();
                }

                _conn.Open();
                MySql.Data.MySqlClient.MySqlCommand _mySqlCommand;

                string _sql = String.Format("INSERT INTO test (printersid, sn, hddid, status, userid, date, dismantled, firmware, defaults, nvram, F, H, PSU, S, A, OP, MB, ENG) VALUES " +
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
                    "'{8}', " +
                    //F
                    "'{9}', " +
                    //H
                    "'{10}', " +
                    //PSU
                    "'{11}', " +
                    //S
                    "'{12}', " +
                    //A
                    "'{13}', " +
                    //OP
                    "'{14}', " +
                    //MB
                    "'{15}', " +
                    //ENG
                    "'{16}'" +
                    ");", PrinterMT, PrinterSN, Status, Login, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, (Firmware==true?1:0), (Defaults==true?1:0), (Nvram==true?1:0),
                     (Fuser == true ? 1 : 0), (Head == true ? 1 : 0), (PSU == true ? 1 : 0), (Scanner == true ? 1 : 0), (ADF == true ? 1 : 0), (OP == true ? 1 : 0), (MB == true ? 1 : 0), (ENG == true ? 1 : 0));

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

        public int GetTestIDFromTestBySN(string SN)
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

        public int GetPrinterIDFromTestBySN(string SN)
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

        public string GetTestStatusFromTestBySN(string SN)
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

        public string GetTestStatusFromTestByTestID(int TestID)
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
                string _sql = "SELECT status FROM test WHERE testid='" + TestID + "';";
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

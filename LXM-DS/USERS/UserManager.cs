using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LXM_DS.USERS
{
    class UserManager
    {
        public List<User> _users = new List<User>();

        public UserManager()
        {
            LoadUsersFromXML();
        }

        private void LoadUsersFromXML()
        {
            Console.WriteLine("LOG:: Wczytywanie użytkowników...");
            try
            {
                XmlReader _xmlReader = XmlReader.Create("..\\..\\USERS\\USERS.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name == "USERS"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            int _permission;
                            int.TryParse(_xmlReader.GetAttribute("PERMISSION"), out _permission);
                            _users.Add(new User(_xmlReader.GetAttribute("LOGIN"), _xmlReader.GetAttribute("PASS"), _permission));
                        }
                    }
                }
                Console.WriteLine("LOG:: UŻytkownicy wczytani");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD:: Wczytywania użytkowników!!!");
                Console.WriteLine(ex.ToString());
            }
        }

        public User GetUserByLogin(string Login)
        {
            User val = null;
            foreach(var iterator in _users)
            {
                if (iterator._login == Login)
                    val = iterator;
            }
            return val;
        }
    }
}

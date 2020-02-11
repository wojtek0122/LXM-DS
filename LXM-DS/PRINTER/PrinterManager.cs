using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LXM_DS;
using LXM_DS.MYSQL;

namespace LXM_DS.PRINTER
{
    class PrinterManager
    {
        private static List<Component> _components = new List<Component>();
        private static List<Printer> _printers = new List<Printer>();

        public PrinterManager()
        {
            //LoadComponents();
            //LoadPrinters();
        }

        public void SetComponents(List<Component> ListOfComponents)
        {
            _components = ListOfComponents;
        }

        public List<Component> GetComponents()
        {
            return _components;
        }

        public void SetPrinters(List<Printer> ListOfPrinters)
        {
            _printers = ListOfPrinters;
        }

        public List<Printer> GetPrinters()
        {
            return _printers;
        }

        //Load Components from XML
        /*NOT USED IN MYSQL VERSION
        private void LoadComponentsFromXML()
        {
            Console.WriteLine("LOG:: Wczytywanie komponentów...");
            try
            {
                XmlReader _xmlReader = XmlReader.Create("..\\..\\PRINTER\\COMPONENTS.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name == "COMP"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            //_components.Add(new Component(_xmlReader.GetAttribute("PN"), _xmlReader.GetAttribute("FOTO"), _xmlReader.GetAttribute("DESC"), _xmlReader.GetAttribute("FID"), _xmlReader.GetAttribute("TYPE")));
                        }
                    }
                }
                Console.WriteLine("LOG:: Komponenty wczytane!");
            } 
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD:: Wczytywania komponentów!!!");
                Console.WriteLine(ex.ToString());
            }
            
        }
        */

        //Load Printers from XML
        /*NOT USED IN MYSQL VERSION
        private void LoadPrintersFromXML()
        {
            Console.WriteLine("LOG:: Wczytywanie drukarek...");
            try
            {
                XmlReader _xmlReader = XmlReader.Create("..\\..\\PRINTER\\PRINTERS.xml");
                while (_xmlReader.Read())
                {
                    if ((_xmlReader.NodeType == XmlNodeType.Element) && (_xmlReader.Name == "PRT"))
                    {
                        if (_xmlReader.HasAttributes)
                        {
                            _printers.Add(new Printer(_xmlReader.GetAttribute("MT"), ParseComponents(_xmlReader.GetAttribute("COMPONENTS"))));
                        }
                    }
                }
                Console.WriteLine("LOG:: Drukarki wczytane!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("BŁĄD:: Wczytywania drukarek!!!");
                Console.WriteLine(ex.ToString());
            }            
        }
        */

        /*NOT USED IN MYSQL VERSION
         * private List<Component> ParseComponents(string Components)
        {
            int _count = 0;

            List<Component> _list = new List<Component>();
            string[] _split = Components.Split(';');
           
            foreach (string iterator in _split)
            {
                _count = _list.Count;
                foreach (var value in _components)
                {
                    if (iterator == value._PN)
                    {
                        _list.Add(value);
                        break;
                    }
                }
                if(_list.Count == _count)
                {
                    Console.WriteLine("BŁĄD:: Nie odnaleziono komponentu: '{0}'!!!", iterator);
                }
            }
            return _list;
        }
        */
    }
}

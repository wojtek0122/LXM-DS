using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LXM_DS.PRINTER
{
    class PrinterManager
    {
        private static List<Component> _components = new List<Component>();
        private static List<Printer> _printers = new List<Printer>();

        public PrinterManager()
        {
            LoadComponents();
            LoadPrinters();

            Console.WriteLine();
        }

        //wczytaj komponenty
        private void LoadComponents()
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
                            Component _comp = new Component(_xmlReader.GetAttribute("PN"), _xmlReader.GetAttribute("FOTO"), _xmlReader.GetAttribute("DESC"), _xmlReader.GetAttribute("FID"));
                            _components.Add(_comp);
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

        //wczytaj drukarki
        private void LoadPrinters()
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

        private List<Component> ParseComponents(string Components)
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

    }
}

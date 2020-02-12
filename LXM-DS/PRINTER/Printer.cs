using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.PRINTER
{
    class Printer
    {
        //private static int _sID = 0;
        private int _ID;
        public string _machineType;
        public List<Component> _componentsList;
        string _foto;

        public Printer(int ID, string MachineType, List<Component> Components, string Foto)
        {
            _ID = ID;//_sID++;
            _machineType = MachineType;
            _componentsList = Components;
            _foto = Foto;
        }

        public List<Component> GetComponentList()
        {
            return _componentsList;
        }

    }
}

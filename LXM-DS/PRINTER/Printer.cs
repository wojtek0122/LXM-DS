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
        private static int _sID = 0;
        private int _ID;
        public string _machineType;
        private List<Component> _componentsList;

        public Printer(string MachineType, List<Component> Components)
        {
            _ID = _sID++;
            _machineType = MachineType;
            _componentsList = Components;
        }

    }
}

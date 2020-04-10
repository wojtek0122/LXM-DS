using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.PRINTER
{
    public class SSubmodel
    {
        int _printersid;
        int _submodel;
        string _name;

        public SSubmodel(int PrintersID, int Submodel, string Name)
        {
            _printersid = PrintersID;
            _submodel = Submodel;
            _name = Name;
        }

        public int GetPrintersID()
        {
            return _printersid;
        }

        public int GetSubModel()
        {
            return _submodel;
        }

        public string GetName()
        {
            return _name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.PRINTER
{
    class Component
    {
        private static int _sID = 0;
        private int _ID;
        public string _PN;
        public string _foto;
        public string _description;
        public string _FID;

        public Component(string PN, string Foto, string Description, string FID)
        {
            _ID = _sID++;
            _PN = PN;
            _foto = Foto;
            _description = Description;
            _FID = FID;
        }
    }
}

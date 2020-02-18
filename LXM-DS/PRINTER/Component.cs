﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.PRINTER
{
    class Component
    {
        //private static int _sID = 0;
        private int _ID;
        public string _PN;
        public string _foto;
        public string _description;
        public string _FID;
        public string _REV;
        public string _type;
        public int _stock;
        public int _yield;
        public string _location;

        public Component(int ID, string PN, string Foto, string Description, string FID, string Rev, string Type, int YIELD, string Location)
        {
            _ID = ID; //_sID++;
            _PN = PN;
            _foto = Foto;
            _description = Description;
            _FID = FID;
            _REV = Rev;
            _type = Type;
            _stock = 0;
            _yield = YIELD;
            _location = Location;
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1}", _PN, _description);
        }
    }
}

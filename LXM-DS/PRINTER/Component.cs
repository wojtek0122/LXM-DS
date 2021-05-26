using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.PRINTER
{
    public enum COMPONENTTYPE
    {
        F,
        H,
        PSU,
        S,
        A,
        OP,
        MB,
        ENG,
        E,
        M
    }

    public class Component
    {
        //private static int _sID = 0;
        public int _id;
        public string _PN;
        public string _foto;
        public string _description;
        public string _test;
        public string _FID;
        public string _REV;
        //public string _type;
        public COMPONENTTYPE _type;
        public int _stock;
        public int _yield;
        public string _location;
        public string _comment;
        public int _destination;

        public Component(int ID, string PN, string Foto, string Description, string Test, string FID, string Rev, string Type, int Stock, int YIELD, string Location, string Comment, int Destination)
        {
            _id = ID; //_sID++;
            _PN = PN;
            _foto = Foto;
            _description = Description;
            _test = Test;
            _FID = FID;
            _REV = Rev;
            //_type = Type;
            _type = ParseComponentType(Type);
            _stock = Stock;
            _yield = YIELD;
            _location = Location;
            _comment = Comment;
            _destination = Destination;
        }

        public Component(Component value)
        {
            _id = value._id;
            _PN = value._PN;
            _foto = value._foto;
            _description = value._description;
            _test = value._test;
            _FID = value._FID;
            _REV = value._REV;
            _type = value._type;
            _stock = value._stock;
            _yield = value._yield;
            _location = value._location;
            _comment = value._comment;
            _destination = value._destination;
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1}", _PN, _description);
        }

        public COMPONENTTYPE ParseComponentType(string Type)
        {
            COMPONENTTYPE _type;
            switch(Type)
            {
                case "F": { _type = COMPONENTTYPE.F; break; };
                case "H": { _type = COMPONENTTYPE.H; break; };
                case "PSU": { _type = COMPONENTTYPE.PSU; break; };
                case "S": { _type = COMPONENTTYPE.S; break; };
                case "A": { _type = COMPONENTTYPE.A; break; };
                case "OP": { _type = COMPONENTTYPE.OP; break; };
                case "MB": { _type = COMPONENTTYPE.MB; break; };
                case "ENG": { _type = COMPONENTTYPE.ENG; break; };
                case "E": { _type = COMPONENTTYPE.E; break; };
                default: { _type = COMPONENTTYPE.M; break; }; 
            }
            return _type;
        }
    }
}

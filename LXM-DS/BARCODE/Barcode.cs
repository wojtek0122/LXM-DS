using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.BARCODE
{
    class Barcode
    {

        string _PN = "";
        string _location = "";
        string _login = "";
        string _def = "";
        string _mt = "";
        public void PrintLabel(string PN, string Location, string Login, string Defective, string MT)
        {
            _PN = PN;
            _location = "Zone: " + Location;
            _login = Login;
            _def = Defective;
            _mt = MT;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            printDoc.Print();
        }

        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            Image img = b.Encode(BarcodeLib.TYPE.CODE128, _PN, Color.Black, Color.White, 200, 30); //60

            _PN = "PN: " + _PN; 
            Point imgCorner = new Point(-5, 10);
            Point txtPNCorner = new Point(20, 45);//85
            Point txtLocationCorner = new Point(20, 70);//110
            Point txtLoginCorner = new Point(20, 95);
            Point txtDefective = new Point(20, 120);
            Point txtMT = new Point(20, 120);
            e.Graphics.DrawImage(img, imgCorner);
            e.Graphics.DrawString(_PN, new Font("Arial", 16), new SolidBrush(Color.Black), txtPNCorner);
            e.Graphics.DrawString(_location, new Font("Arial", 16), new SolidBrush(Color.Black), txtLocationCorner);
            e.Graphics.DrawString(_login, new Font("Arial", 16), new SolidBrush(Color.Black), txtLoginCorner);
            e.Graphics.DrawString(_def, new Font("Arial", 16), new SolidBrush(Color.Black), txtDefective);
            e.Graphics.DrawString(_mt, new Font("Arial", 16), new SolidBrush(Color.Black), txtMT);
        }
    }
}

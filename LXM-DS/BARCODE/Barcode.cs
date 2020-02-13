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
        public void PrintLabel(string PN, string Location)
        {
            _PN = PN;
            _location = "Zone: " + Location;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            printDoc.Print();
        }

        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            Image img = b.Encode(BarcodeLib.TYPE.CODE128, _PN, Color.Black, Color.White, 200, 60);

            _PN = "PN: " + _PN; 
            Point imgCorner = new Point(-5, 20);
            Point txtPNCorner = new Point(20, 85);
            Point txtLocationCorner = new Point(20, 110);
            e.Graphics.DrawImage(img, imgCorner);
            e.Graphics.DrawString(_PN, new Font("Arial", 16), new SolidBrush(Color.Black), txtPNCorner);
            e.Graphics.DrawString(_location, new Font("Arial", 16), new SolidBrush(Color.Black), txtLocationCorner);
        }
    }
}

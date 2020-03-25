﻿using System;
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
        public void PrintLabel(string PN, string Location, string Login)
        {
            _PN = PN;
            _location = "Zone: " + Location;
            _login = Login;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            printDoc.Print();
        }

        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            Image img = b.Encode(BarcodeLib.TYPE.CODE128, _PN, Color.Black, Color.White, 200, 30); //60

            _PN = "PN: " + _PN; 
            Point imgCorner = new Point(-5, 20);
            Point txtPNCorner = new Point(20, 55);//85
            Point txtLocationCorner = new Point(20, 80);//110
            Point txtLoginCorner = new Point(20, 105);
            e.Graphics.DrawImage(img, imgCorner);
            e.Graphics.DrawString(_PN, new Font("Arial", 16), new SolidBrush(Color.Black), txtPNCorner);
            e.Graphics.DrawString(_location, new Font("Arial", 16), new SolidBrush(Color.Black), txtLocationCorner);
            e.Graphics.DrawString(_login, new Font("Arial", 16), new SolidBrush(Color.Black), txtLoginCorner);
        }
    }
}

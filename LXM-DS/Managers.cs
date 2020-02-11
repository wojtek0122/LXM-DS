﻿using LXM_DS.MYSQL;
using LXM_DS.PRINTER;
using LXM_DS.USERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS
{
    class Managers
    {
        private static Managers _managers;
        PrinterManager _printerManager;
        //UserManager _userManager;
        MySQLManager _mysqlManager;
        private Managers()
        {
            _mysqlManager = MySQLManager.CreateManager();
            _printerManager = new PrinterManager();
            
            //_userManager = new UserManager();
        }

        public static Managers CreateManagers()
        {
            if(_managers == null)
            {
                _managers = new Managers();
            }
            return _managers;
        }

        public PrinterManager GetPrinterManager()
        {
            return _printerManager;
        }

        /* NOT USED IN MYSQL VERSION
         * public UserManager GetUserManager()
         *{
         *   return _userManager;
         *}
         */

        public MySQLManager GetMySQLManager()
        {
            return _mysqlManager;
        }

        public void InitializePrinters()
        {
            _printerManager.SetComponents(_mysqlManager.GetComponents());
            _printerManager.SetPrinters(_mysqlManager.GetPrinters(_printerManager.GetComponents()));
        }
    }
}

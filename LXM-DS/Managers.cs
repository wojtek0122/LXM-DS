﻿using LXM_DS.PRINTER;
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
        UserManager _userManager;
        private Managers()
        {
            _printerManager = new PrinterManager();
            _userManager = new UserManager();
        }

        public static Managers CreateManagers()
        {
            if(_managers == null)
            {
                _managers = new Managers();
            }
            return _managers;
        }

        public PrinterManager GetPrintManager()
        {
            return _printerManager;
        }

        public UserManager GetUserManager()
        {
            return _userManager;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXM_DS.BUTTON
{
    class ButtonListManager
    {
        private static ButtonListManager _buttonListManager;
        private static List<(StatusButton, int, int)> _buttonList = new List<(StatusButton BTN, int COL, int ROW)>();
        private ButtonListManager()
        {

        }
        public static ButtonListManager CreateManager()
        {
            if (_buttonListManager == null)
            {
                _buttonListManager = new ButtonListManager();
            }
            return _buttonListManager;
        }
        public void SetButtonList(List<(StatusButton, int, int)> ListOfButtons)
        {
            _buttonList = ListOfButtons;
        }

        public void AddItemToButtonList((StatusButton, int, int) ButtonToAdd)
        {
            _buttonList.Add(ButtonToAdd);
        }

        public List<(StatusButton, int, int)> GetButtonList()
        {
            return _buttonList;
        }

        public StatusButton GetStatusButton(int Index)
        {
            return _buttonList[Index].Item1;
        }
    }
}

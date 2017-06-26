/* 
* ***** BEGIN GPL LICENSE BLOCK*****

* Copyright © 2017 Pavel Silukou

* This file is part of AlienRP.

* AlienRP is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* AlienRP is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with AlienRP.  If not, see<http://www.gnu.org/licenses/>.

* ***** END GPL LICENSE BLOCK*****
*/

using System.Collections.Generic;
using System.Windows.Controls;
using AlienRP.Elements;

namespace AlienRP.Controls
{
    public partial class HotkeysControl : UserControl
    {
        private List<int> hotkeysIDList;
        private List<HotkeyItem> hotkeyItemsList = new List<HotkeyItem>();

        public HotkeysControl()
        {
            InitializeComponent();
        }

        private List<string> GetHotkeyActionsName()
        {
            List<string> actionsNameList = new List<string>();

            actionsNameList.Add(Properties.Resources.HotkeysControl_Action0);
            actionsNameList.Add(Properties.Resources.HotkeysControl_Action1);
            actionsNameList.Add(Properties.Resources.HotkeysControl_Action2);
            actionsNameList.Add(Properties.Resources.HotkeysControl_Action3);
            actionsNameList.Add(Properties.Resources.HotkeysControl_Action4);
            actionsNameList.Add(Properties.Resources.HotkeysControl_Action5);
            actionsNameList.Add(Properties.Resources.HotkeysControl_Action6);

            return actionsNameList;
        }

        public List<int> GetHotkeysIDList()
        {
            return hotkeysIDList;
        }

        public void SetNewHotkeyID(int id, HotkeyItem item)
        {
            int index = hotkeyItemsList.IndexOf(item);

            hotkeysIDList[index] = id;

            forTabulation.Focus();
        }

        public void LoadHotkeys()
        {
            if (hotkeyItemsList.Count > 0)
            {
                hotkeyItemsList.Clear();
            }
            if (hotkeysPanel.Children.Count > 0)
            {
                hotkeysPanel.Children.Clear();
            }

            this.hotkeysIDList = GlobalSettings.GetHotkeys();
            List<string> actionsNameList = GetHotkeyActionsName();

            for (int i = 0; i < actionsNameList.Count; i++)
            {
                HotkeyItem item = new HotkeyItem(actionsNameList[i], hotkeysIDList[i]);
                hotkeysPanel.Children.Add(item);
                hotkeyItemsList.Add(item);
            }
        }
    }
}

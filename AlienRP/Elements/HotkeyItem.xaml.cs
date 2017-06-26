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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AlienRP.Controls;

namespace AlienRP.Elements
{
    public partial class HotkeyItem : UserControl
    {
        private int hotkeyGlobalID;

        public HotkeyItem(string actionName, int hotkeyGlobalID)
        {
            InitializeComponent();
            this.hotkeyGlobalID = hotkeyGlobalID;
            this.hotkeyAction.Text = actionName;
            this.hotkeyGlobal.Content = Hotkeys.GetHotkeyName(hotkeyGlobalID);
        }

        private void HotkeyItemKeyDown(object sender, KeyEventArgs e)
        {
            HotkeysControl parent = this.GetParent();
            List<int> hotkeysIDList = parent.GetHotkeysIDList();

            int newKeyID = Hotkeys.GetKeyIDByKey(e.Key);

            if (hotkeysIDList.Contains(newKeyID))
            {
                return;
            }
            else
            {
                hotkeyGlobalID = newKeyID;
                hotkeyGlobal.Content = Hotkeys.GetHotkeyName(hotkeyGlobalID);
                parent.SetNewHotkeyID(hotkeyGlobalID, this);
            }


        }

        private HotkeysControl GetParent()
        {
            DependencyObject ucParent = this.Parent;

            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            return (HotkeysControl)ucParent;
        }
    }
}

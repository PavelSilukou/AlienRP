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

namespace AlienRP.Controls
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            int currentQuality = qualityControl.GetSelectedQualityButtonId();
            if (currentQuality != PlayerSettings.qualitylistId)
            {
                PlayerSettings.qualitylistId = currentQuality;
                PlayerSettings.SavePlayerSettings();
                PlayerControl.Instance.ChangeQuality();
            }

            List<int> hotkeysIDList = hotkeysControl.GetHotkeysIDList();
            GlobalSettings.SaveHotkeys(hotkeysIDList);
            GlobalHotkeyManager.UpdateHotkeys(hotkeysIDList);
        }

        public void LoadSettings()
        {
            qualityControl.LoadQualityButtons();
            hotkeysControl.LoadHotkeys();
            userSettingsControl.LoadUserSettings();
        }
    }
}

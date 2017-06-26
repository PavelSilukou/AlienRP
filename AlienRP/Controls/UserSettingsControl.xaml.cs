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

using System.Windows;
using System.Windows.Controls;
using AlienRP.Windows;

namespace AlienRP.Controls
{
    public partial class UserSettingsControl : UserControl
    {
        public UserSettingsControl()
        {
            InitializeComponent();
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Logout();
        }

        public void LoadUserSettings()
        {
            nameText.Text = MainWindow.Instance.accountText.Text;
        }
    }
}

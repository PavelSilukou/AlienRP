/* 
* ***** BEGIN GPL LICENSE BLOCK*****

* Copyright © 2018 Pavel Silukou

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

namespace AlienRP.Elements
{
    public partial class RememberAutoLoginCheckBoxes : UserControl
    {
        public RememberAutoLoginCheckBoxes()
        {
            InitializeComponent();
        }

        private void RememberMeCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            rememberMeCheckBox.Style = (Style)FindResource("DoubleCheckBoxMinorStyle");
            autoLoginCheckBox.Visibility = Visibility.Visible;
        }

        private void RememberMeCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            rememberMeCheckBox.Style = (Style)FindResource("DoubleCheckBoxMainStyle");
            autoLoginCheckBox.Visibility = Visibility.Collapsed;
            autoLoginCheckBox.IsChecked = false;
        }

        public void SetCheckStatus(bool isRememberMeChecked, bool isAutoLoginChecked)
        {
            rememberMeCheckBox.IsChecked = isRememberMeChecked;
            autoLoginCheckBox.IsChecked = isAutoLoginChecked;
        }

        public bool GetRememberMeStatus()
        {
            return rememberMeCheckBox.IsChecked.Value;
        }

        public bool GetAutoLoginStatus()
        {
            return autoLoginCheckBox.IsChecked.Value;
        }
    }
}

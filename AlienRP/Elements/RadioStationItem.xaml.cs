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

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AlienRP.Controls;

namespace AlienRP.Elements
{
    public partial class RadioStationItem : UserControl
    {
        private string name;

        public RadioStationItem(string name, string urlBack, string urlText)
        {
            InitializeComponent();
            
            this.name = name;
            this.backImage.Source = new BitmapImage(new Uri(urlBack, UriKind.Relative));
            this.textImage.Source = new BitmapImage(new Uri(urlText, UriKind.Relative));
        }

        private void OuterBorderMouseEnter(object sender, MouseEventArgs e)
        {
            PART_OpacityMask.Opacity = 1.0;
        }

        private void OuterBorderMouseLeave(object sender, MouseEventArgs e)
        {
            PART_OpacityMask.Opacity = 0.7;
        }

        private void RadioChecked(object sender, RoutedEventArgs e)
        {
            RadioStationsControl.Instance.RadioStationChecked(this.name);
        }
    }
}

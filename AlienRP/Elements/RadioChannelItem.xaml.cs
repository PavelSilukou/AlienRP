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
    public partial class RadioChannelItem : UserControl
    {
        Channel channel = null;

        public RadioChannelItem()
        {
            InitializeComponent();
        }

        private void OuterBorderMouseEnter(object sender, MouseEventArgs e)
        {
            PART_OpacityMask.Opacity = 1.0;
        }

        private void OuterBorderMouseLeave(object sender, MouseEventArgs e)
        {
            PART_OpacityMask.Opacity = 0.7;
        }

        public RadioChannelItem(Channel channel)
        {
            InitializeComponent();
            
            this.channel = channel;
            Image.Source = new BitmapImage(new Uri(channel.imageUrl));
            
            channelName.Text = channel.name;
            favoriteButton.IsChecked = channel.isFavorite;
        }

        private void ChannelChecked(object sender, RoutedEventArgs e)
        {
            RadioChannelsControl.Instance.ChannelChecked(this.channel); 
        }

        private void FavoriteButtonClick(object sender, RoutedEventArgs e)
        {
            if (favoriteButton.IsChecked == true)
            {
                try
                {
                    RadioAPI.AddFavorite(channel.id);
                }
                catch (Exception ex)
                {
                    RadioChannelsControl.Instance.ChildError(ex);
                    return;
                }
            }
            else
            {
                try
                {
                    RadioAPI.DeleteFavorite(channel.id);
                }
                catch (Exception ex)
                {
                    RadioChannelsControl.Instance.ChildError(ex);
                    return;
                }
            }
            RadioChannelsControl.Instance.LoadChannels();
        }
    }
}

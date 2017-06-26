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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Drawing;
using AlienRP.Elements;
using AlienRP.Windows;

namespace AlienRP.Controls
{
    public partial class RadioChannelsControl : UserControl
    {
        private List<Channel> channels = new List<Channel>();
        BackgroundWorker getChannelsWorker = new BackgroundWorker();
        public static RadioChannelsControl Instance { get; private set; }

        public RadioChannelsControl()
        {
            InitializeComponent();
            loadingControl.SetParameters(Properties.Resources.LoadingControl_RadioChannels, 20, 72);

            getChannelsWorker.DoWork += GetChannelsWorker;
            getChannelsWorker.RunWorkerCompleted += GetChannelsWorkerCompleted;

            Instance = this;
        }

        private void AddChannels(Channel channel)
        {
            RadioChannelItem item = new RadioChannelItem(channel);
            channelsPanel.Children.Add(item);
        }

        private void GetChannelsWorker(object sender, DoWorkEventArgs e)
        {
            e.Result = RadioAPI.GetAllChannels();
        }

        private void GetChannelsWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {   
            if (e.Error != null)
            {
                if (e.Error is MemberException)
                {
                    ChangeChildControlVisible(3);
                }
                else if (e.Error is InternetException)
                {
                    ChangeChildControlVisible(2);
                }
            }
            else
            {
                channels = (List<Channel>)e.Result;

                int width;
                TransformXToWidth(400, out width);

                foreach (Channel channel in channels)
                {
                    channel.imageUrl = channel.imageUrl + "?width=" + width;
                    AddChannels(channel);
                }
                ChangeChildControlVisible(0);
            }
        }

        public void LoadChannels()
        {
            if (channels.Count > 0)
            {
                channels.Clear();
            }
            if (channelsPanel.Children.Count > 0)
            {
                channelsPanel.Children.Clear();
            }

            ChangeChildControlVisible(1);
            
            if (!getChannelsWorker.IsBusy)
            {
                getChannelsWorker.RunWorkerAsync();
            }
        }

        public void ChannelChecked(Channel channel)
        {
            PlayerSettings.currentChannelId = channel.id;
            PlayerSettings.currentChannelKey = channel.key;
            PlayerSettings.currentChannelName = channel.name;
            PlayerSettings.SavePlayerSettings();

            PlayerControl.Instance.RadioChannelChecked();
        }

        private void ReloadButtonClick(object sender, RoutedEventArgs e)
        {
            LoadChannels();
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Logout();
        }

        private void ChangeChildControlVisible(int controlId)
        {
            if (controlId == 0)
            {
                channelsGrid.Visibility = Visibility.Visible;
                loadingGrid.Visibility = Visibility.Collapsed;
                internetErrorGrid.Visibility = Visibility.Collapsed;
                memberErrorGrid.Visibility = Visibility.Collapsed;
            }
            else if (controlId == 1)
            {
                channelsGrid.Visibility = Visibility.Collapsed;
                loadingGrid.Visibility = Visibility.Visible;
                internetErrorGrid.Visibility = Visibility.Collapsed;
                memberErrorGrid.Visibility = Visibility.Collapsed;
            }
            else if (controlId == 2)
            {
                channelsGrid.Visibility = Visibility.Collapsed;
                loadingGrid.Visibility = Visibility.Collapsed;
                internetErrorGrid.Visibility = Visibility.Visible;
                memberErrorGrid.Visibility = Visibility.Collapsed;
            }
            else if (controlId == 3)
            {
                channelsGrid.Visibility = Visibility.Collapsed;
                loadingGrid.Visibility = Visibility.Collapsed;
                internetErrorGrid.Visibility = Visibility.Collapsed;
                memberErrorGrid.Visibility = Visibility.Visible;
            }
        }

        public void ChildError(Exception ex)
        {
            if (ex is InternetException)
            {
                ChangeChildControlVisible(2);
            }
            else if (ex is MemberException)
            {
                ChangeChildControlVisible(3);
            }
        }

        public void TransformToPixels(double unitX,
                              double unitY,
                              out int pixelX,
                              out int pixelY)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                pixelX = (int)((g.DpiX / 96) * unitX);
                pixelY = (int)((g.DpiY / 96) * unitY);
            }
        }

        public void TransformXToWidth(double X, out int width)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                width = (int)((g.DpiX / 96) * X);
            }
        }
    }
}

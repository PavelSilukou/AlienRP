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
using System.Windows.Threading;

namespace AlienRP.Elements
{
    public partial class TrackBufferingElement : UserControl
    {
        DispatcherTimer bufferingTimer;
        MediaElement targetPlayer;

        public BufferStopEvent BufferStop;

        public TrackBufferingElement()
        {
            InitializeComponent();

            bufferingTimer = new System.Windows.Threading.DispatcherTimer();
            bufferingTimer.Tick += BufferingTick;
            bufferingTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }

        private void BufferingTick(object sender, EventArgs e)
        {
            if (targetPlayer.BufferingProgress < 1 && targetPlayer.DownloadProgress == 0)
            {
                bufferingText.Text = Properties.Resources.PlayerControl_Buffering + (targetPlayer.BufferingProgress * 100).ToString("F0") + "%";
            }
            else if (targetPlayer.BufferingProgress == 1 && targetPlayer.DownloadProgress == 0)
            {
                Stop();
                BufferStop();
            }
        }

        public void Start(MediaElement player)
        {
            targetPlayer = player;
            this.Visibility = Visibility.Visible;
            bufferingText.Text = Properties.Resources.PlayerControl_Buffering;
            bufferingTimer.Start();
        }

        public void Stop()
        {
            this.Visibility = Visibility.Collapsed;
            bufferingTimer.Stop();
        }
    }

    public delegate void BufferStopEvent();
}

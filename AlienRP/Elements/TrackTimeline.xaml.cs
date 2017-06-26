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
    public partial class TrackTimeline : UserControl
    {
        DispatcherTimer trackTimelineTimer;
        private double trackTimeTicks = 0;
        private double finishTrackTimeTicks = 0;

        public TimerCompletedEvent TimerCompleted;

        public TrackTimeline()
        {
            InitializeComponent();

            trackTimelineTimer = new System.Windows.Threading.DispatcherTimer();
            trackTimelineTimer.Tick += TrackTimeTick;
            trackTimelineTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void TrackTimeTick(object sender, EventArgs e)
        {
            if (trackTimeTicks < finishTrackTimeTicks)
            {
                trackTime.Text = GetFormattedTime();
                trackTimeTicks += 1;
            }
            else
            {
                TimerCompleted();
                trackTimelineTimer.Stop();
            }
        }

        private string TimeSpanToString(double time)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            int minutes = timeSpan.Minutes + timeSpan.Hours * 60;
            return minutes.ToString() + ":" + timeSpan.ToString("ss");
        }

        private string GetFormattedTime()
        {
            return TimeSpanToString(trackTimeTicks) + " / " + TimeSpanToString(finishTrackTimeTicks);
        }

        public void Start(double startTime, double finishTime)
        {
            this.trackTimeTicks = startTime;
            this.finishTrackTimeTicks = finishTime;
            this.trackTimelineTimer.Start();
            trackTime.Text = GetFormattedTime();
            this.Visibility = Visibility.Visible;
        }

        public void Stop()
        {
            this.Visibility = Visibility.Collapsed;
            this.trackTimelineTimer.Stop();
        }
    }

    public delegate void TimerCompletedEvent();
}

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
using System.Windows.Media;
using System.ComponentModel;
using AlienRP.Controls;

namespace AlienRP.Elements
{
    public partial class VoteButtonsElement : UserControl
    {
        private string currentTrackId;
        public VoteButtonsErrorEvent VoteButtonsError;
        BackgroundWorker getVotesWorker;

        public VoteButtonsElement()
        {
            InitializeComponent();

            getVotesWorker = new BackgroundWorker();

            getVotesWorker.DoWork += GetVoteWorker;
            getVotesWorker.RunWorkerCompleted += GetVoteWorkerCompleted;
        }

        private void SetVoteCount(int up, int down)
        {
            if (up < 1000)
            {
                voteUpCount.Text = "" + up;
            }
            else if (up < 10000)
            {
                voteUpCount.Text = String.Format("{0:0.00}", (up * 1.0 / 1000)) + "k";
            }
            else
            {
                voteUpCount.Text = String.Format("{0:0.0}", (up * 1.0 / 1000)) + "k";
            }

            if (down < 1000)
            {
                voteDownCount.Text = "" + down;
            }
            else if (down < 10000)
            {
                voteDownCount.Text = String.Format("{0:0.00}", (down * 1.0 / 1000)) + "k";
            }
            else
            {
                voteDownCount.Text = String.Format("{0:0.0}", (down * 1.0 / 1000)) + "k";
            }
        }

        public void Disable()
        {
            voteUpButton.IsEnabled = false;
            voteDownButton.IsEnabled = false;
            voteUpCount.Foreground = (Brush)Application.Current.Resources["MainColor1"];
            voteDownCount.Foreground = (Brush)Application.Current.Resources["MainColor1"];
            voteUpCount.Text = "0";
            voteDownCount.Text = "0";

            GlobalHotkeyManager.IsEnableVoteHotkeys(false);
        }

        public void Enable()
        {
            Track track = PlayerControl.Instance.currentTrack;
            SetVoteCount(track.upVote, track.downVote);
            this.currentTrackId = track.id;

            if (!getVotesWorker.IsBusy)
            {
                getVotesWorker.RunWorkerAsync();
            }
        }

        private void VoteUpButtonClick(object sender, RoutedEventArgs e)
        {
            if (voteUpButton.IsChecked == true)
            {
                if (voteDownButton.IsChecked == true)
                {
                    voteDownButton.IsChecked = false;
                }

                VoteUp();
            }
            else
            {
                VoteDelete();
            }
        }

        private void VoteDownButtonClick(object sender, RoutedEventArgs e)
        {
            if (voteDownButton.IsChecked == true)
            {
                if (voteUpButton.IsChecked == true)
                {
                    voteUpButton.IsChecked = false;
                }

                VoteDown();
            }
            else
            {
                VoteDelete();
            }
        }

        public void OnVoteChanged(int state)
        {
            switch (state)
            {
                case -1:
                    {
                        if (voteDownButton.IsChecked == false)
                        {
                            if (voteUpButton.IsChecked == true)
                            {
                                voteUpButton.IsChecked = false;
                            }

                            VoteDown();
                            voteDownButton.IsChecked = true;
                        }
                        break;
                    }
                case 0:
                    {
                        if (voteUpButton.IsChecked == true || voteDownButton.IsChecked == true)
                        {
                            VoteDelete();
                            voteDownButton.IsChecked = false;
                            voteUpButton.IsChecked = false;
                        }
                        break;
                    }
                case 1:
                    {
                        if (voteUpButton.IsChecked == false)
                        {
                            if (voteDownButton.IsChecked == true)
                            {
                                voteDownButton.IsChecked = false;
                            }

                            VoteUp();
                            voteUpButton.IsChecked = true;
                        }
                        break;
                    }
            }
        }

        private void VoteUp()
        {
            VotesCount count;
            try
            {
                count = RadioAPI.VoteUp(currentTrackId);
            }
            catch (MemberException)
            {
                VoteButtonsError(1);
                return;
            }
            catch (InternetException)
            {
                VoteButtonsError(0);
                return;
            }

            SetVoteCount(count.up, count.down);
        }

        private void VoteDown()
        {
            VotesCount count;
            try
            {
                count = RadioAPI.VoteDown(currentTrackId);
            }
            catch (MemberException)
            {
                VoteButtonsError(1);
                return;
            }
            catch (InternetException)
            {
                VoteButtonsError(0);
                return;
            }

            SetVoteCount(count.up, count.down);
        }

        private void VoteDelete()
        {
            VotesCount count;

            try
            {
                count = RadioAPI.DeleteVote(currentTrackId);
            }
            catch (MemberException)
            {
                VoteButtonsError(1);
                return;
            }
            catch (InternetException)
            {
                VoteButtonsError(0);
                return;
            }

            SetVoteCount(count.up, count.down);
        }

        private void GetVoteWorker(object sender, DoWorkEventArgs e)
        {
            e.Result = RadioAPI.GetVote(currentTrackId);
        }

        private void GetVoteWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is MemberException)
                {
                    VoteButtonsError(1);
                }
                else if (e.Error is InternetException)
                {
                    VoteButtonsError(0);
                }
            }
            else
            {
                Vote currentTrackVote = (Vote)e.Result;

                voteUpButton.IsChecked = currentTrackVote.isUp;
                voteDownButton.IsChecked = currentTrackVote.isDown;

                voteUpButton.IsEnabled = true;
                voteDownButton.IsEnabled = true;
                voteUpCount.Foreground = (Brush)Application.Current.Resources["LightFontColor1"];
                voteDownCount.Foreground = (Brush)Application.Current.Resources["LightFontColor1"];

                GlobalHotkeyManager.IsEnableVoteHotkeys(true);
            }
        }

        private void VoteButtonsElementIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled == true)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }
    }

    public delegate void VoteButtonsErrorEvent(int state);
}

/* 
* ***** BEGIN GPL LICENSE BLOCK*****

* Copyright © 2017-2018 Pavel Silukou

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
using System.ComponentModel;
using System.Threading;

namespace AlienRP.Controls
{
    public partial class PlayerControl : UserControl
    {
        public static PlayerControl Instance { get; private set; }
        
        public Track currentTrack = new Track();
        public MediaElement player = new MediaElement();
        private BackgroundWorker getTrackInfoWorker = new BackgroundWorker();
        private BackgroundWorker getAudioLinksWorker = new BackgroundWorker();
        
        public PlayerControl()
        {
            InitializeComponent();

            player.LoadedBehavior = MediaState.Manual;
            player.UnloadedBehavior = MediaState.Manual;
            player.MediaFailed += PlayerMediaFailed;
            
            bufferingText.BufferStop += PlayerStopBuffering;

            trackTimeline.TimerCompleted += GetNextTrack;
            voteButtons.VoteButtonsError += ShowError;

            getTrackInfoWorker.DoWork += GetTrackInfoWorker;
            getTrackInfoWorker.RunWorkerCompleted += GetTrackInfoWorkerCompleted;
            getTrackInfoWorker.WorkerSupportsCancellation = true;

            getAudioLinksWorker.DoWork += GetAudioLinksWorker;
            getAudioLinksWorker.RunWorkerCompleted += GetAudioLinksWorkerCompleted;
            getAudioLinksWorker.WorkerSupportsCancellation = true;
            
            soundBar.Value = GlobalSettings.GetVolumeLevel();
            
            SetQuality();
            
            ChangePlayerState(1);
            
            Instance = this;
        }

        private void GetNextTrack()
        {
            voteButtons.IsEnabled = false;
            ShowInfo(0);

            if (!getTrackInfoWorker.IsBusy)
            {
                getTrackInfoWorker.RunWorkerAsync();
            }
        }

        public void RadioStationChanged()
        {
            if (playButton.IsChecked == true)
            {
                Stop();
            }

            SetQuality();

            ChangePlayerState(1);
        }

        public void RadioChannelChecked()
        {
            if (playButton.IsChecked == true)
            {
                Stop();
            }
            Play();
        }

        public void TryAgain()
        {
            Play();
        }

        private void SetQuality()
        {
            Quality quality = RadioAPI.GetQuality();
            qualityText.Text = quality.format + " " + quality.bitrate;
            currentQualityInfoText.Text = quality.format + " " + quality.bitrate;
        }

        public void ChangeQuality()
        {
            SetQuality();

            if (playButton.IsChecked == true)
            {
                Stop();
                Play();
            }
        }
        
        public void OnPlay()
        {
            if (playButton.IsChecked == true)
            {
                Stop();
            }
            else
            {
                Play();
            }
        }

        public void OnVolumeChanged(int state)
        {
            switch (state)
            {
                case -1:
                    {
                        soundBar.Value -= 0.1f;
                        break;
                    }
                case 0:
                    {
                        soundBar.Value = 0;
                        break;
                    }
                case 1:
                    {
                        soundBar.Value += 0.1f;
                        break;
                    }
            }
        }

        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            if (playButton.IsChecked == true)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }

        private void Play()
        {
            playButton.IsChecked = true;
            ShowInfo(1);
            
            if (!getAudioLinksWorker.IsBusy)
            {
                getAudioLinksWorker.RunWorkerAsync();
            }
        }

        private void Stop()
        {
            playButton.IsChecked = false;
            
            if (getAudioLinksWorker.IsBusy)
            {
                getAudioLinksWorker.CancelAsync();
            }
            if (getAudioLinksWorker.IsBusy)
            {
                getTrackInfoWorker.CancelAsync();
            }
            
            voteButtons.IsEnabled = false;
            
            player.Pause();
            player.Source = null;
            currentTrack = new Track();
            
            bufferingText.Stop();
            trackTimeline.Stop();
            
            ChangePlayerState(1);
        }

        private void GetAudioLinksWorker(object sender, DoWorkEventArgs e)
        {
            RadioAPI.Ping();
            if (getAudioLinksWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            RadioAPI.GetAudioLinks();
            if (getAudioLinksWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void GetAudioLinksWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            if (e.Error != null)
            {
                if (e.Error is StreamlistException)
                {
                    ShowError(2);
                }
                else if (e.Error is ChannelException)
                {
                    ShowError(3);
                }
                else if (e.Error is MemberException)
                {
                    ShowError(1);
                }
                else if (e.Error is InternetException)
                {
                    ShowError(0);
                }
            }
            else
            {
                player.Source = new Uri(PlayerSettings.audioStream);
                player.Play();
            if (!getTrackInfoWorker.IsBusy)
                {
                    getTrackInfoWorker.RunWorkerAsync();
                }
            }
        }

        private void GetTrackInfoWorker(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
            if (getTrackInfoWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            Track newTrack = RadioAPI.GetNowPlayingTrack();
                if (!newTrack.id.Equals(currentTrack.id))
                {
                    e.Result = newTrack;
                    return;
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }

            e.Cancel = true;

            if (getTrackInfoWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void GetTrackInfoWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is InternetException)
                {
                    ShowError(0);
                }
                else if (e.Error is BadRequestException)
                {
                    ShowError(4);
                }
            }
            else
            {
                if (e.Cancelled)
                {
                    return;
                }

                currentTrack = (Track)e.Result;

                ChangePlayerState(3);
                bufferingText.Start(player);
            }
        }
        
        private void soundBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = soundBar.Value;
        }

        private void PlayerMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Stop();
            Play();
        }

        private void PlayerStopBuffering()
        {
            trackTimeline.Start((RadioAPI.ConvertToUnixTimestamp(DateTime.UtcNow) - PlayerSettings.timeOffset) - currentTrack.startTime, currentTrack.duration);
        }

        private void ChangePlayerState(int state)
        {
            switch (state)
            {
                case 1:
                    {
                        currentStationInfoText.Text = RadioAPI.GetRadioStationName();
                        currentChannelInfoText.Text = PlayerSettings.currentChannelName;

                        currentChannelInfoGrid.Visibility = Visibility.Visible;
                        trackInfoGrid.Visibility = Visibility.Collapsed;
                        infoGrid.Visibility = Visibility.Collapsed;

                        if (PlayerSettings.currentChannelName.Equals(""))
                        {
                            currentChannelInfoTextSplitter.Visibility = Visibility.Collapsed;
                            currentChannelInfoText.Visibility = Visibility.Collapsed;
                            playerControlGrid.Visibility = Visibility.Collapsed;

                            GlobalHotkeyManager.IsEnableAllHotkeys(false);
                        }
                        else
                        {
                            currentChannelInfoTextSplitter.Visibility = Visibility.Visible;
                            currentChannelInfoText.Visibility = Visibility.Visible;
                            playerControlGrid.Visibility = Visibility.Visible;

                            GlobalHotkeyManager.IsEnableAllHotkeys(true);
                        }

                        break;
                    }
                case 2:
                    {
                        currentChannelInfoGrid.Visibility = Visibility.Collapsed;
                        trackInfoGrid.Visibility = Visibility.Collapsed;
                        infoGrid.Visibility = Visibility.Visible;
                        playerControlGrid.Visibility = Visibility.Visible;

                        GlobalHotkeyManager.IsEnableAllHotkeys(true);

                        break;
                    }
                case 3:
                    {
                        trackName.SetText(currentTrack.trackName);
                        artistName.SetText(currentTrack.artistName);
                        channelName.Text = PlayerSettings.currentChannelName;
                        voteButtons.IsEnabled = true;

                        currentChannelInfoGrid.Visibility = Visibility.Collapsed;
                        trackInfoGrid.Visibility = Visibility.Visible;
                        infoGrid.Visibility = Visibility.Collapsed;
                        playerControlGrid.Visibility = Visibility.Visible;

                        GlobalHotkeyManager.IsEnableAllHotkeys(true);

                        break;
                    }
            }
        }

        private void ShowInfo(int state)
        {
            ChangePlayerState(2);

            switch (state)
            {
                case 0:
                    {
                        infoText.Text = Properties.Resources.PlayerControl_GetNewTrack;
                        break;
                    }
                case 1:
                    {
                        infoText.Text = Properties.Resources.PlayerControl_GetTrack;
                        break;
                    }
            }
        }

        private void ShowError(int state)
        {
            Stop();
            errorGrid.SetErrorState(state);
        }

        private void SoundBarDragCompleted(object sender, RoutedEventArgs e)
        {
            GlobalSettings.SaveVolumeLevel(soundBar.Value);
        }

        public void LogoutAction()
        {
            Stop();
        }
    }
}

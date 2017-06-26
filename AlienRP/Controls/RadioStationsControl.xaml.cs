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
using AlienRP.Elements;

namespace AlienRP.Controls
{
    public partial class RadioStationsControl : UserControl
    {
        public static RadioStationsControl Instance { get; private set; }

        public RadioStationsControl()
        {
            InitializeComponent();
            Instance = this;
        }
        
        public void LoadRadioStations()
        {
            string resourcePath = "/Resources/images/";

            radioStationsPanel.Children.Add(new RadioStationItem("di", resourcePath + "di_back.png", resourcePath + "di_text.png"));
            radioStationsPanel.Children.Add(new RadioStationItem("rockradio", resourcePath + "rock_back.png", resourcePath + "rock_text.png"));
            radioStationsPanel.Children.Add(new RadioStationItem("radiotunes", resourcePath + "tunes_back.png", resourcePath + "tunes_text.png"));
            radioStationsPanel.Children.Add(new RadioStationItem("jazzradio", resourcePath + "jazz_back.png", resourcePath + "jazz_text.png"));
            radioStationsPanel.Children.Add(new RadioStationItem("classicalradio", resourcePath + "classical_back.png", resourcePath + "classical_text.png"));
        }

        public void ResizeControl()
        {
            double actualHeight = this.ActualHeight;
            double radioItemHeight = actualHeight / 5;

            if (radioItemHeight >= 100 && radioItemHeight <= 200)
            {
                this.radioStationsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            else
            {
                this.radioStationsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            
            foreach (UIElement child in radioStationsPanel.Children)
            {
                RadioStationItem radioChannel = (RadioStationItem)child;
                if (radioItemHeight < 100)
                {
                    radioChannel.outerBorder.Height = 100;
                }
                else if (radioItemHeight > 200)
                {
                    radioChannel.outerBorder.Height = 200;
                }
                else
                {
                    radioChannel.outerBorder.Height = radioItemHeight;
                }
            }
        }

        public void RadioStationChecked(string radioName)
        {
            RadioAPI.SetRadioStation(radioName);
            PlayerSettings.ClearChannelInfo();
            PlayerControl.Instance.RadioStationChanged();
        }

        private void RadioStationsControlLoaded(object sender, RoutedEventArgs e)
        {
            LoadRadioStations();
            ResizeControl();
        }

        private void RadioStationsControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeControl();
        }
    }
}

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

namespace AlienRP.Controls
{
    public partial class QualityButtonsControl : UserControl
    {
        private RadioButton[] qualityButtons;
        //private string mp3Icon = "";
        //private string aacIcon = "";

        public QualityButtonsControl()
        {
            InitializeComponent();
        }

        private void CreateQualityButtons()
        {
            Quality[] qualityList = RadioAPI.GetQualityList();
            qualityButtons = new RadioButton[qualityList.Length];
            for (int i = 0; i < qualityList.Length; i++)
            {
                RadioButton qualityButton = new RadioButton();
                qualityButton.Style = (Style)Application.Current.Resources["QualityButtonStyle"];
                qualityButton.GroupName = "QualityButtons";
                qualityButton.Content = qualityList[i].bitrate;
                switch (qualityList[i].format)
                {
                    case "MP3":
                        {
                            //qualityButton.Tag = mp3Icon;
                            qualityButton.Tag = "MP3";
                            break;
                        }
                    case "AAC":
                        {
                            //qualityButton.Tag = aacIcon;
                            qualityButton.Tag = "AAC";
                            break;
                        }
                }
                this.qualityButtonsGrid.Children.Add(qualityButton);
                Grid.SetColumn(qualityButton, i);
                qualityButtons[i] = qualityButton;
            }
        }

        public int GetSelectedQualityButtonId()
        {
            for (int i = 0; i < qualityButtons.Length; i++)
            {
                if (qualityButtons[i].IsChecked == true)
                {
                    return i;
                }
            }

            return 2;
        }

        public void LoadQualityButtons()
        {
            CreateQualityButtons();

            int qualityId = PlayerSettings.qualitylistId;
            qualityButtons[qualityId].IsChecked = true;
        }
    }
}

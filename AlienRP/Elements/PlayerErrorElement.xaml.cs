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
using AlienRP.Controls;
using AlienRP.Windows;

namespace AlienRP.Elements
{
    public partial class PlayerErrorElement : UserControl
    {
        public PlayerErrorElement()
        {
            InitializeComponent();
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Logout();
        }

        private void TryButtonClick(object sender, RoutedEventArgs e)
        {
            PlayerControl.Instance.TryAgain();
            this.Visibility = Visibility.Collapsed;
        }

        private void ReloadButtonClick(object sender, RoutedEventArgs e)
        {
            RadioChannelsControl.Instance.LoadChannels();
            this.Visibility = Visibility.Collapsed;
        }

        public void SetErrorState(int state)
        {
            switch (state)
            {
                case 0:
                    {
                        errorText.Text = Properties.Resources.PlayerControl_NoInternet_Text;
                        logoutButton.Visibility = Visibility.Collapsed;
                        reloadButton.Visibility = Visibility.Collapsed;
                        tryButton.Visibility = Visibility.Visible;

                        break;
                    }
                case 1:
                    {
                        errorText.Text = Properties.Resources.PlayerControl_MemberError;
                        logoutButton.Visibility = Visibility.Visible;
                        reloadButton.Visibility = Visibility.Collapsed;
                        tryButton.Visibility = Visibility.Visible;

                        break;
                    }
                case 2:
                    {
                        errorText.Text = Properties.Resources.PlayerControl_StreamlistError;
                        logoutButton.Visibility = Visibility.Collapsed;
                        reloadButton.Visibility = Visibility.Collapsed;
                        tryButton.Visibility = Visibility.Visible;

                        break;
                    }
                case 3:
                    {
                        errorText.Text = Properties.Resources.PlayerControl_ChannelError;
                        logoutButton.Visibility = Visibility.Collapsed;
                        reloadButton.Visibility = Visibility.Visible;
                        tryButton.Visibility = Visibility.Visible;

                        break;
                    }
                case 4:
                    {
                        errorText.Text = Properties.Resources.PlayerControl_BadRequestError;
                        logoutButton.Visibility = Visibility.Collapsed;
                        reloadButton.Visibility = Visibility.Collapsed;
                        tryButton.Visibility = Visibility.Visible;

                        break;
                    }
            }

            this.Visibility = Visibility.Visible;
        }
    }
}

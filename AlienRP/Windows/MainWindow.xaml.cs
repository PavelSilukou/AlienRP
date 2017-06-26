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
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace AlienRP.Windows
{
    public partial class MainWindow : Window
    {
        private bool isCollapsed = false;
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            
            accountText.Text = User.firstName + " " + User.lastName;
            accountText.Visibility = Visibility.Visible;

            isCollapsed = GlobalSettings.GetWindowCollapsedStatus();
            WindowPosition windowPosition = GlobalSettings.GetWindowPosition();

            if (windowPosition.height == 0)
            {
                WindowPosition initialWindowPosition = new WindowPosition();
                Rect desktopWorkingArea = System.Windows.SystemParameters.WorkArea;

                this.MinHeight = 600;

                this.Height = desktopWorkingArea.Height;
                this.Left = desktopWorkingArea.Right - this.Width;
                this.Top = desktopWorkingArea.Bottom - this.Height;

                initialWindowPosition.height = this.Height;
                initialWindowPosition.left = this.Left;
                initialWindowPosition.top = this.Top;
                initialWindowPosition.width = this.Width;

                GlobalSettings.SaveWindowPosition(initialWindowPosition);
                GlobalSettings.SaveWindowCollapsedPosition(initialWindowPosition);
            }
            else
            {
                if (!isCollapsed)
                {
                    MainWindowName.MinHeight = 600;

                    this.Top = windowPosition.top;
                    this.Left = windowPosition.left;
                    this.Width = windowPosition.width;
                    this.Height = windowPosition.height;

                    collapseButton.IsChecked = false;
                }
                else
                {
                    menuGrid.Visibility = Visibility.Collapsed;
                    topSizeGrip.Visibility = Visibility.Collapsed;
                    bottomSizeGrip.Visibility = Visibility.Collapsed;

                    MainWindowName.MinHeight = 1;

                    WindowPosition collapseWindowPosition = GlobalSettings.GetWindowCollapsedPosition();
                    this.Top = collapseWindowPosition.top;
                    this.Left = collapseWindowPosition.left;
                    this.Height = collapseWindowPosition.height;

                    collapseButton.IsChecked = true;
                }
            }

            MainWindow.Instance = this;
        }

        bool ResizeInProcess = false;
        private void ResizeInit(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = true;
                senderRect.CaptureMouse();
            }
        }

        private void ResizeEnd(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = false; ;
                senderRect.ReleaseMouseCapture();
            }
        }

        private void ResizeingForm(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess)
            {
                Rectangle senderRect = sender as Rectangle;
                Window mainWindow = senderRect.Tag as Window;
                if (senderRect != null)
                {
                    double width = e.GetPosition(mainWindow).X;
                    double height = e.GetPosition(mainWindow).Y;

                    Point pointToWindow = Mouse.GetPosition(this);
                    Point pointToScreen = PointToScreen(pointToWindow);

                    double screenTop = System.Windows.SystemParameters.WorkArea.Top;
                    double screenBottom = System.Windows.SystemParameters.WorkArea.Bottom;

                    senderRect.CaptureMouse();
                    if (senderRect.Name.ToLower().Contains("right"))
                    {
                        width += 5;
                        if (width > 0)
                            mainWindow.Width = width;
                    }
                    if (senderRect.Name.ToLower().Contains("left"))
                    {
                        if (mainWindow.Width - width >= mainWindow.MinWidth)
                        {
                            width -= 5;
                            mainWindow.Left += width;
                            width = mainWindow.Width - width;
                            if (width > 0)
                            {
                                mainWindow.Width = width;
                            }
                        }
                    }

                    bool isContinue = false;

                    if (senderRect.Name.ToLower().Contains("bottom"))
                    {
                        if (pointToScreen.Y > screenBottom - 10)
                        {
                            mainWindow.Height += screenBottom - (mainWindow.Top + mainWindow.Height);
                            isContinue = true;
                        }
                        if (!isContinue)
                        {
                            if (height > mainWindow.MinHeight)
                            {
                                mainWindow.Height = height;
                            }
                            else
                            {
                                mainWindow.Height = mainWindow.MinHeight;
                            }
                        }
                    }

                    isContinue = false;
                    if (senderRect.Name.ToLower().Contains("top"))
                    {
                        if (pointToScreen.Y < screenTop + 10)
                        {
                            double tempHeight = mainWindow.Top + mainWindow.Height - screenTop;
                            mainWindow.Top = screenTop;
                            mainWindow.Height = tempHeight;
                            isContinue = true;
                        }
                        if (!isContinue)
                        {
                            if (mainWindow.Height - height > mainWindow.MinHeight)
                            {
                                mainWindow.Top += height;
                                height = mainWindow.Height - height;
                                mainWindow.Height = height;
                            }
                            else
                            {
                                mainWindow.Top += mainWindow.Height - mainWindow.MinHeight;
                                mainWindow.Height = mainWindow.MinHeight;
                            }
                        }
                    }
                    if (!isCollapsed)
                        GlobalSettings.SaveWindowPosition(new WindowPosition(mainWindow.Top, mainWindow.Left, mainWindow.Width, mainWindow.Height));
                    else
                        GlobalSettings.SaveWindowCollapsedPosition(new WindowPosition(mainWindow.Top, mainWindow.Left));
                }
            }
        }

        private void DragRectangleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

            double screenLeft = System.Windows.SystemParameters.WorkArea.Left;
            double screenRight = System.Windows.SystemParameters.WorkArea.Right;
            double screenTop = System.Windows.SystemParameters.WorkArea.Top;
            double screenBottom = System.Windows.SystemParameters.WorkArea.Bottom;

            if (MainWindowName.Top + MainWindowName.Height > screenBottom - 10)
            {
                MainWindowName.Top = screenBottom - MainWindowName.Height;
            }

            if (MainWindowName.Top < screenTop + 10)
            {
                MainWindowName.Top = screenTop;
            }

            if (MainWindowName.Left < screenLeft + 10)
            {
                MainWindowName.Left = screenLeft;
            }

            if (MainWindowName.Left + MainWindowName.Width > screenRight - 10)
            {
                MainWindowName.Left = screenRight - MainWindowName.Width;
            }

            
            if (!isCollapsed)
                GlobalSettings.SaveWindowPosition(new WindowPosition(MainWindowName.Top, MainWindowName.Left, MainWindowName.Width, MainWindowName.Height));
            else
                GlobalSettings.SaveWindowCollapsedPosition(new WindowPosition(MainWindowName.Top, MainWindowName.Left));
        }

        private void DragRectangleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void MinimizeToTrayButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void CollapseButtonClick(object sender, RoutedEventArgs e)
        {
            if (collapseButton.IsChecked == true)
            {
                menuGrid.Visibility = Visibility.Collapsed;
                topSizeGrip.Visibility = Visibility.Collapsed;
                bottomSizeGrip.Visibility = Visibility.Collapsed;

                MainWindowName.MinHeight = 1;

                WindowPosition windowPosition = GlobalSettings.GetWindowCollapsedPosition();
                MainWindowName.Top = windowPosition.top;
                MainWindowName.Left = windowPosition.left;
                MainWindowName.Height = windowPosition.height;

                isCollapsed = true;
                GlobalSettings.SaveWindowCollapsedStatus(isCollapsed);
            }
            else
            {
                menuGrid.Visibility = Visibility.Visible;
                topSizeGrip.Visibility = Visibility.Visible;
                bottomSizeGrip.Visibility = Visibility.Visible;
                
                MainWindowName.MinHeight = 600;

                WindowPosition windowPosition = GlobalSettings.GetWindowPosition();
                MainWindowName.Top = windowPosition.top;
                MainWindowName.Left = windowPosition.left;
                MainWindowName.Width = windowPosition.width;
                MainWindowName.Height = windowPosition.height;

                isCollapsed = false;
                GlobalSettings.SaveWindowCollapsedStatus(isCollapsed);
            }
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainControlClosed(object sender, EventArgs e)
        {
            GlobalHotkeyManager.RemoveAllHotkeys();
        }

        public void Logout()
        {
            playerControl.LogoutAction();
            PlayerSettings.ClearChannelInfo();

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void RadioStationsTabItemSelected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                radioStationsControl.LoadRadioStations();
            }
        }

        private void RadioChannelsTabItemSelected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                radioChannelsControl.LoadChannels();
            }
        }

        private void SettingsTabItemSelected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                settingsControl.LoadSettings();
            }
        }
    }
}

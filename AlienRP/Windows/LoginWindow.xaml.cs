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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace AlienRP.Windows
{
    public partial class LoginWindow : Window
    {
        BackgroundWorker loginWorker = new BackgroundWorker();

        public LoginWindow()
        {
            InitializeComponent();

            InitBasicLoginWindow();

            if (autoLoginCheckBox.IsChecked.Value)
            {
                InitAutoLoginWindow();
            }
            else
            {
                InitManualLoginWindow();
            }
        }

        private void InitBasicLoginWindow()
        {
            loginWorker.DoWork += LoginWorker;
            loginWorker.RunWorkerCompleted += LoginWorkerCompleted;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            LoginData loginData = GlobalSettings.GetLoginData();
            autoLoginCheckBox.IsChecked = loginData.autoLogin;
            emailTextBox.Text = loginData.email;
            passwordTextBox.Password = loginData.password;
        }

        public LoginWindow(bool isManual)
        {
            InitializeComponent();

            InitBasicLoginWindow();
            InitManualLoginWindow();
        }

        private void InitManualLoginWindow()
        {
            manualLogin.Visibility = Visibility.Visible;
            autoLogin.Visibility = Visibility.Collapsed;

            loginButtonLoading.SetParameters(36);
            collapseButton.Visibility = Visibility.Visible;

            LoginWindowName.Width = 360;
        }

        private void InitAutoLoginWindow()
        {
            manualLogin.Visibility = Visibility.Collapsed;
            autoLogin.Visibility = Visibility.Visible;

            autoLoginLoading.SetParameters(72);
            collapseButton.Visibility = Visibility.Collapsed;

            LoginWindowName.Width = 250;

            AutoLogin();
        }

        private void AutoLogin()
        {
            object[] parameters = new object[] { emailTextBox.Text, passwordTextBox.Password };
            if (!loginWorker.IsBusy)
            {
                loginWorker.RunWorkerAsync(parameters);
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            emailTextBox.Tag = "";
            errorLoginLabel1.Visibility = Visibility.Collapsed;
            errorLoginLabel2.Visibility = Visibility.Collapsed;

            passwordTextBox.Tag = "";
            errorPasswordLabel.Visibility = Visibility.Collapsed;

            generalErrorLabel1.Visibility = Visibility.Collapsed;
            generalErrorLabel2.Visibility = Visibility.Collapsed;
            generalErrorLabel3.Visibility = Visibility.Collapsed;

            if (emailTextBox.Text.Equals(""))
            {
                emailTextBox.Tag = "Error";
                errorLoginLabel1.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                bool isEmail = Regex.IsMatch(emailTextBox.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isEmail)
                {
                    emailTextBox.Tag = "Error";
                    errorLoginLabel2.Visibility = Visibility.Visible;
                    return;
                }
            }

            if (passwordTextBox.Password.Equals(""))
            {
                passwordTextBox.Tag = "Error";
                errorPasswordLabel.Visibility = Visibility.Visible;
                return;
            }

            IsEnabledControls(false);
            loginButton.Visibility = Visibility.Hidden;
            loginButtonLoading.Visibility = Visibility.Visible;

            object[] parameters = new object[] { emailTextBox.Text, passwordTextBox.Password };
            if (!loginWorker.IsBusy)
            {
                loginWorker.RunWorkerAsync(parameters);
            }
        }

        private void IsEnabledControls(bool isEnable)
        {
            emailTextBox.IsEnabled = isEnable;
            passwordTextBox.IsEnabled = isEnable;
            autoLoginCheckBox.IsEnabled = isEnable;
            socialLinkButton.IsEnabled = isEnable;
            forgotLinkButton.IsEnabled = isEnable;
        }

        private void LoginWorker(object sender, DoWorkEventArgs e)
        {
            object[] parameters = e.Argument as object[];
            RadioAPI.Login((string)parameters[0], (string)parameters[1]);
        }

        private void LoginWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                InitManualLoginWindow();

                if (e.Error is InternetException)
                {
                    generalErrorLabel1.Visibility = Visibility.Visible;
                }

                if (e.Error is MemberException)
                {
                    generalErrorLabel2.Visibility = Visibility.Visible;
                    emailTextBox.Tag = "Error";
                    passwordTextBox.Tag = "Error";
                }

                if (e.Error is NotPremiumException)
                {
                    generalErrorLabel3.Visibility = Visibility.Visible;
                }

                loginButtonLoading.Visibility = Visibility.Hidden;
                loginButton.Visibility = Visibility.Visible;
                IsEnabledControls(true);
            }
            else
            {
                LoginData loginData = new LoginData();
                loginData.email = emailTextBox.Text;
                loginData.password = passwordTextBox.Password;
                loginData.autoLogin = autoLoginCheckBox.IsChecked.Value;

                GlobalSettings.SaveLoginData(loginData);

                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
        }

        private void LinkButtonClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            System.Diagnostics.Process.Start(b.Tag.ToString());
        }

        private void DragRectangleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MinimizeToTrayButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ForgotButtonClick(object sender, RoutedEventArgs e)
        {
            socialPanel.Visibility = Visibility.Collapsed;
            string radioUrl = RadioAPI.GetRadioUrl();
            forgotLinkButton.Tag = string.Format("https://{0}/login", radioUrl);
            forgotLinkButton.Content = string.Format((string)forgotLinkButton.Content, RadioAPI.GetRadioStationName());
            forgotPanel.Visibility = Visibility.Visible;
        }

        private void SocialButtonClick(object sender, RoutedEventArgs e)
        {
            forgotPanel.Visibility = Visibility.Collapsed;
            string radioUrl = RadioAPI.GetRadioUrl();
            socialLinkButton.Tag = string.Format("https://{0}/member/profile", radioUrl);
            socialLinkButton.Content = string.Format((string)socialLinkButton.Content, RadioAPI.GetRadioStationName());
            socialPanel.Visibility = Visibility.Visible;
        }
    }
}

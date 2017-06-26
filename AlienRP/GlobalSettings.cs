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

using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;

namespace AlienRP
{
    public class GlobalSettings
    {
        static GlobalSettings()
        {
            UpgradeSettings();
        }

        public static void SaveWindowPosition(WindowPosition windowPosition)
        {
            Properties.Settings.Default.mainWindowTop = windowPosition.top;
            Properties.Settings.Default.mainWindowLeft = windowPosition.left;
            Properties.Settings.Default.mainWindowWidth = windowPosition.width;
            Properties.Settings.Default.mainWindowHeight = windowPosition.height;

            Properties.Settings.Default.Save();
        }

        public static WindowPosition GetWindowPosition()
        {
            WindowPosition windowPosition = new WindowPosition();

            windowPosition.top = Properties.Settings.Default.mainWindowTop;
            windowPosition.left = Properties.Settings.Default.mainWindowLeft;
            windowPosition.height = Properties.Settings.Default.mainWindowHeight;
            windowPosition.width = Properties.Settings.Default.mainWindowWidth;

            return windowPosition;
        }

        public static void SaveWindowCollapsedPosition(WindowPosition windowPosition)
        {
            Properties.Settings.Default.mainWindowCollapsedTop = windowPosition.top;
            Properties.Settings.Default.mainWindowCollapsedLeft = windowPosition.left;

            Properties.Settings.Default.Save();
        }

        public static WindowPosition GetWindowCollapsedPosition()
        {
            WindowPosition windowPosition = new WindowPosition();

            windowPosition.top = Properties.Settings.Default.mainWindowCollapsedTop;
            windowPosition.left = Properties.Settings.Default.mainWindowCollapsedLeft;
            windowPosition.height = Properties.Settings.Default.mainWindowCollapsedHeight;

            return windowPosition;
        }

        public static void SaveWindowCollapsedStatus(bool isCollapsed)
        {
            Properties.Settings.Default.mainWindowIsCollapsed = isCollapsed;

            Properties.Settings.Default.Save();
        }

        public static bool GetWindowCollapsedStatus()
        {
            return Properties.Settings.Default.mainWindowIsCollapsed;
        }

        public static double GetVolumeLevel()
        {
            return Properties.Settings.Default.volumeLevel;
        }

        public static void SaveVolumeLevel(double volumeLevel)
        {
            Properties.Settings.Default.volumeLevel = volumeLevel;

            Properties.Settings.Default.Save();
        }

        public static List<int> GetHotkeys()
        {
            List<int> hotkeysList = new List<int>();
            int numHotkeys = 7;

            for (int i = 0; i < numHotkeys; i++)
            {
                hotkeysList.Add((int)Properties.Settings.Default["hotkey"+i]);
            }

            return hotkeysList;
        }

        public static void SaveHotkeys(List<int> hotkeysIDList)
        {
            int numHotkeys = 7;

            for (int i = 0; i < numHotkeys; i++)
            {
                Properties.Settings.Default["hotkey" + i] = hotkeysIDList[i];
            }

            Properties.Settings.Default.Save();
        }

        public static void SavePlayerData(PlayerData playerData)
        {
            Properties.Settings.Default.currentChannelKey = playerData.currentChannelKey;
            Properties.Settings.Default.currentChannelId = playerData.currentChannelId;
            Properties.Settings.Default.currentChannelName = playerData.currentChannelName;
            Properties.Settings.Default.qualityId = playerData.qualitylistId;

            Properties.Settings.Default.Save();
        }

        public static PlayerData GetPlayerData()
        {
            PlayerData playerData = new PlayerData();

            playerData.currentChannelId = Properties.Settings.Default.currentChannelId;
            playerData.currentChannelKey = Properties.Settings.Default.currentChannelKey;
            playerData.currentChannelName = Properties.Settings.Default.currentChannelName;
            playerData.qualitylistId = Properties.Settings.Default.qualityId;

            return playerData;
        }

        private static string GetPassword()
        {
            string result = "";
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("alienrpcredentials", FileMode.OpenOrCreate, isolatedStorage));

                if (srReader == null)
                {
                    return result;
                }
                else
                {
                    srReader.ReadLine();
                    result = srReader.ReadLine();
                }
                srReader.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return Decrypt(result);
        }

        private static string GetEmail()
        {
            string result = "";
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("alienrpcredentials", FileMode.OpenOrCreate, isolatedStorage));

                if (srReader == null)
                {
                    return result;
                }
                else
                {
                    result = srReader.ReadLine();
                }
                srReader.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return Decrypt(result);
        }

        public static void SaveLoginData(LoginData loginData)
        {
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamWriter srWriter = new StreamWriter(new IsolatedStorageFileStream("alienrpcredentials", FileMode.Create, isolatedStorage));

                if (loginData.rememberMe == true)
                {
                    srWriter.WriteLine(Encrypt(loginData.email));
                    srWriter.WriteLine(Encrypt(loginData.password));
                }
                else
                {
                    srWriter.WriteLine(Encrypt(""));
                    srWriter.WriteLine(Encrypt(""));
                }

                srWriter.Flush();
                srWriter.Close();
            }
            catch (System.Security.SecurityException)
            {
                throw;
            }

            Properties.Settings.Default.rememberMe = loginData.rememberMe;
            Properties.Settings.Default.Save();
        }

        public static string Encrypt(string str)
        {
            byte[] entropy = { 60, 47, 55, 29, 74, 49, 3, 99 };
            byte[] data = Encoding.ASCII.GetBytes(str);
            string protectedData = Convert.ToBase64String(ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser));
            return protectedData;
        }

        public static string Decrypt(string str)
        {
            if (str == null) return "";
            byte[] protectedData = Convert.FromBase64String(str);
            byte[] entropy = { 60, 47, 55, 29, 74, 49, 3, 99 };
            string data = Encoding.ASCII.GetString(ProtectedData.Unprotect(protectedData, entropy, DataProtectionScope.CurrentUser));
            return data;
        }

        private static void UpgradeSettings()
        {
            if (Properties.Settings.Default.upgradeRequired)
            {
                
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Reload();
                Properties.Settings.Default.upgradeRequired = false;
                Properties.Settings.Default.Save();
            }
        }

        public static LoginData GetLoginData()
        {
            LoginData loginData = new LoginData();

            loginData.email = GlobalSettings.GetEmail();
            loginData.password = GlobalSettings.GetPassword();
            loginData.rememberMe = Properties.Settings.Default.rememberMe;

            return loginData;
        }

        public static string GetRadioStation()
        {
            return Properties.Settings.Default.radioStation;
        }

        public static void SaveRadioStation(string radioStation)
        {
            Properties.Settings.Default.radioStation = radioStation;

            Properties.Settings.Default.Save();
        }
    }

    public class WindowPosition
    {
        public double top = 0;
        public double left = 0;
        public double height = 0;
        public double width = 0;

        public WindowPosition(double top, double left, double width, double height)
        {
            this.top = top;
            this.left = left;
            this.width = width;
            this.height = height;
        }

        public WindowPosition(double top, double left)
        {
            this.top = top;
            this.left = left;
        }

        public WindowPosition()
        {
        }
    }

    public class PlayerData
    {
        public string currentChannelKey;
        public string currentChannelId;
        public string currentChannelName;
        public int qualitylistId;
    }

    public class LoginData
    {
        public string email;
        public string password;
        public bool rememberMe;
    }
}

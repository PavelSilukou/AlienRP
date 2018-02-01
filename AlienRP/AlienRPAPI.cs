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
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Management;
using Microsoft.Win32;

namespace AlienRP
{
    class AlienRPAPI
    {
        //private static int userID;
        private static string alienrpAPI = "http://api.alienrp.com/";
        //private static bool apiEnabled;

        //static AlienRPAPI()
        //{
            //SetRadioStation(GlobalSettings.GetRadioStation());
        //}

        //public static void EnableAPI(bool state)
        //{
        //    apiEnabled = state;
        //}

        public static void Login()
        {
            if (!GlobalSettings.GetARPAPIEnabled()) return;

            int userID = GlobalSettings.GetARPUserID();
            if (userID != 0)
            {
                RestClient client = new RestClient(string.Format(alienrpAPI + "/login/{0}", userID));

                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(new { alienrp_version = GetAlienRPVersion() });
                IRestResponse response = client.Execute(request);

                if (response.Content.Equals("Invalid request"))
                {
                    return;
                }

                if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return;
                }
            }
            else
            {
                RestClient client = new RestClient(string.Format(alienrpAPI + "/login"));

                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(new { alienrp_version = GetAlienRPVersion() });
                IRestResponse response = client.Execute(request);

                if (response.Content.Equals("Invalid request"))
                {
                    return;
                }

                if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return;
                }
                else
                {
                    JObject json = JObject.Parse(response.Content);
                    int newUserID = Convert.ToInt32(json["user_id"]);
                    GlobalSettings.SaveARPUserID(newUserID);
                }
            }
        }

        public static void SendUserInfo()
        {
            if (!GlobalSettings.GetARPAPIEnabled()) return;

            int userID = GlobalSettings.GetARPUserID();
            RestClient client = new RestClient(string.Format(alienrpAPI + "/user/{0}", userID));

            RestRequest request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { os_version = GetOSVersionName(), os_architecture = GetOSArchitecture() });
            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Invalid request"))
            {
                return;
            }

            if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return;
            }
        }

        public static string GetLastVersion()
        {
            RestClient client = new RestClient(string.Format(alienrpAPI + "/version"));

            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Invalid request"))
            {
                return "";
            }

            if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return "";
            }
            else
            {
                JObject json = JObject.Parse(response.Content);
                return json["version"].ToString();
            }
        }

        private static string GetOSVersionName()
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                if (rk == null) return "";
                string ProductName = (string)rk.GetValue("ProductName");
                if (ProductName != "")
                {
                    return ProductName;
                }
                return "";
            }
            catch { return ""; }
        }

        private static string GetOSArchitecture()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return "x64";
            }
            else
            {
                return "x86";
            }
        }

        private static string GetAlienRPVersion()
        {
            return GlobalSettings.GetAlienRPVersion();
        }
    }
}

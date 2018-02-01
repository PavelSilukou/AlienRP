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

using NHotkey;
using NHotkey.Wpf;
using System.Windows.Input;
using System.Collections.Generic;
using AlienRP.Controls;

namespace AlienRP
{
    public class GlobalHotkeyManager
    {
        static bool voteButtonsState = false;
        static bool playerControlButtonsState = false;

        static GlobalHotkeyManager()
        {
            SetHotkeys();
        }

        private static void SetHotkeys()
        {
            List<int> hotkeysIDList = GlobalSettings.GetHotkeys();
            Key[] keys = new Key[hotkeysIDList.Count];

            for (int i = 0; i < hotkeysIDList.Count; i++)
            {
                keys[i] = Hotkeys.GetHotkey(hotkeysIDList[i]);
            }
            
            RemoveAllHotkeys();

            try
            {
                if (keys[0] != 0)
                {
                    HotkeyManager.Current.AddOrReplace("Play", keys[0], ModifierKeys.Control | ModifierKeys.Shift, OnPlayGlobal);
                }
                if (keys[1] != 0)
                {
                    HotkeyManager.Current.AddOrReplace("VolumeUp", keys[1], ModifierKeys.Control | ModifierKeys.Shift, OnVolumeUpGlobal);
                }
                if (keys[2] != 0)
                {
                    HotkeyManager.Current.AddOrReplace("VolumeDown", keys[2], ModifierKeys.Control | ModifierKeys.Shift, OnVolumeDownGlobal);
                }
                if (keys[3] != 0)
                {
                    HotkeyManager.Current.AddOrReplace("Mute", keys[3], ModifierKeys.Control | ModifierKeys.Shift, OnVolumeMuteGlobal);
                }
                if (keys[4] != 0)
                {
                    HotkeyManager.Current.AddOrReplace("VoteUp", keys[4], ModifierKeys.Control | ModifierKeys.Shift, OnVoteUpGlobal);
                }
                if (keys[5] != 0)
                {
                    HotkeyManager.Current.AddOrReplace("VoteDown", keys[5], ModifierKeys.Control | ModifierKeys.Shift, OnVoteDownGlobal);
                }
                if (keys[6] != 0)
                {
                    HotkeyManager.Current.AddOrReplace("VoteDelete", keys[6], ModifierKeys.Control | ModifierKeys.Shift, OnVoteDeleteGlobal);
                }
            }
            catch (NHotkey.HotkeyAlreadyRegisteredException)
            {

            }
        }

        public static void UpdateHotkeys(List<int> hotkeysIDList)
        {
            Key[] keys = new Key[hotkeysIDList.Count];

            for (int i = 0; i < hotkeysIDList.Count; i++)
            {
                keys[i] = Hotkeys.GetHotkey(hotkeysIDList[i]);
            }
            
            RemoveAllHotkeys();

            if (keys[0] != 0)
            {
                HotkeyManager.Current.AddOrReplace("Play", keys[0], ModifierKeys.Control | ModifierKeys.Shift, OnPlayGlobal);
            }
            if (keys[1] != 0)
            {
                HotkeyManager.Current.AddOrReplace("VolumeUp", keys[1], ModifierKeys.Control | ModifierKeys.Shift, OnVolumeUpGlobal);
            }
            if (keys[2] != 0)
            {
                HotkeyManager.Current.AddOrReplace("VolumeDown", keys[2], ModifierKeys.Control | ModifierKeys.Shift, OnVolumeDownGlobal);
            }
            if (keys[3] != 0)
            {
                HotkeyManager.Current.AddOrReplace("Mute", keys[3], ModifierKeys.Control | ModifierKeys.Shift, OnVolumeMuteGlobal);
            }
            if (keys[4] != 0)
            {
                HotkeyManager.Current.AddOrReplace("VoteUp", keys[4], ModifierKeys.Control | ModifierKeys.Shift, OnVoteUpGlobal);
            }
            if (keys[5] != 0)
            {
                HotkeyManager.Current.AddOrReplace("VoteDown", keys[5], ModifierKeys.Control | ModifierKeys.Shift, OnVoteDownGlobal);
            }
            if (keys[6] != 0)
            {
                HotkeyManager.Current.AddOrReplace("VoteDelete", keys[6], ModifierKeys.Control | ModifierKeys.Shift, OnVoteDeleteGlobal);
            }
        }

        public static void RemoveAllHotkeys()
        {
            HotkeyManager.Current.Remove("Play");
            HotkeyManager.Current.Remove("VolumeUp");
            HotkeyManager.Current.Remove("VolumeDown");
            HotkeyManager.Current.Remove("Mute");
            HotkeyManager.Current.Remove("VoteUp");
            HotkeyManager.Current.Remove("VoteDown");
            HotkeyManager.Current.Remove("VoteDelete");
        }

        static void OnPlayGlobal(object sender, HotkeyEventArgs e)
        {
            if (playerControlButtonsState)
            {
                PlayerControl.Instance.OnPlay();
                e.Handled = true;
            }
        }

        static void OnVolumeUpGlobal(object sender, HotkeyEventArgs e)
        {
            if (playerControlButtonsState)
            {
                PlayerControl.Instance.OnVolumeChanged(1);
                e.Handled = true;
            }
        }

        static void OnVolumeDownGlobal(object sender, HotkeyEventArgs e)
        {
            if (playerControlButtonsState)
            {
                PlayerControl.Instance.OnVolumeChanged(-1);
                e.Handled = true;
            }
        }

        static void OnVolumeMuteGlobal(object sender, HotkeyEventArgs e)
        {
            if (playerControlButtonsState)
            {
                PlayerControl.Instance.OnVolumeChanged(0);
                e.Handled = true;
            }
        }

        static void OnVoteUpGlobal(object sender, HotkeyEventArgs e)
        {
            if (voteButtonsState)
            {
                PlayerControl.Instance.voteButtons.OnVoteChanged(1);
                e.Handled = true;
            }
        }

        static void OnVoteDownGlobal(object sender, HotkeyEventArgs e)
        {
            if (voteButtonsState)
            {
                PlayerControl.Instance.voteButtons.OnVoteChanged(-1);
                e.Handled = true;
            }
        }

        static void OnVoteDeleteGlobal(object sender, HotkeyEventArgs e)
        {
            if (voteButtonsState)
            {
                PlayerControl.Instance.voteButtons.OnVoteChanged(0);
                e.Handled = true;
            }
        }

        public static void IsEnableVoteHotkeys(bool state)
        {
            voteButtonsState = state;
        }

        public static void IsEnablePlayerControlHotkeys(bool state)
        {
            playerControlButtonsState = state;
        }

        public static void IsEnableAllHotkeys(bool state)
        {
            IsEnableVoteHotkeys(state);
            IsEnablePlayerControlHotkeys(state);
        }
    }
}

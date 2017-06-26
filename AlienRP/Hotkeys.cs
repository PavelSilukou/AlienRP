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
using System.Windows.Input;

namespace AlienRP
{
    public static class Hotkeys
    {
        public class KeyNamePair
        {
            public Key key;
            public string name;

            public KeyNamePair(Key key, string name)
            {
                this.key = key;
                this.name = name;
            }
        }

        public static Dictionary<int, KeyNamePair> hotkeysDict = new Dictionary<int, KeyNamePair>();

        static Hotkeys()
        {
            hotkeysDict.Add(0, new KeyNamePair(Key.A, "A"));
            hotkeysDict.Add(1, new KeyNamePair(Key.B, "B"));
            hotkeysDict.Add(2, new KeyNamePair(Key.C, "C"));
            hotkeysDict.Add(3, new KeyNamePair(Key.D, "D"));
            hotkeysDict.Add(4, new KeyNamePair(Key.E, "E"));
            hotkeysDict.Add(5, new KeyNamePair(Key.F, "F"));
            hotkeysDict.Add(6, new KeyNamePair(Key.G, "G"));
            hotkeysDict.Add(7, new KeyNamePair(Key.H, "H"));
            hotkeysDict.Add(8, new KeyNamePair(Key.I, "I"));
            hotkeysDict.Add(9, new KeyNamePair(Key.J, "J"));
            hotkeysDict.Add(10, new KeyNamePair(Key.K, "K"));
            hotkeysDict.Add(11, new KeyNamePair(Key.L, "L"));
            hotkeysDict.Add(12, new KeyNamePair(Key.M, "M"));
            hotkeysDict.Add(13, new KeyNamePair(Key.N, "N"));
            hotkeysDict.Add(14, new KeyNamePair(Key.O, "O"));
            hotkeysDict.Add(15, new KeyNamePair(Key.P, "P"));
            hotkeysDict.Add(16, new KeyNamePair(Key.Q, "Q"));
            hotkeysDict.Add(17, new KeyNamePair(Key.R, "R"));
            hotkeysDict.Add(18, new KeyNamePair(Key.S, "S"));
            hotkeysDict.Add(19, new KeyNamePair(Key.T, "T"));
            hotkeysDict.Add(20, new KeyNamePair(Key.U, "U"));
            hotkeysDict.Add(21, new KeyNamePair(Key.V, "V"));
            hotkeysDict.Add(22, new KeyNamePair(Key.W, "W"));
            hotkeysDict.Add(23, new KeyNamePair(Key.X, "X"));
            hotkeysDict.Add(24, new KeyNamePair(Key.Y, "Y"));
            hotkeysDict.Add(25, new KeyNamePair(Key.Z, "Z"));
            hotkeysDict.Add(26, new KeyNamePair(Key.D0, "0"));
            hotkeysDict.Add(27, new KeyNamePair(Key.D1, "1"));
            hotkeysDict.Add(28, new KeyNamePair(Key.D2, "2"));
            hotkeysDict.Add(29, new KeyNamePair(Key.D3, "3"));
            hotkeysDict.Add(30, new KeyNamePair(Key.D4, "4"));
            hotkeysDict.Add(31, new KeyNamePair(Key.D5, "5"));
            hotkeysDict.Add(32, new KeyNamePair(Key.D6, "6"));
            hotkeysDict.Add(33, new KeyNamePair(Key.D7, "7"));
            hotkeysDict.Add(34, new KeyNamePair(Key.D8, "8"));
            hotkeysDict.Add(35, new KeyNamePair(Key.D9, "9"));
            hotkeysDict.Add(36, new KeyNamePair(Key.F1, "F1"));
            hotkeysDict.Add(37, new KeyNamePair(Key.F2, "F2"));
            hotkeysDict.Add(38, new KeyNamePair(Key.F3, "F3"));
            hotkeysDict.Add(39, new KeyNamePair(Key.F4, "F4"));
            hotkeysDict.Add(40, new KeyNamePair(Key.F5, "F5"));
            hotkeysDict.Add(41, new KeyNamePair(Key.F6, "F6"));
            hotkeysDict.Add(42, new KeyNamePair(Key.F7, "F7"));
            hotkeysDict.Add(43, new KeyNamePair(Key.F8, "F8"));
            hotkeysDict.Add(44, new KeyNamePair(Key.F9, "F9"));
            hotkeysDict.Add(45, new KeyNamePair(Key.F10, "F10"));
            hotkeysDict.Add(46, new KeyNamePair(Key.F11, "F11"));
            hotkeysDict.Add(47, new KeyNamePair(Key.F12, "F12"));
            hotkeysDict.Add(48, new KeyNamePair(Key.F13, "F13"));
            hotkeysDict.Add(49, new KeyNamePair(Key.F14, "F14"));
            hotkeysDict.Add(50, new KeyNamePair(Key.F15, "F15"));
            hotkeysDict.Add(51, new KeyNamePair(Key.F16, "F16"));
            hotkeysDict.Add(52, new KeyNamePair(Key.F17, "F17"));
            hotkeysDict.Add(53, new KeyNamePair(Key.F18, "F18"));
            hotkeysDict.Add(54, new KeyNamePair(Key.F19, "F19"));
            hotkeysDict.Add(55, new KeyNamePair(Key.F20, "F20"));
            hotkeysDict.Add(56, new KeyNamePair(Key.F21, "F21"));
            hotkeysDict.Add(57, new KeyNamePair(Key.F22, "F22"));
            hotkeysDict.Add(58, new KeyNamePair(Key.F23, "F23"));
            hotkeysDict.Add(59, new KeyNamePair(Key.F24, "F24"));
            hotkeysDict.Add(60, new KeyNamePair(Key.Delete, "Delete"));
            hotkeysDict.Add(61, new KeyNamePair(Key.Space, "Space"));
            hotkeysDict.Add(62, new KeyNamePair(Key.Back, "Backspace"));
            hotkeysDict.Add(63, new KeyNamePair(Key.OemTilde, "~"));
            hotkeysDict.Add(64, new KeyNamePair(Key.OemMinus, "-"));
            hotkeysDict.Add(65, new KeyNamePair(Key.OemPlus, "+"));
            hotkeysDict.Add(66, new KeyNamePair(Key.OemOpenBrackets, "["));
            hotkeysDict.Add(67, new KeyNamePair(Key.Oem6, "]"));
            hotkeysDict.Add(68, new KeyNamePair(Key.Oem5, "\\"));
            hotkeysDict.Add(69, new KeyNamePair(Key.OemComma, ","));
            hotkeysDict.Add(70, new KeyNamePair(Key.OemPeriod, "."));
            hotkeysDict.Add(71, new KeyNamePair(Key.Oem1, ";"));
            hotkeysDict.Add(72, new KeyNamePair(Key.OemQuotes, "\""));
            hotkeysDict.Add(73, new KeyNamePair(Key.OemQuestion, "?"));
            hotkeysDict.Add(74, new KeyNamePair(Key.NumPad0, "Num 0"));
            hotkeysDict.Add(75, new KeyNamePair(Key.NumPad1, "Num 1"));
            hotkeysDict.Add(76, new KeyNamePair(Key.NumPad2, "Num 2"));
            hotkeysDict.Add(77, new KeyNamePair(Key.NumPad3, "Num 3"));
            hotkeysDict.Add(78, new KeyNamePair(Key.NumPad4, "Num 4"));
            hotkeysDict.Add(79, new KeyNamePair(Key.NumPad5, "Num 5"));
            hotkeysDict.Add(80, new KeyNamePair(Key.NumPad6, "Num 6"));
            hotkeysDict.Add(81, new KeyNamePair(Key.NumPad7, "Num 7"));
            hotkeysDict.Add(82, new KeyNamePair(Key.NumPad8, "Num 8"));
            hotkeysDict.Add(83, new KeyNamePair(Key.NumPad9, "Num 9"));
            hotkeysDict.Add(84, new KeyNamePair(Key.Left, Properties.Resources.HotkeyArrowLeft));
            hotkeysDict.Add(85, new KeyNamePair(Key.Right, Properties.Resources.HotkeyArrowRight));
            hotkeysDict.Add(86, new KeyNamePair(Key.Down, Properties.Resources.HotkeyArrowDown));
            hotkeysDict.Add(87, new KeyNamePair(Key.Up, Properties.Resources.HotkeyArrowUp));
            hotkeysDict.Add(88, new KeyNamePair(Key.Add, "Num +"));
            hotkeysDict.Add(89, new KeyNamePair(Key.Divide, "Num /"));
            hotkeysDict.Add(90, new KeyNamePair(Key.Multiply, "Num *"));
            hotkeysDict.Add(91, new KeyNamePair(Key.Subtract, "Num -"));
            hotkeysDict.Add(92, new KeyNamePair(Key.Decimal, "Num ."));
            hotkeysDict.Add(93, new KeyNamePair(Key.Insert, "Insert"));
            hotkeysDict.Add(94, new KeyNamePair(Key.PageDown, "Page Down"));
            hotkeysDict.Add(95, new KeyNamePair(Key.PageUp, "Page Up"));
            hotkeysDict.Add(96, new KeyNamePair(Key.Home, "Home"));
            hotkeysDict.Add(97, new KeyNamePair(Key.End, "End"));
        }

        public static string GetHotkeyName(int keyID)
        {
            if (hotkeysDict.ContainsKey(keyID))
            {
                return hotkeysDict[keyID].name;
            }
            else
            {
                return "";
            }
        }

        public static Key GetHotkey(int keyID)
        {
            if (hotkeysDict.ContainsKey(keyID))
            {
                return hotkeysDict[keyID].key;
            }
            else
            {
                return 0;
            }
        }

        public static int GetKeyIDByKey(Key key)
        {
            foreach (KeyValuePair<int, KeyNamePair> hotkey in hotkeysDict)
            {
                if (hotkey.Value.key == key)
                {
                    return hotkey.Key;
                }
            }

            return -1;
        }
    }
}
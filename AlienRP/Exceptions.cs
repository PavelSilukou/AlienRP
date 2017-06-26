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

namespace AlienRP
{
    public class InternetException : Exception
    {
        public InternetException()
        {
        }
    }

    public class MemberException : Exception
    {
        public MemberException()
        {
        }
    }

    public class NotPremiumException : Exception
    {
        public NotPremiumException()
        {
        }
    }

    public class StreamlistException : Exception
    {
        public StreamlistException()
        {
        }
    }

    public class ChannelException : Exception
    {
        public ChannelException()
        {
        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }
    }
}

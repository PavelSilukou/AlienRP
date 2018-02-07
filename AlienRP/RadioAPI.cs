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

using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace AlienRP
{
    class RadioAPI
    {
        private static RadioData radioData;

        static RadioAPI()
        {
            SetRadioStation(GlobalSettings.GetRadioStation());
        }

        public static void SetRadioStation(string station)
        {
            switch (station)
            {
                case "di":
                {
                    radioData = new DIRadioData();
                    break;
                }
                case "rockradio":
                {
                    radioData = new RockRadioData();
                    break;
                }
                case "classicalradio":
                {
                    radioData = new ClassicalRadioData();
                    break;
                }
                case "jazzradio":
                {
                    radioData = new JazzRadioData();
                    break;
                }
                case "radiotunes":
                {
                    radioData = new RadioTunesData();
                    break;
                }
            }

            GlobalSettings.SaveRadioStation(station);
        }

        public static void Login(string username, string password)
        {
            RestClient client = new RestClient(string.Format("https://api.audioaddict.com/v1/{0}/members/authenticate", radioData.RadioStationPrefix));

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Invalid Username/Password"))
            {
                throw new MemberException();
            }

            if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }
            else
            {
                JsonConvert.DeserializeObject<User>(response.Content);
                JObject json = JObject.Parse(response.Content);

                if (json["subscriptions"].HasValues)
                {
                    string status = json["subscriptions"][0]["status"].ToString();
                    if (!status.Equals("active"))
                    {
                        throw new NotPremiumException();
                    }
                }
                else
                {
                    throw new NotPremiumException();
                }
            }
        }

        public static void Ping()
        {
            RestClient client = new RestClient(string.Format("http://{0}/_papi/v1/ping", radioData.RadioUrl));

            DateTime now = DateTime.Now;

            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }

            JObject json = JObject.Parse(response.Content);
            string time = json["time"].ToString();
            PlayerSettings.timeOffset = ConvertToUnixTimestamp(now) - ConvertToUnixTimestamp(DateTime.ParseExact(time, "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture));

        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static void GetAudioLinks()
        {
            RestClient client = new RestClient(string.Format("http://listen.{3}/{0}/{1}?{2}", radioData.QualityList[PlayerSettings.qualitylistId].id, PlayerSettings.currentChannelKey, User.listenKey, radioData.RadioStationListenValue));

            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Unable to find a matching streamlist"))
            {
                throw new StreamlistException();
            }
            else if (response.Content.Equals("Unable to find a channel by that key"))
            {
                throw new ChannelException();
            }
            else if (response.Content.Equals("Invalid Listen Key"))
            {
                throw new MemberException();
            }
            else if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }
            else
            {
                IList<string> audioStreams = JsonConvert.DeserializeObject<IList<string>>(response.Content);
                Random rnd = new Random();
                int randomStream = rnd.Next(audioStreams.Count);
                PlayerSettings.audioStream = audioStreams[randomStream];
            }
        }

        public static Track GetNowPlayingTrack()
        {
            RestClient client = new RestClient(string.Format("http://{0}/_papi/v1/{1}/track_history/channel/{2}", radioData.RadioUrl, radioData.RadioStationPrefix, PlayerSettings.currentChannelId));

            RestRequest request = new RestRequest(Method.GET);

            client.Execute(request);
            IRestResponse response = client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }

            JArray json = JArray.Parse(response.Content);

            for (int i = 0; i < json.Count; i++)
            {
                string type = json[i]["type"].ToString();
                if (!type.Equals("advertisement"))
                {
                    Track track = new Track();
                    track.artistName = json[i]["display_artist"].ToString();
                    track.trackName = json[i]["display_title"].ToString();
                    track.duration = Int32.Parse(json[i]["duration"].ToString());
                    track.startTime = Int32.Parse(json[i]["started"].ToString());
                    track.id = json[i]["track_id"].ToString();

                    track.upVote = Int32.Parse(json[i]["votes"]["up"].ToString());
                    track.downVote = Int32.Parse(json[i]["votes"]["down"].ToString());
                    
                    return track;
                }
            }

            throw new BadRequestException();
        }

        public static List<Channel> GetAllChannels()
        {
            RestClient client = new RestClient(string.Format("http://{0}/_papi/v1/{1}/channel_filters/{2}", radioData.RadioUrl, radioData.RadioStationPrefix, radioData.RadioStationAllStationFilterValue));

            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }
            else
            {
                JObject json = JObject.Parse(response.Content);

                List<Channel> channels = new List<Channel>(json["channels"].Count());

                for (int i = 0; i < json["channels"].Count(); i++)
                {
                    Channel channel = new Channel();

                    channel.id = json["channels"][i]["id"].ToString();
                    channel.key = json["channels"][i]["key"].ToString();
                    channel.name = json["channels"][i]["name"].ToString();

                    string imageUrl = radioData.GetStationBanner(json["channels"][i]["images"]);

                    int imageFormatIndex = imageUrl.IndexOf("jpg");
                    if (imageFormatIndex == -1)
                    {
                        imageFormatIndex = imageUrl.IndexOf("png");
                    }

                    imageUrl = "http:" + imageUrl.Substring(0, imageFormatIndex + 3);
                    channel.imageUrl = imageUrl;

                    channels.Add(channel);
                }

                IList<FavoriteChannel> favoriteChannels = GetFavoritesChannelsList();

                foreach (FavoriteChannel favorite in favoriteChannels.Reverse())
                {
                    for (int i = 0; i < channels.Count; i++)
                    {
                        if (channels[i].id == favorite.channelId)
                        {
                            Channel tempChannel = channels.ElementAt(i);
                            tempChannel.isFavorite = true;
                            channels.RemoveAt(i);
                            channels.Insert(0, tempChannel);
                        }
                    }
                }

                return channels;
            }
        }

        public static IList<FavoriteChannel> GetFavoritesChannelsList()
        {
            RestClient client = new RestClient(string.Format("https://{0}/_papi/v1/{1}/members/{2}/favorites/channels", radioData.RadioUrl, radioData.RadioStationPrefix, User.id));

            RestRequest request = new RestRequest(Method.GET);
            request.AddParameter("api_key", User.apiKey);

            IRestResponse response = client.Execute(request);

            if (response.Content.Contains("Member Authentication required"))
            {
                throw new MemberException();
            }
            else if (response.Content.Equals("Invalid API Key"))
            {
                throw new MemberException();
            }

            if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }
            else
            {
                return JsonConvert.DeserializeObject<IList<FavoriteChannel>>(response.Content);
            }
        }

        public static Vote GetVote(string trackId)
        {
            RestClient client = new RestClient(string.Format("https://{0}/_papi/v1/{1}/members/{2}/track_votes/{3}", radioData.RadioUrl, radioData.RadioStationPrefix, User.id, trackId));

            RestRequest request = new RestRequest(Method.GET);
            request.AddParameter("api_key", User.apiKey);

            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Invalid API Key"))
            {
                throw new MemberException();
            }
            else if (response.Content.Equals("Member Authentication required"))
            {
                throw new MemberException();
            }
            else if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }

            if (!response.Content.Equals("[]"))
            {
                JArray json = JArray.Parse(response.Content);

                Vote vote = new Vote();

                vote.isDown = Boolean.Parse(json[0]["down"].ToString());
                vote.isUp = Boolean.Parse(json[0]["up"].ToString());

                return vote;
            }
            else
            {
                return new Vote { isDown = false, isUp = false };
            }
        }

        public static VotesCount VoteUp(string trackId)
        {
            RestClient client = new RestClient(string.Format("https://{0}/_papi/v1/{1}/tracks/{2}/vote/{3}/up", radioData.RadioUrl, radioData.RadioStationPrefix, trackId, PlayerSettings.currentChannelId));

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("api_key", User.apiKey);
            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Invalid API Key"))
            {
                throw new MemberException();
            }
            else if (response.Content.Equals("Member Authentication required"))
            {
                throw new MemberException();
            }
            else if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }

            return JsonConvert.DeserializeObject<VotesCount>(response.Content);
        }

        public static VotesCount VoteDown(string trackId)
        {
            RestClient client = new RestClient(string.Format("https://{0}/_papi/v1/{1}/tracks/{2}/vote/{3}/down", radioData.RadioUrl, radioData.RadioStationPrefix, trackId, PlayerSettings.currentChannelId));

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("api_key", User.apiKey);
            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Invalid API Key"))
            {
                throw new MemberException();
            }
            else if (response.Content.Equals("Member Authentication required"))
            {
                throw new MemberException();
            }
            else if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }

            return JsonConvert.DeserializeObject<VotesCount>(response.Content);
        }

        public static VotesCount DeleteVote(string trackId)
        {
            RestClient client = new RestClient(string.Format("https://{0}/_papi/v1/{1}/tracks/{2}/vote/{3}", radioData.RadioUrl, radioData.RadioStationPrefix, trackId, PlayerSettings.currentChannelId));

            RestRequest request = new RestRequest(Method.DELETE);
            request.AddParameter("api_key", User.apiKey);
            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Invalid API Key"))
            {
                throw new MemberException();
            }
            else if (response.Content.Equals("Member Authentication required"))
            {
                throw new MemberException();
            }
            else if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }

            return JsonConvert.DeserializeObject<VotesCount>(response.Content);
        }

        public static void AddFavorite(string channelId)
        {
            RestClient client = new RestClient(string.Format("https://{0}/_papi/v1/{1}/members/{2}/favorites/channel/{3}", radioData.RadioUrl, radioData.RadioStationPrefix, User.id, channelId));

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("api_key", User.apiKey);

            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Member Authentication required"))
            {
                throw new MemberException();
            }
            else if (response.Content.Equals("Invalid API Key"))
            {
                throw new MemberException();
            }
            else if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternetException();
            }
        }

        public static void DeleteFavorite(string channelId)
        {
            RestClient client = new RestClient(string.Format("https://{0}/_papi/v1/{1}/members/{2}/favorites/channel/{3}", radioData.RadioUrl, radioData.RadioStationPrefix, User.id, channelId));

            RestRequest request = new RestRequest(Method.DELETE);
            request.AddParameter("api_key", User.apiKey);

            IRestResponse response = client.Execute(request);

            if (response.Content.Equals("Member Authentication required"))
            {
                throw new MemberException();
            }
            else if (response.Content.Equals("Invalid API Key"))
            {
                throw new MemberException();
            }
            else if (response.ResponseStatus == ResponseStatus.Error || response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new InternetException();
            }
        }

        public static string GetRadioUrl()
        {
            return radioData.RadioUrl;
        }

        public static Quality GetQuality()
        {
            return radioData.QualityList[PlayerSettings.qualitylistId];
        }

        public static Quality[] GetQualityList()
        {
            return radioData.QualityList;
        }

        public static string GetRadioStationName()
        {
            return radioData.RadioStationName;
        }
    }

    public class FavoriteChannel
    {
        [JsonProperty("channel_id")]
        public string channelId;
        public int position;
    }

    public static class PlayerSettings
    {
        public static int qualitylistId;
        public static string currentChannelKey;
        public static string currentChannelId;
        public static string currentChannelName;
        public static string audioStream;
        public static double timeOffset = 0;

        static PlayerSettings()
        {
            PlayerData playerData = GlobalSettings.GetPlayerData();
            currentChannelId = playerData.currentChannelId;
            currentChannelKey = playerData.currentChannelKey;
            currentChannelName = playerData.currentChannelName;
            qualitylistId = playerData.qualitylistId;
        }

        public static void SavePlayerSettings()
        {
            PlayerData playerData = new PlayerData();
            playerData.currentChannelId = currentChannelId;
            playerData.currentChannelKey = currentChannelKey;
            playerData.currentChannelName = currentChannelName;
            playerData.qualitylistId = qualitylistId;

            GlobalSettings.SavePlayerData(playerData);
        }

        public static void ClearChannelInfo()
        {
            currentChannelKey = "";
            currentChannelId = "";
            currentChannelName = "";
        }
    }

    public class User
    {
        [JsonProperty("api_key")]
        public static string apiKey { get; set; }
        [JsonProperty("listen_key")]
        public static string listenKey { get; set; }
        [JsonProperty("first_name")]
        public static string firstName { get; set; }
        [JsonProperty("last_name")]
        public static string lastName { get; set; }
        [JsonProperty("id")]
        public static string id { get; set; }
    }

    public class Channel
    {
        public string id;
        public string key;
        public string name;
        public string imageUrl;
        public bool isFavorite = false;

        public Channel(string id, string key, string name)
        {
            this.id = id;
            this.key = key;
            this.name = name;
        }

        public Channel()
        {

        }
    }

    public class Track
    {
        public string artistName;
        public string trackName;
        public int duration;
        public int startTime;
        public string id;
        public int downVote;
        public int upVote;
    }

    public class Vote
    {
        public bool isUp;
        public bool isDown;
    }

    public class VotesCount
    {
        public int up;
        public int down;
    }

    abstract class RadioData
    {
        abstract public string RadioStationPrefix { get; set; }
        abstract public string RadioStationListenValue { get; set; }
        abstract public string RadioUrl { get; set; }
        abstract public int RadioStationAllStationFilterValue { get; set; }
        abstract public Quality[] QualityList { get; set; }
        abstract public string RadioStationName { get; set; }

        abstract public string GetStationBanner(JToken jtoken);
    }

    class DIRadioData : RadioData
    {
        public override string RadioStationPrefix { get; set; }
        public override string RadioStationListenValue { get; set; }
        public override string RadioUrl { get; set; }
        public override int RadioStationAllStationFilterValue { get; set; }
        public override Quality[] QualityList { get; set; }
        public override string RadioStationName { get; set; }

        public DIRadioData()
        {
            this.RadioStationPrefix = "di";
            this.RadioStationListenValue = "di.fm";
            this.RadioUrl = "www.di.fm";
            this.RadioStationAllStationFilterValue = 1;
            this.QualityList = new Quality[] {new Quality("premium_low", "40K", "AAC"),
                                           new Quality("premium_medium", "64K", "AAC"),
                                           new Quality("premium", "128K", "AAC"),
                                           new Quality("premium_high", "320K", "MP3")};
            this.RadioStationName = "Digitally Imported";
        }

        public override string GetStationBanner(JToken jtoken)
        {
            return jtoken["horizontal_banner"].ToString();
        }
    }

    class JazzRadioData : RadioData
    {
        public override string RadioStationPrefix { get; set; }
        public override string RadioStationListenValue { get; set; }
        public override string RadioUrl { get; set; }
        public override int RadioStationAllStationFilterValue { get; set; }
        public override Quality[] QualityList { get; set; }
        public override string RadioStationName { get; set; }

        public JazzRadioData()
        {
            this.RadioStationPrefix = "jazzradio";
            this.RadioStationListenValue = "jazzradio.com";
            this.RadioUrl = "www.jazzradio.com";
            this.RadioStationAllStationFilterValue = 3;
            this.QualityList = new Quality[] {new Quality("premium_low", "40K", "AAC"),
                                           new Quality("premium_medium", "64K", "AAC"),
                                           new Quality("premium", "128K", "AAC"),
                                           new Quality("premium_high", "320K", "MP3")};
            this.RadioStationName = "JAZZRADIO.com";
        }

        public override string GetStationBanner(JToken jtoken)
        {
            return jtoken["default"].ToString();
        }
    }

    class RockRadioData : RadioData
    {
        public override string RadioStationPrefix { get; set; }
        public override string RadioStationListenValue { get; set; }
        public override string RadioUrl { get; set; }
        public override int RadioStationAllStationFilterValue { get; set; }
        public override Quality[] QualityList { get; set; }
        public override string RadioStationName { get; set; }

        public RockRadioData()
        {
            this.RadioStationPrefix = "rockradio";
            this.RadioStationListenValue = "rockradio.com";
            this.RadioUrl = "www.rockradio.com";
            this.RadioStationAllStationFilterValue = 50;
            this.QualityList = new Quality[] {new Quality("premium_low", "40K", "AAC"),
                                           new Quality("premium_medium", "64K", "AAC"),
                                           new Quality("premium", "128K", "AAC"),
                                           new Quality("premium_high", "256K", "MP3")};
            this.RadioStationName = "ROCKRADIO.COM";
        }

        public override string GetStationBanner(JToken jtoken)
        {
            return jtoken["default"].ToString();
        }
    }

    class RadioTunesData : RadioData
    {
        public override string RadioStationPrefix { get; set; }
        public override string RadioStationListenValue { get; set; }
        public override string RadioUrl { get; set; }
        public override int RadioStationAllStationFilterValue { get; set; }
        public override Quality[] QualityList { get; set; }
        public override string RadioStationName { get; set; }

        public RadioTunesData()
        {
            this.RadioStationPrefix = "radiotunes";
            this.RadioStationListenValue = "radiotunes.com";
            this.RadioUrl = "www.radiotunes.com";
            this.RadioStationAllStationFilterValue = 2;
            this.QualityList = new Quality[] {new Quality("premium_low", "40K", "AAC"),
                                           new Quality("premium_medium", "64K", "AAC"),
                                           new Quality("premium", "128K", "AAC"),
                                           new Quality("premium_high", "320K", "MP3")};
            this.RadioStationName = "RadioTunes";
        }

        public override string GetStationBanner(JToken jtoken)
        {
            return jtoken["default"].ToString();
        }
    }

    class ClassicalRadioData : RadioData
    {
        public override string RadioStationPrefix { get; set; }
        public override string RadioStationListenValue { get; set; }
        public override string RadioUrl { get; set; }
        public override int RadioStationAllStationFilterValue { get; set; }
        public override Quality[] QualityList { get; set; }
        public override string RadioStationName { get; set; }

        public ClassicalRadioData()
        {
            this.RadioStationPrefix = "classicalradio";
            this.RadioStationListenValue = "classicalradio.com";
            this.RadioUrl = "www.classicalradio.com";
            this.RadioStationAllStationFilterValue = 90;
            this.QualityList = new Quality[] {new Quality("premium_low", "40K", "AAC"),
                                           new Quality("premium_medium", "64K", "AAC"),
                                           new Quality("premium", "128K", "AAC"),
                                           new Quality("premium_high", "320K", "MP3")};
            this.RadioStationName = "ClassicalRadio.com";
        }

        public override string GetStationBanner(JToken jtoken)
        {
            return jtoken["default"].ToString();
        }
    }

    public class Quality
    {
        public string id;
        public string bitrate;
        public string format;

        public Quality(string id, string bitrate, string format)
        {
            this.id = id;
            this.bitrate = bitrate;
            this.format = format;
        }
    }
}


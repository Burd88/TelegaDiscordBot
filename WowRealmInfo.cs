using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    public class WoWRealStatus
    {
        public string RealmName { get; set; }
        public string RealmStatus { get; set; }
        public string RealnStatusType { get; set; }
        public bool Error { get; set; }


    }
    class WowRealmInfo
    {
        private static WoWRealStatus wowRealmStatus;
        public static string realmstatustype = "";
        public static WoWRealStatus GetRealmInfo()
        {
            wowRealmStatus = new();
            GetRealmConnectedLink();

            return wowRealmStatus;
        }


        public static string[] GetRealmInfoForTimer()
        {
            Task<BotSettings> set = Functions.ReadJson<BotSettings>("BotSettings");
            Program.settings = set.Result;
            wowRealmStatus = new();
            GetRealmConnectedLink();
            if (!wowRealmStatus.Error)
            {
                string str = CheckRealmStatus(Program.settings.RealmStatusType);

                if (str == "Up")
                {
                    Program.settings.RealmStatusType = wowRealmStatus.RealnStatusType;
                    Functions.WriteJSon(Program.settings, "BotSettings");
                    string[] str1 = new string[2];
                    str1[0] = "**Тех. Обслуживание закончилось!**";
                    str1[1] = $"Игровой мир: **{wowRealmStatus.RealmName}** работает!\u2705";
                    return str1;

                }
                else if (str == "Down")
                {
                    string[] str1 = new string[2];
                    str1[0] = "**Тех. Обслуживание началось!**";
                    str1[1] = $"Игровой мир: **{wowRealmStatus.RealmName}** не работает!\u274c";
                    Program.settings.RealmStatusType = wowRealmStatus.RealnStatusType;
                    Functions.WriteJSon(Program.settings, "BotSettings");
                    return str1;

                }
                else if (str == "No work")
                {
                    return null;
                }
                else if (str == "Work")
                {
                    return null;
                }
            }


            return null;
        }
        static void GetRealmConnectedLink()
        {
            try
            {

                WebRequest request = WebRequest.Create($"https://eu.api.blizzard.com/data/wow/realm/{settings.RealmSlug}?namespace=dynamic-eu&locale=ru_RU&access_token={tokenWow}");

                WebResponse responce = request.GetResponse();

                using (System.IO.Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {


                            RealmInfo realm = JsonConvert.DeserializeObject<RealmInfo>(line);
                            RealmUpdateFunction(realm.id.ToString());


                        }
                    }
                }
                responce.Close();
                wowRealmStatus.Error = false;

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    wowRealmStatus.Error = true;

                    string message = $"\nGetRealmConnectedLink Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                wowRealmStatus.Error = true;

                string message = $"GetRealmConnectedLink Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }


        }

        static void RealmUpdateFunction(string id)
        {
            try
            {

                WebRequest request = WebRequest.Create($"https://eu.api.blizzard.com/data/wow/connected-realm/{id}?namespace=dynamic-eu&locale=ru_RU&access_token={tokenWow}");

                WebResponse responce = request.GetResponse();

                using (System.IO.Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {


                            ConnectRealm realm = JsonConvert.DeserializeObject<ConnectRealm>(line);
                            if (realm.status.type == "UP")
                            {

                                wowRealmStatus.RealnStatusType = realm.status.type;
                                wowRealmStatus.RealmStatus = "\u2705" + realm.status.name;
                            }
                            else
                            {
                                wowRealmStatus.RealnStatusType = realm.status.type;
                                wowRealmStatus.RealmStatus = "\u274c" + realm.status.name;

                            }

                            foreach (ConectRealm realms in realm.realms)
                            {
                                wowRealmStatus.RealmName = realms.name;

                            }



                        }
                    }
                }
                responce.Close();
                wowRealmStatus.Error = false;

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    wowRealmStatus.Error = true;

                    string message = $"\nRealmUpdateFunction Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                wowRealmStatus.Error = true;

                string message = $"RealmUpdateFunction Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }


        }

        static string CheckRealmStatus(string beforeRealStatus)
        {

            try
            {


                if (beforeRealStatus == wowRealmStatus.RealnStatusType)
                {
                    if (wowRealmStatus.RealnStatusType == "UP")
                    {

                        return "Work";
                    }
                    else
                    {

                        return "No work";
                    }

                }
                else
                {
                    if (wowRealmStatus.RealnStatusType == "UP")
                    {

                        return "Up";
                    }
                    else
                    {

                        return "Down";
                    }
                }


            }

            catch (Exception e)
            {

                string message = $"CheckRealmStatus Error: {e.Message}";
                Functions.WriteLogs(message, "error");
                return "Error";
            }

        }

    }
    #region RealmInfo Classes




    public class Population
    {
        public string type { get; set; }
        public string name { get; set; }
    }



    public class Region
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ConnectedRealm
    {
        public string href { get; set; }
    }

    public class Type1
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class ConectRealm
    {
        public int id { get; set; }
        public Region region { get; set; }
        public ConnectedRealm connected_realm { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
        public Type1 type { get; set; }
        public bool is_tournament { get; set; }
        public string slug { get; set; }
    }

    public class MythicLeaderboards
    {
        public string href { get; set; }
    }

    public class Auctions
    {
        public string href { get; set; }
    }

    public class ConnectRealm
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public bool has_queue { get; set; }
        public Status status { get; set; }
        public Population population { get; set; }
        public List<ConectRealm> realms { get; set; }
        public MythicLeaderboards mythic_leaderboards { get; set; }
        public Auctions auctions { get; set; }
    }







    public class RealmInfo
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public Region region { get; set; }
        public ConnectedRealm connected_realm { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
        public Type type { get; set; }
        public bool is_tournament { get; set; }
        public string slug { get; set; }
    }
    #endregion
}

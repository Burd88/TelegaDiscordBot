using System;
using System.Collections.Generic;
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
        private WoWRealStatus wowRealmStatus;
        private string realmstatustype = "";
        public WoWRealStatus GetRealmInfo()
        {
            wowRealmStatus = new();
            GetRealmConnectedLink();

            return wowRealmStatus;
        }


        public string[] GetRealmInfoForTimer()
        {
            settings = Functions.ReadJson<BotSettings>("BotSettings").Result;

            wowRealmStatus = new();
            GetRealmConnectedLink();
            if (!wowRealmStatus.Error)
            {
                string str = CheckRealmStatus(settings.RealmStatusType);

                if (str == "Up")
                {
                    settings.RealmStatusType = wowRealmStatus.RealnStatusType;
                    Functions.WriteJSon(settings, "BotSettings");
                    string[] str1 = new string[2];
                    str1[0] = "**Тех. Обслуживание закончилось!**";
                    str1[1] = $"Игровой мир: **{wowRealmStatus.RealmName}** работает!\u2705";
                    return str1;

                }
                else if (str == "Down")
                {
                    settings.RealmStatusType = wowRealmStatus.RealnStatusType;
                    Functions.WriteJSon(settings, "BotSettings");
                    string[] str1 = new string[2];
                    str1[0] = "**Тех. Обслуживание началось!**";
                    str1[1] = $"Игровой мир: **{wowRealmStatus.RealmName}** не работает!\u274c";

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
        private void GetRealmConnectedLink()
        {

            RealmInfo realm = Functions.GetWebJson<RealmInfo>($"https://eu.api.blizzard.com/data/wow/realm/{settings.RealmSlug}?namespace=dynamic-eu&locale=ru_RU&access_token={tokenWow}");
            if (realm != null)
            {
                wowRealmStatus.Error = false;
                RealmUpdateFunction(realm.id.ToString());

            }
            else
            {
                wowRealmStatus.Error = true;
            }




        }

        private void RealmUpdateFunction(string id)
        {


            ConnectRealm realm = Functions.GetWebJson<ConnectRealm>($"https://eu.api.blizzard.com/data/wow/connected-realm/{id}?namespace=dynamic-eu&locale=ru_RU&access_token={tokenWow}");
            if (realm != null)
            {
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
                wowRealmStatus.Error = false;
            }
            else
            {
                wowRealmStatus.Error = true;
            }

        }

        private string CheckRealmStatus(string beforeRealStatus)
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

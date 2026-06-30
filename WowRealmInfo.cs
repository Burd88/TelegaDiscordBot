using System;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{

    class WowRealmInfo
    {
        private WoWRealStatus wowRealmStatus;

        public async Task<WoWRealStatus> GetRealmInfo()
        {
            wowRealmStatus = new();
            await GetRealmConnectedLink();

            return wowRealmStatus;
        }


        public async Task<string[]> GetRealmInfoForTimer()
        {
            settings = Functions.ReadJson<BotSettings>("BotSettings");

            wowRealmStatus = new();
            await GetRealmConnectedLink();
            if (!wowRealmStatus.Error)
            {
                string str = CheckRealmStatus(settings.RealmStatusType);

                if (str == "Up")
                {
                    try
                    {
                        settings.RealmStatusType = wowRealmStatus.RealnStatusType;

                        Functions.WriteJSon(settings, "BotSettings");

                        string[] str1 = new string[2];

                        str1[0] = "**Техническое обслуживание закончилось!**";

                        str1[1] = $"Игровой мир: **{wowRealmStatus.RealmName}** работает!\u2705";

                        return str1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return null;
                    }


                }
                else if (str == "Down")
                {
                    try
                    {
                        settings.RealmStatusType = wowRealmStatus.RealnStatusType;
                        Functions.WriteJSon(settings, "BotSettings");
                        string[] str1 = new string[2];
                        str1[0] = "**Техническое обслуживание началось!**";
                        str1[1] = $"Игровой мир: **{wowRealmStatus.RealmName}** не работает!\u274c";

                        return str1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return null;
                    }
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
        private async Task GetRealmConnectedLink()
        {
            RealmInfo realm = await Functions.GetWebJson<RealmInfo>($"https://eu.api.blizzard.com/data/wow/realm/{settings.RealmSlug}?namespace=dynamic-eu&locale={settings.Locale}");

            if (realm != null)
            {

                wowRealmStatus.Error = false;
                await RealmUpdateFunction(realm.id.ToString());

            }
            else
            {
                wowRealmStatus.Error = true;
            }




        }

        private async Task RealmUpdateFunction(string id)
        {

            Realms realm = await Functions.GetWebJson<Realms>($"https://eu.api.blizzard.com/data/wow/connected-realm/{id}?namespace=dynamic-eu&locale={settings.Locale}");

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

                foreach (ConnectRealm realms in realm.realms)
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

}

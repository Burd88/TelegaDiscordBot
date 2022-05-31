using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class GuildAchievements
    {

        public static AllAchievements achievements = new AllAchievements() { Achievements = new List<Achievement>() };
        private static string error = "false";

        private static List<Achievement> beforeAchievement = new List<Achievement>();
        private static List<Achievement> afterAchievement = new List<Achievement>();
        public static List<Achievement> newAchievement = new List<Achievement>();


        public static void GetGuildAchievementChange()
        {
            try
            {
                beforeAchievement = new List<Achievement>();
                afterAchievement = new List<Achievement>();
                newAchievement = new List<Achievement>();

                Task<BotSettings> set = Functions.ReadJson<BotSettings>("BotSettings");
                settings = set.Result;

                GetGuildAchievements();


                if (settings.LastGuildAchiveTime != 0)
                {
                    if (afterAchievement.Count != 0 && error == "false")
                    {

                        for (int i = 0; i < afterAchievement.Count; i++)
                        {

                            if (afterAchievement[i].Time > settings.LastGuildAchiveTime)
                            {

                                newAchievement.Add(afterAchievement[i]);
                            }

                        }


                        if (newAchievement.Count != 0)
                        {
                            var last = newAchievement.Max(after => after.Time);
                            Console.WriteLine($"LastGuildAchiveTime write new : {last}\n");
                            Functions.WriteJSon<List<Achievement>>(afterAchievement, "BeforeGuildAchievement");
                            settings.LastGuildAchiveTime = last;
                            Functions.WriteJSon(settings, "BotSettings");

                        }

                    }
                }
                else
                {
                    var last = afterAchievement.Max(after => after.Time);
                    Console.WriteLine($"LastGuildAchiveTime write : {last}\n");
                    settings.LastGuildAchiveTime = last;
                    Functions.WriteJSon(settings, "BotSettings");

                }
            }

            catch (Exception e)
            {
                error = "true";
                string message = $"GetGuildAchievementChange Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }

        }

        public static void GetGuildAchievements()
        {

            try
            {
                achievements = new() { Achievements = new List<Achievement>() };
                WebRequest requesta = WebRequest.Create($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")}/achievements?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);
                WebResponse responcea = requesta.GetResponse();

                using (Stream stream = responcea.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {



                            GuildAchievement achievementsAll = JsonConvert.DeserializeObject<GuildAchievement>(line);


                            if (achievementsAll.recent_events != null)
                            {

                                for (int i = 0; i < achievementsAll.recent_events.Count; i++)
                                {
                                    if (Convert.ToInt64(achievementsAll.recent_events[i].timestamp) > settings.LastGuildAchiveTime)
                                    {
                                        GetGuildAchievementsRU(achievementsAll.recent_events[i].achievement.key.href, achievementsAll.recent_events[i].timestamp.ToString());
                                    }



                                }

                            }
                            //WriteAchievementsInFile();

                        }
                    }
                }
                responcea.Close();
                error = "false";

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    error = "true";
                    string message = $"\nGetGuildAchievements Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                error = "true";
                string message = $"GetGuildAchievements Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }

        }
        public static void GetGuildAchievementsRU(string link, string time)
        {

            try
            {
                //Console.WriteLine("https://eu.api.blizzard.com/data/wow/achievement/" + id + "?namespace=static-9.1.5_40764-eu&locale=ru_RU&access_token=" + tokenWow);
                WebRequest requesta = WebRequest.Create(link + "&locale=ru_RU&access_token=" + tokenWow);
                WebResponse responcea = requesta.GetResponse();

                using (Stream stream = responcea.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {



                            GuildAchievementMedia achievement = JsonConvert.DeserializeObject<GuildAchievementMedia>(line);
                            GetGuildAchievementsRUMedia(achievement.category.name, achievement.name, Convert.ToInt64(time), achievement.media.key.href);
                            //   afterAchievement.Add(new Achievement { Category = achievement.category.name, Name = achievement.name, Time = Convert.ToInt64(time) });






                        }
                    }
                }
                responcea.Close();
                error = "false";

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    error = "true";
                    string message = $"\nGetGuildAchievementsRU Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                error = "true";
                string message = $"GetGuildAchievementsRU Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }

        }
        public static void GetGuildAchievementsRUMedia(string category, string name, long time, string link)
        {

            try
            {
                //Console.WriteLine("https://eu.api.blizzard.com/data/wow/achievement/" + id + "?namespace=static-9.1.5_40764-eu&locale=ru_RU&access_token=" + tokenWow);
                WebRequest requesta = WebRequest.Create(link + "&locale=ru_RU&access_token=" + tokenWow);
                WebResponse responcea = requesta.GetResponse();

                using (Stream stream = responcea.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {



                            AcievCharMEdia achievement = JsonConvert.DeserializeObject<AcievCharMEdia>(line);

                            afterAchievement.Add(new Achievement { Category = category, Name = name, Time = time, Icon = achievement.assets[0].value });






                        }
                    }
                }
                responcea.Close();
                error = "false";

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    error = "true";
                    string message = $"\nGetGuildAchievementsRUMedia Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                error = "true";
                string message = $"GetGuildAchievementsRUMedia Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }

        }

    }



    public class AllAchievements
    {
        public List<Achievement> Achievements { get; set; }
    }

    public class Achievement
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public long Time { get; set; }
        public string Icon { get; set; }
    }



    public class Faction
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Guild1
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm realm { get; set; }
        public Faction faction { get; set; }
    }

    public class Achievement2
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ChildCriteria1
    {
        public int id { get; set; }
        public object amount { get; set; }
        public bool is_completed { get; set; }
    }

    public class Criteria1
    {
        public int id { get; set; }
        public bool is_completed { get; set; }
        public List<ChildCriteria1> child_criteria { get; set; }
        public int? amount { get; set; }
    }

    public class AchievementBN
    {
        public int id { get; set; }
        public Achievement2 achievement { get; set; }
        public Criteria1 criteria { get; set; }
        public object completed_timestamp { get; set; }
    }



    public class CategoryProgress
    {
        public Category category { get; set; }
        public int quantity { get; set; }
        public int points { get; set; }
    }

    public class RecentEvent
    {
        public Achievement2 achievement { get; set; }
        public object timestamp { get; set; }
    }

    public class GuildAchievement
    {
        public Links _links { get; set; }
        public Guild1 guild { get; set; }
        public int total_quantity { get; set; }
        public int total_points { get; set; }
        public List<AchievementBN> achievements { get; set; }
        public List<CategoryProgress> category_progress { get; set; }
        public List<RecentEvent> recent_events { get; set; }
    }
    public class SelfAchievMedia
    {
        public string href { get; set; }
    }

    public class LinksAchievMedia
    {
        public SelfAchievMedia self { get; set; }
    }

    public class KeyAchievMedia
    {
        public string href { get; set; }
    }

    public class CategoryAchievMedia
    {
        public KeyAchievMedia key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class OperatorAchievMedia
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class ChildCriteriaAchievMedia
    {
        public int id { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
    }

    public class CriteriaAchievMedia
    {
        public int id { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public OperatorAchievMedia @operator { get; set; }
        public List<ChildCriteriaAchievMedia> child_criteria { get; set; }
    }

    public class MediaAchievMedia
    {
        public KeyAchievMedia key { get; set; }
        public int id { get; set; }
    }

    public class GuildAchievementMedia
    {
        public LinksAchievMedia _links { get; set; }
        public int id { get; set; }
        public CategoryAchievMedia category { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int points { get; set; }
        public bool is_account_wide { get; set; }
        public CriteriaAchievMedia criteria { get; set; }
        public MediaAchievMedia media { get; set; }
        public int display_order { get; set; }
    }
}

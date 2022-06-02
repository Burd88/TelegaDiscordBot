using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class GuildAchievements
    {

        private AllAchievements achievements = new() { Achievements = new List<Achievement>() };
        private bool error = false;

        private List<Achievement> beforeAchievement = new();
        private List<Achievement> afterAchievement = new();
        private List<Achievement> newAchievement = new();


        public List<Achievement> GetGuildAchievementChange()
        {
            try
            {
                beforeAchievement = new();
                afterAchievement = new();
                newAchievement = new();
                achievements = new() { Achievements = new List<Achievement>() };
                Task<BotSettings> set = Functions.ReadJson<BotSettings>("BotSettings");
                settings = set.Result;

                GetGuildAchievements();


                if (settings.LastGuildAchiveTime != 0)
                {
                    if (afterAchievement.Count != 0 && !error)
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
                            return newAchievement;
                        }

                    }
                    return null;
                }
                else
                {

                    var last = afterAchievement.Max(after => after.Time);
                    Console.WriteLine($"LastGuildAchiveTime write : {last}\n");
                    settings.LastGuildAchiveTime = last;
                    Functions.WriteJSon(settings, "BotSettings");
                    return null;
                }
            }

            catch (Exception e)
            {

                string message = $"GetGuildAchievementChange Error: {e.Message}";
                Functions.WriteLogs(message, "error");
                return null;
            }

        }

        private void GetGuildAchievements()
        {

            GuildAchievement achievementsAll = Functions.GetWebJson<GuildAchievement>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")}/achievements?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);
            if (achievementsAll != null)
            {
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
            }
            else
            {
                error = true;
            }


        }
        private void GetGuildAchievementsRU(string link, string time)
        {


            GuildAchievementMedia achievement = Functions.GetWebJson<GuildAchievementMedia>(link + "&locale=ru_RU&access_token=" + tokenWow);
            if (achievement != null)
            {
                GetGuildAchievementsRUMedia(achievement.category.name, achievement.name, Convert.ToInt64(time), achievement.media.key.href);
            }
            else
            {
                error = true;
            }



        }
        private void GetGuildAchievementsRUMedia(string category, string name, long time, string link)
        {

            GetBNetMEdia achievement = Functions.GetWebJson<GetBNetMEdia>(link + "&locale=ru_RU&access_token=" + tokenWow);
            if (achievement != null)
            {
                afterAchievement.Add(new Achievement { Category = category, Name = name, Time = time, Icon = achievement.assets[0].value });

            }
            else
            {
                error = true;
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

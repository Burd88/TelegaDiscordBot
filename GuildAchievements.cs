using System;
using System.Collections.Generic;
using System.Linq;
using static DiscordBot.Program;

namespace DiscordBot
{
    class GuildAchievements
    {

        private bool error = false;
        private List<Achievement> afterAchievement;
        private List<Achievement> newAchievement;

        public List<Achievement> GetGuildAchievementChange()
        {
            try
            {
                afterAchievement = new();
                newAchievement = new();

                settings = Functions.ReadJson<BotSettings>("BotSettings");

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

            GuildAchievement achievementsAll = Functions.GetWebJson<GuildAchievement>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.GuildName.ToLower().Replace(" ", "-")}/achievements?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);
            if (achievementsAll != null)
            {
                if (achievementsAll.recent_events != null)
                {

                    for (int i = 0; i < achievementsAll.recent_events.Count; i++)
                    {

                        if (Convert.ToInt64(achievementsAll.recent_events[i].timestamp) > settings.LastGuildAchiveTime)
                        {
                            Achievement achievement = new();
                            achievement.Time = Convert.ToInt64(achievementsAll.recent_events[i].timestamp.ToString());

                            GuildAchievementMedia achievementInfo = Functions.GetWebJson<GuildAchievementMedia>(achievementsAll.recent_events[i].achievement.key.href + "&locale={settings.Locale}&access_token=" + tokenWow);
                            if (achievementInfo != null)
                            {
                                achievement.Name = achievementInfo.name;
                                achievement.Category = achievementInfo.category.name;
                                // GetGuildAchievementsRUMedia(achievementInfo.category.name, achievementInfo.name, Convert.ToInt64(time), achievementInfo.media.key.href);
                                GetBNetMEdia achievementMedia = Functions.GetWebJson<GetBNetMEdia>(achievementInfo.media.key.href + "&locale={settings.Locale}&access_token=" + tokenWow);
                                if (achievementMedia != null)
                                {
                                    achievement.Icon = achievementMedia.assets[0].value;
                                    //  afterAchievement.Add(new Achievement { Category = category, Name = name, Time = time, Icon = achievementMedia.assets[0].value });
                                    afterAchievement.Add(achievement);
                                }
                                else
                                {
                                    error = true;
                                }
                            }
                            else
                            {
                                error = true;
                            }
                        }
                        else
                        {
                            error = true;
                        }



                    }

                }
                else
                {
                    error = true;
                }
            }
            else
            {
                error = true;
            }


        }
        /*
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
        */
    }

}

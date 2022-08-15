using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class GuildActivity
    {

        private bool error = false;


        private List<Activity> afterActivity;
    
        private List<Activity> newActivity;


        public List<Activity> GetGuildActivityChange()
        {
            try
            {
               
                afterActivity = new();
                newActivity = new();
                settings = Functions.ReadJson<BotSettings>("BotSettings");
              

                GetGuildActivityNew();
               
                

                if (settings.LastGuildActiveTime != 0)
                {
                    if (afterActivity.Count != 0 && error == false)
                    {

                        for (int i = 0; i < afterActivity.Count; i++)
                        {

                            if (afterActivity[i].Time > settings.LastGuildActiveTime)
                            {

                                newActivity.Add(afterActivity[i]);
                            }

                        }


                        if (newActivity.Count != 0)
                        {
                            var last = newActivity.Max(after => after.Time);
                           
                            settings.LastGuildActiveTime = last;
                            Functions.WriteJSon(settings, "BotSettings");
                            return newActivity;
                        }

                    }
                    return null;
                }
                else
                {
                    var last = afterActivity.Max(after => after.Time);
                   
                    settings.LastGuildActiveTime = last;
                   
                    Functions.WriteJSon(settings, "BotSettings");
                    return null;
                }

            }

            catch (Exception e)
            {

                string message = $"GetGuildActivityChange Error: {e.Message}";
                Functions.WriteLogs(message, "error");
                return null;
            }

        }


        private void GetGuildActivityNew()
        {
            GuildActivitys activity = Functions.GetWebJson<GuildActivitys>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")}/activity?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);

            if (activity != null)
            {
                if (activity.activities != null)
                {

                    for (int i = 0; i < activity.activities.Count; i++)
                    {
                        if (Convert.ToInt64(activity.activities[i].timestamp) > settings.LastGuildActiveTime)
                        {
                            Activity activNew = new();
                            if (activity.activities[i].activity.type == "CHARACTER_ACHIEVEMENT")
                            {
                                activNew.Name = activity.activities[i].character_achievement.character.name;
                                activNew.Mode = activity.activities[i].character_achievement.achievement.name;
                                activNew.Time = Convert.ToInt64(activity.activities[i].timestamp);
                                activNew.Type = "CHARACTER_ACHIEVEMENT";
                                AchievChar activityInfo = Functions.GetWebJson<AchievChar>($"{activity.activities[i].character_achievement.achievement.key.href}&locale=ru_RU&access_token={tokenWow}");
                                if (activityInfo != null)
                                {
                                    activNew.Categor = activityInfo.category.name;
                                    activNew.Award = activityInfo.reward_description;
                                    GetBNetMEdia activityMedia = Functions.GetWebJson<GetBNetMEdia>($"{activityInfo.media.key.href}&locale=ru_RU&access_token={tokenWow}");
                                    if (activityMedia != null)
                                    {
                                        foreach (Asset asset in activityMedia.assets)
                                        {
                                            activNew.Icon = asset.value;
                                          //  afterActivity.Add(new Activity() { Name = name, Mode = achivname, Time = time, Icon = asset.value, Award = award, Categor = category, Type = type });

                                        }
                                    }
                                    else
                                    {
                                        error = true;
                                    }
                                    // GetGuildActivityInfoMedia(name, achivname, time, activity.category.name, activity.reward_description, activity.media.key.href, type);
                                }
                                else
                                {
                                    activNew.Categor = null;
                                    activNew.Award = null;
                                    activNew.Icon = null;
                                    //afterActivity.Add(new Activity() { Name = name, Mode = achivname, Time = time, Icon = null, Award = null, Categor = null, Type = type });
                                }
                               // GetGuildActivityInfo(activity.activities[i].character_achievement.character.name, activity.activities[i].character_achievement.achievement.name, Convert.ToInt64(activity.activities[i].timestamp), activity.activities[i].character_achievement.achievement.key.href, "CHARACTER_ACHIEVEMENT");

                            }
                            else if (activity.activities[i].activity.type == "ENCOUNTER")
                            {
                                activNew.Name = activity.activities[i].encounter_completed.encounter.name;
                                activNew.Mode = activity.activities[i].encounter_completed.mode.name;
                                activNew.Time = Convert.ToInt64(activity.activities[i].timestamp);
                                activNew.Type = "ENCOUNTER";
                                //GetGuildActivityInfo(activity.activities[i].encounter_completed.encounter.name, activity.activities[i].encounter_completed.mode.name, Convert.ToInt64(activity.activities[i].timestamp), activity.activities[i].encounter_completed.encounter.key.href, "ENCOUNTER");
                                BossKill activityInfo = Functions.GetWebJson<BossKill>($"{activity.activities[i].encounter_completed.encounter.key.href}&locale=ru_RU&access_token={tokenWow}");
                                if (activityInfo != null)
                                {
                                    activNew.Categor = activityInfo.instance.name;
                                    activNew.Award = null;
                                    GetBNetMEdia activityMedia = Functions.GetWebJson<GetBNetMEdia>($"{activityInfo.creatures[0].creature_display.key.href}&locale=ru_RU&access_token={tokenWow}");
                                    if (activityMedia != null)
                                    {
                                        foreach (Asset asset in activityMedia.assets)
                                        {
                                            activNew.Icon = asset.value;
                                            //  afterActivity.Add(new Activity() { Name = name, Mode = achivname, Time = time, Icon = asset.value, Award = award, Categor = category, Type = type });

                                        }
                                    }
                                    else
                                    {
                                        error = true;
                                    }
                                    // GetGuildActivityInfoMedia(name, achivname, time, activity.instance.name, null, activity.creatures[0].creature_display.key.href, type);
                                }
                                else
                                {
                                    activNew.Categor = null;
                                    activNew.Award = null;
                                    activNew.Icon = null;
                                    //afterActivity.Add(new Activity() { Name = name, Mode = achivname, Time = time, Icon = null, Award = null, Categor = null, Type = type });

                                }

                            }
                            afterActivity.Add(activNew);
                        }                        
                          
                               
                           
                        

                    }

                }
                
            }
            else
            {
                error = true;
            }

        }
        /*
        private void GetGuildActivityInfo(string name, string achivname, long time, string linkmedia, string type)
        {
            if (type == "CHARACTER_ACHIEVEMENT")
            {
                AchievChar activity = Functions.GetWebJson<AchievChar>($"{linkmedia}&locale=ru_RU&access_token={tokenWow}");
                if (activity != null)
                {
                    GetGuildActivityInfoMedia(name, achivname, time, activity.category.name, activity.reward_description, activity.media.key.href, type);
                }
                else
                {

                    afterActivity.Add(new Activity() { Name = name, Mode = achivname, Time = time, Icon = null, Award = null, Categor = null, Type = type });
                }

            }
            else if (type == "ENCOUNTER")
            {
                BossKill activity = Functions.GetWebJson<BossKill>($"{linkmedia}&locale=ru_RU&access_token={tokenWow}");
                if (activity != null)
                {
                    GetGuildActivityInfoMedia(name, achivname, time, activity.instance.name, null, activity.creatures[0].creature_display.key.href, type);
                }
                else
                {
                    afterActivity.Add(new Activity() { Name = name, Mode = achivname, Time = time, Icon = null, Award = null, Categor = null, Type = type });

                }
            }



        }

        private void GetGuildActivityInfoMedia(string name, string achivname, long time, string category, string award, string linkmedia, string type)
        {

            GetBNetMEdia activity = Functions.GetWebJson<GetBNetMEdia>($"{linkmedia}&locale=ru_RU&access_token={tokenWow}");
            if (activity != null)
            {
                foreach (Asset asset in activity.assets)
                {
                    afterActivity.Add(new Activity() { Name = name, Mode = achivname, Time = time, Icon = asset.value, Award = award, Categor = category, Type = type });

                }
            }
            else
            {
                error = true;
            }
        }
        */
    }


}

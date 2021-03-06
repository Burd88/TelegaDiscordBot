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
            ActivityAll activity = Functions.GetWebJson<ActivityAll>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")}/activity?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);

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


    #region Classes Activity
    class AllActivitys
    {
        public List<Activity> activity { get; set; }
    }
    class Activity
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Categor { get; set; }
        public string Icon { get; set; }
        public string Award { get; set; }
        public string Mode { get; set; }
        public long Time { get; set; }
    }

    public class ActivityAllSelf
    {
        public string href { get; set; }
    }

    public class ActivityAllLinks
    {
        public ActivityAllSelf self { get; set; }
    }

    public class ActivityAllKey
    {
        public string href { get; set; }
    }

    public class ActivityAllRealm
    {
        public ActivityAllKey key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class ActivityAllFaction
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class ActivityAllGuild
    {
        public ActivityAllKey key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public ActivityAllRealm realm { get; set; }
        public ActivityAllFaction faction { get; set; }
    }

    public class ActivityAllEncounter
    {
        public ActivityAllKey_encou key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }
    public class ActivityAllKey_encou
    {
        public string href { get; set; }
    }
    public class ActivityAllMode
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class ActivityAllEncounterCompleted
    {
        public ActivityAllEncounter encounter { get; set; }
        public ActivityAllMode mode { get; set; }
    }

    public class ActivityAllActivity2
    {
        public string type { get; set; }
    }

    public class ActivityAllKey_charakter
    {
        public string href { get; set; }
    }
    public class ActivityAllCharacter
    {
        public ActivityAllKey_charakter key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public ActivityAllRealm_char realm { get; set; }
    }
    public class ActivityAllRealm_char
    {
        public ActivityAllKey_realm_achiv key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }
    public class ActivityAllKey_realm_achiv
    {
        public string href { get; set; }
    }
    public class ActivityAllKey_achiv
    {
        public string href { get; set; }
    }
    public class ActivityAllAchievement
    {
        public ActivityAllKey_achiv key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ActivityAllCharacterAchievement
    {
        public ActivityAllCharacter character { get; set; }
        public ActivityAllAchievement achievement { get; set; }
    }

    public class ActivityAllActivity
    {
        public ActivityAllEncounterCompleted encounter_completed { get; set; }
        public ActivityAllActivity2 activity { get; set; }
        public string timestamp { get; set; }
        public ActivityAllCharacterAchievement character_achievement { get; set; }
    }

    public class ActivityAll
    {
        public ActivityAllLinks _links { get; set; }
        public ActivityAllGuild guild { get; set; }
        public List<ActivityAllActivity> activities { get; set; }
    }
    #endregion




    public class Asset
    {
        public string key { get; set; }
        public string value { get; set; }
        public int file_data_id { get; set; }
    }

    public class GetBNetMEdia
    {
        public Links _links { get; set; }
        public List<Asset> assets { get; set; }
        public int id { get; set; }
    }



    public class Category
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Criteria
    {
        public int id { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
    }

    public class Media
    {
        public Key key { get; set; }
        public int id { get; set; }
    }

    public class AchievChar
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int points { get; set; }
        public bool is_account_wide { get; set; }
        public Criteria criteria { get; set; }
        public string reward_description { get; set; }
        public Media media { get; set; }
        public int display_order { get; set; }
    }



    public class CreatureDisplay
    {
        public Key key { get; set; }
        public int id { get; set; }
    }

    public class Creature
    {
        public int id { get; set; }
        public string name { get; set; }
        public CreatureDisplay creature_display { get; set; }
    }

    public class Item2
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public Item2 item { get; set; }
    }

    public class Spell
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Section
    {
        public int id { get; set; }
        public string title { get; set; }
        public Spell spell { get; set; }
        public string body_text { get; set; }
        public List<Section> sections { get; set; }
        public CreatureDisplay creature_display { get; set; }
    }


    public class CategoryBossKill
    {
        public string type { get; set; }
    }

    public class ModeBN
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class BossKill
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Creature> creatures { get; set; }
        public List<Item> items { get; set; }
        public List<Section> sections { get; set; }
        public Instance instance { get; set; }
        public CategoryBossKill category { get; set; }
        public List<ModeBN> modes { get; set; }
    }

}

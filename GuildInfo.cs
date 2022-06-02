using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    public class GuildInfoFull
    {
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Leader { get; set; }
        public string MemberCount { get; set; }
        public string Achievement { get; set; }
        public string TimeCreate { get; set; }
        public string RaidProgress { get; set; }
        public string RaidRankName { get; set; }
        public string RaidRankRealm { get; set; }
        public string RaidRankWorld { get; set; }
        public string RaidRankRegion { get; set; }
        public string RAidFull { get; set; }
        public bool Error { get; set; }
    }
    class GuildInfo
    {
        #region Получение инфы о гильдии
        private GuildInfoFull _guildInfoFull;
        public GuildInfoFull GetGuildInfo()
        {
            _guildInfoFull = new();
            Guild_raid_progress();
            GetGuildOtherInfo();
            GetGuildRosterInfo();
            if (!_guildInfoFull.Error)
            {
                return _guildInfoFull;
            }
            else
            {
                return null;
            }
        }

        private void GetGuildRosterInfo()
        {
            MainGuild guild = Functions.GetWebJson<MainGuild>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")}/roster?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);
            if (guild != null)
            {
                foreach (Member_guild character in guild.members)
                {
                    if (character.rank == 0)
                    {
                        _guildInfoFull.Leader = $"{character.character.name}";
                    }
                }
            }
            else
            {
                _guildInfoFull.Error = true;
            }

        }


        private void GetGuildOtherInfo()
        {
            GuildMain guildmain = Functions.GetWebJson<GuildMain>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")}?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);
            if (guildmain != null)
            {
                _guildInfoFull.TimeCreate = $"{Functions.FromUnixTimeStampToDateTime(guildmain.created_timestamp.ToString())}";
                _guildInfoFull.Name = guildmain.name;
                _guildInfoFull.Achievement = $"{guildmain.achievement_points}";
                _guildInfoFull.MemberCount = $"{guildmain.member_count}";
                _guildInfoFull.Faction = $"{guildmain.faction.name}";
            }
            else
            {
                _guildInfoFull.Error = true;
            }
        }


        private void Guild_raid_progress()
        {
            GuildRaiderIO rio_guild = Functions.GetWebJson<GuildRaiderIO>($"https://raider.io/api/v1/guilds/profile?region=eu&realm={settings.RealmSlug}&name={settings.Guild.ToLower()}&fields=raid_progression%2Craid_rankings");
            if (rio_guild != null)
            {
                _guildInfoFull.RaidProgress = rio_guild.raid_progression.SepulcherOfTheFirstOnes.summary;
                if (rio_guild.raid_rankings.SepulcherOfTheFirstOnes.mythic.world == 0)
                {
                    if (rio_guild.raid_rankings.SepulcherOfTheFirstOnes.heroic.world == 0)
                    {
                        if (rio_guild.raid_rankings.SepulcherOfTheFirstOnes.normal.world == 0)
                        {
                            _guildInfoFull.RaidRankName = "**Сложность**: Обычный";
                            _guildInfoFull.RaidRankWorld = "**Мир**: -";
                            _guildInfoFull.RaidRankRegion = "**Регион**: -";
                            _guildInfoFull.RaidRankRealm = "**Сервер**: -";

                        }
                        else
                        {
                            _guildInfoFull.RaidRankName = "**Сложность**: Обычный";
                            _guildInfoFull.RaidRankWorld = $"**Мир**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.normal.world} место";
                            _guildInfoFull.RaidRankRegion = $"**Регион**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.normal.region} место";
                            _guildInfoFull.RaidRankRealm = $"**Сервер**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.normal.realm} место";
                        }


                    }
                    else
                    {
                        _guildInfoFull.RaidRankName = "**Сложность**: Героический";
                        _guildInfoFull.RaidRankWorld = $"**Мир**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.heroic.world} место";
                        _guildInfoFull.RaidRankRegion = $"**Регион**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.heroic.region} место";
                        _guildInfoFull.RaidRankRealm = $"**Сервер**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.heroic.realm} место";
                    }

                }
                else
                {
                    _guildInfoFull.RaidRankName = "**Сложность**: Мифический";
                    _guildInfoFull.RaidRankWorld = $"**Мир**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.mythic.world} место";
                    _guildInfoFull.RaidRankRegion = $"**Регион**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.mythic.region} место";
                    _guildInfoFull.RaidRankRealm = $"**Сервер**: {rio_guild.raid_rankings.SepulcherOfTheFirstOnes.mythic.realm} место";
                }
                _guildInfoFull.RAidFull = $"{_guildInfoFull.RaidProgress}\n{ _guildInfoFull.RaidRankWorld}\n{ _guildInfoFull.RaidRankRegion}\n{ _guildInfoFull.RaidRankRealm}";

            }
            else
            {
                _guildInfoFull.Error = true;
            }

        }
        #endregion

        #region Проверка состава гильдии на встплуние и покидание
        private List<RosterLeaveInv> beforeRoster;
        private List<RosterLeaveInv> afterRoster;
        private List<RosterLeaveInv> inviteRoster;
        private List<RosterLeaveInv> leaveRoster;
        private bool error = false;
        public List<RosterLeaveInv> GetInviteRoster()
        {
            if (inviteRoster != null && inviteRoster.Count != 0)
            {
                return inviteRoster;
            }
            else
            {
                return null;
            }
        }
        public List<RosterLeaveInv> GetLeaveRoster()
        {
            if (leaveRoster != null && leaveRoster.Count != 0)
            {
                return leaveRoster;
            }
            else
            {
                return null;
            }
        }
        public void GetGuildRosterChange()
        {
            try
            {


                beforeRoster = new();
                afterRoster = new();
                inviteRoster = new();
                leaveRoster = new();

                Task<List<RosterLeaveInv>> befordata = Functions.ReadJson<List<RosterLeaveInv>>("BeforeRoster");
                beforeRoster = befordata.Result;
                GetGuildRosterFull();
                //Functions.WriteJSon<List<RosterLeaveInv>>(afterRoster, "BeforeRoster");
                if (beforeRoster.Count != 0)
                {
                    if (afterRoster.Count != 0 && !error)
                    {
                        leaveRoster = beforeRoster;

                        for (int i = 0; i < afterRoster.Count; i++)
                        {
                            var leave = beforeRoster.Find(x => x.Name == afterRoster[i].Name);

                            if (leave != null)
                            {
                                leaveRoster.RemoveAll(x => x.Name == leave.Name);
                            }
                            else
                            {
                                inviteRoster.Add(afterRoster[i]);
                            }
                        }

                        if (leaveRoster.Count != 0 || inviteRoster.Count != 0)
                        {
                            Functions.WriteJSon(afterRoster, "BeforeRoster");
                        }
                    }
                }
                else
                {
                    Functions.WriteJSon(afterRoster, "BeforeRoster");
                }
            }

            catch (Exception e)
            {

                string message = $"GetGuildRosterChange Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
        private void GetGuildRosterFull()
        {
            MainGuild guild = Functions.GetWebJson<MainGuild>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")}/roster?namespace=profile-eu&locale=ru_RU&access_token=" + tokenWow);
            if (guild != null)
            {
                foreach (Member_guild character in guild.members)
                {
                    afterRoster.Add(new RosterLeaveInv { Name = character.character.name, Class = character.character.playable_class.id.ToString(), LVL = character.character.level.ToString(), Race = character.character.playable_race.id.ToString(), Rank = character.rank });

                }
            }
            else
            {
                error = true;
            }


        }
        #endregion
    }





    #region Guild Members Info
    public class RosterLeaveInv
    {
        public string Name { get; set; }
        public string LVL { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }
        public int Rank { get; set; }
    }
    public class Guild
    {

        public int ID { get; set; }
        public string Region { get; set; }
        public string Realm { get; set; }
        public string GuildName { get; set; }
        public string Local { get; set; }
        public string RealmSlug { get; set; }
        public string GuildSlug { get; set; }
        public string LocalSlug { get; set; }

    }


    public class GuildRosterMain
    {

        public int Level { get; set; }
        public string Race { get; set; }
        public string Class { get; set; }
        public string Rank { get; set; }
        public string Name { get; set; }

    }

    public class Self_guild
    {
        public string href { get; set; }
    }

    public class Links_guild
    {
        public Self_guild self { get; set; }
    }

    public class Key_guild
    {
        public string href { get; set; }
    }

    public class Realm_guild
    {
        public Key_guild key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Faction_guild
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class Guild_guild
    {
        public Key_guild key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm_guild realm { get; set; }
        public Faction_guild faction { get; set; }
    }

    public class PlayableClass_guild
    {
        public Key_guild key { get; set; }
        public int id { get; set; }
    }

    public class PlayableRace_guild
    {
        public Key_guild key { get; set; }
        public int id { get; set; }
    }

    public class Character_guild
    {
        public Key_guild key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm_guild realm { get; set; }
        public int level { get; set; }
        public PlayableClass_guild playable_class { get; set; }
        public PlayableRace_guild playable_race { get; set; }
    }

    public class Member_guild
    {
        public Character_guild character { get; set; }
        public int rank { get; set; }
    }

    public class MainGuild
    {
        public Links_guild _links { get; set; }
        public Guild_guild guild { get; set; }
        public List<Member_guild> members { get; set; }
    }
    #endregion

    #region RaiderIo


    public class GuildNormal
    {
        public int world { get; set; }
        public int region { get; set; }
        public int realm { get; set; }
    }

    public class GuildHeroic
    {
        public int world { get; set; }
        public int region { get; set; }
        public int realm { get; set; }
    }

    public class GuildMythic
    {
        public int world { get; set; }
        public int region { get; set; }
        public int realm { get; set; }
    }

    public class GuildCastleNathria
    {
        public GuildNormal normal { get; set; }
        public GuildHeroic heroic { get; set; }
        public GuildMythic mythic { get; set; }
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class GuildSanctumOfDomination
    {
        public GuildNormal normal { get; set; }
        public GuildHeroic heroic { get; set; }
        public GuildMythic mythic { get; set; }
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class GuildSepulcherOfTheFirstOnes
    {
        public GuildNormal normal { get; set; }
        public GuildHeroic heroic { get; set; }
        public GuildMythic mythic { get; set; }
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }

    public class GuildRaidRankings
    {
        [JsonProperty("castle-nathria")]
        public GuildCastleNathria CastleNathria { get; set; }

        [JsonProperty("sanctum-of-domination")]
        public GuildSanctumOfDomination SanctumOfDomination { get; set; }

        [JsonProperty("sepulcher-of-the-first-ones")]
        public GuildSepulcherOfTheFirstOnes SepulcherOfTheFirstOnes { get; set; }
    }

    public class GuildRaidProgression
    {
        [JsonProperty("castle-nathria")]
        public GuildCastleNathria CastleNathria { get; set; }

        [JsonProperty("sanctum-of-domination")]
        public GuildSanctumOfDomination SanctumOfDomination { get; set; }

        [JsonProperty("sepulcher-of-the-first-ones")]
        public GuildSepulcherOfTheFirstOnes SepulcherOfTheFirstOnes { get; set; }
    }

    public class GuildRaiderIO
    {
        public string name { get; set; }
        public string faction { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public GuildRaidRankings raid_rankings { get; set; }
        public GuildRaidProgression raid_progression { get; set; }
    }
    #endregion

    #region Main Guild Info
    public class GuildMainSelf
    {
        public string href { get; set; }
    }

    public class GuildMainLinks
    {
        public GuildMainSelf self { get; set; }
    }

    public class GuildMainFaction
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class GuildMainKey
    {
        public string href { get; set; }
    }

    public class GuildMainRealm
    {
        public GuildMainKey key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class GuildMainMedia
    {
        public GuildMainKey key { get; set; }
        public int id { get; set; }
    }

    public class GuildMainRgba
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public double a { get; set; }
    }

    public class GuildMainColor
    {
        public int id { get; set; }
        public GuildMainRgba rgba { get; set; }
    }

    public class GuildMainEmblem
    {
        public int id { get; set; }
        public GuildMainMedia media { get; set; }
        public GuildMainColor color { get; set; }
    }

    public class GuildMainBorder
    {
        public int id { get; set; }
        public GuildMainMedia media { get; set; }
        public GuildMainColor color { get; set; }
    }

    public class GuildMainBackground
    {
        public GuildMainColor color { get; set; }
    }

    public class GuildMainCrest
    {
        public GuildMainEmblem emblem { get; set; }
        public GuildMainBorder border { get; set; }
        public GuildMainBackground background { get; set; }
    }

    public class GuildMainRoster
    {
        public string href { get; set; }
    }

    public class GuildMainAchievements
    {
        public string href { get; set; }
    }

    public class GuildMainActivity
    {
        public string href { get; set; }
    }

    public class GuildMain
    {
        public GuildMainLinks _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public GuildMainFaction faction { get; set; }
        public int achievement_points { get; set; }
        public int member_count { get; set; }
        public GuildMainRealm realm { get; set; }
        public GuildMainCrest crest { get; set; }
        public GuildMainRoster roster { get; set; }
        public GuildMainAchievements achievements { get; set; }
        public long created_timestamp { get; set; }
        public GuildMainActivity activity { get; set; }
    }

    #endregion
}

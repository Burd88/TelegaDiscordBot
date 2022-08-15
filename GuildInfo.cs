using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
  
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
                foreach (Member character in guild.members)
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

                beforeRoster = Functions.ReadJson<List<RosterLeaveInv>>("Roster");
                
                GetGuildRosterFull();
                //Functions.WriteJSon<List<RosterLeaveInv>>(afterRoster, "BeforeRoster");
                if (beforeRoster != null && beforeRoster.Count != 0 )
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
                            Functions.WriteJSon(afterRoster, "Roster");
                        }
                    }
                }
                else
                {
                    Functions.WriteJSon(afterRoster, "Roster");
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
                foreach (Member character in guild.members)
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

}

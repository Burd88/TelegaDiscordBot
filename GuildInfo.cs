using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{

    class GuildInfo
    {
        #region Получение инфы о гильдии
        private GuildInfoFull _guildInfoFull;
        public async Task<GuildInfoFull> GetGuildInfo()
        {
            _guildInfoFull = new();

            await Guild_raid_progress();
            await GetGuildOtherInfo();
            await GetGuildRosterInfo();
            if (!_guildInfoFull.Error)
            {
                return _guildInfoFull;
            }
            else
            {
                return null;
            }
        }

        private async Task GetGuildRosterInfo()
        {
            MainGuild guild = await Functions.GetWebJson<MainGuild>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.GuildName.ToLower().Replace(" ", "-")}/roster?namespace=profile-eu&locale=ru_RU");

            if (guild != null)
            {
                foreach (Member character in guild.members)
                {
                    Console.WriteLine(character.character.name);
                    if (character.rank == 0)
                    {
                        _guildInfoFull.Leader = $"{character.character.name}";
                    }
                }
            }
            else
            {
                Console.WriteLine(1);
                _guildInfoFull.Error = true;
            }

        }


        private async Task GetGuildOtherInfo()
        {
            GuildMain guildmain = await Functions.GetWebJson<GuildMain>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.GuildName.ToLower().Replace(" ", "-")}?namespace=profile-eu&locale=ru_RU");
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
                Console.WriteLine(2);
                _guildInfoFull.Error = true;
            }
        }


        private async Task Guild_raid_progress()
        {
            GuildRaiderIO rio_guild = await Functions.GetWebJson<GuildRaiderIO>($"https://raider.io/api/v1/guilds/profile?region=eu&realm={settings.RealmSlug}&name={settings.GuildName.ToLower()}&fields=raid_progression%2Craid_rankings");

            if (rio_guild != null)
            {
                _guildInfoFull.RaidProgress = rio_guild.raid_progression.midNightOneSeason.summary;
                GuildRaidProgressionFull raidProgression = rio_guild.raid_progression.midNightOneSeason;
                GuildRaidProgressionFull raidRankings = rio_guild.raid_rankings.midNightOneSeason;
                Console.WriteLine(rio_guild.raid_progression.midNightOneSeason.summary);
                if (raidProgression.mythic_bosses_killed == 0)
                {
                    if (raidProgression.heroic_bosses_killed == 0)
                    {
                        if (raidProgression.normal_bosses_killed == 0)
                        {
                            _guildInfoFull.RaidRankName = "**Сложность**: Обычный";
                            _guildInfoFull.RaidRankWorld = "**Мир**: -";
                            _guildInfoFull.RaidRankRegion = "**Регион**: -";
                            _guildInfoFull.RaidRankRealm = "**Сервер**: -";

                        }
                        else
                        {
                            _guildInfoFull.RaidRankName = "**Сложность**: Обычный";
                            _guildInfoFull.RaidRankWorld = $"**Мир**: {raidRankings.normal.world} место";
                            _guildInfoFull.RaidRankRegion = $"**Регион**: {raidRankings.normal.region} место";
                            _guildInfoFull.RaidRankRealm = $"**Сервер**: {raidRankings.normal.realm} место";
                        }


                    }
                    else
                    {
                        _guildInfoFull.RaidRankName = "**Сложность**: Героический";
                        _guildInfoFull.RaidRankWorld = $"**Мир**: {raidRankings.heroic.world} место";
                        _guildInfoFull.RaidRankRegion = $"**Регион**: {raidRankings.heroic.region} место";
                        _guildInfoFull.RaidRankRealm = $"**Сервер**: {raidRankings.heroic.realm} место";
                    }

                }
                else
                {
                    _guildInfoFull.RaidRankName = "**Сложность**: Мифический";
                    _guildInfoFull.RaidRankWorld = $"**Мир**: {raidRankings.mythic.world} место";
                    _guildInfoFull.RaidRankRegion = $"**Регион**: {raidRankings.mythic.region} место";
                    _guildInfoFull.RaidRankRealm = $"**Сервер**: {raidRankings.mythic.realm} место";
                }
                _guildInfoFull.RAidFull = $"{_guildInfoFull.RaidProgress}\n{_guildInfoFull.RaidRankWorld}\n{_guildInfoFull.RaidRankRegion}\n{_guildInfoFull.RaidRankRealm}";

            }
            else
            {
                Console.WriteLine(3);
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
        public async Task GetGuildRosterChange()
        {
            try
            {


                beforeRoster = new();
                afterRoster = new();
                inviteRoster = new();
                leaveRoster = new();

                beforeRoster = Functions.ReadJson<List<RosterLeaveInv>>("Roster");

                afterRoster = await GetGuildRosterFull();
                //Functions.WriteJSon<List<RosterLeaveInv>>(afterRoster, "BeforeRoster");
                if (beforeRoster != null && beforeRoster.Count != 0)
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
                    Console.WriteLine("1afterRoster - " + afterRoster.Count);
                    Functions.WriteJSon(afterRoster, "Roster");
                }
            }

            catch (Exception e)
            {

                string message = $"GetGuildRosterChange Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
        private async Task<List<RosterLeaveInv>> GetGuildRosterFull()
        {
            List<RosterLeaveInv> rosterList = new();
            MainGuild guild = await Functions.GetWebJson<MainGuild>($"https://eu.api.blizzard.com/data/wow/guild/{settings.RealmSlug}/{settings.GuildName.ToLower().Replace(" ", "-")}/roster?namespace=profile-eu&locale=ru_RU");
            if (guild != null)
            {
                foreach (Member character in guild.members)

                {
                    //Console.WriteLine(character.character.name + " " + character.character.level);
                    rosterList.Add(new RosterLeaveInv { Name = character.character.name, Class = character.character.playable_class.id.ToString(), LVL = character.character.level.ToString(), Race = character.character.playable_race.id.ToString(), Rank = character.rank });

                }
                return rosterList;
            }
            else
            {
                error = true;
                return null;
            }


        }
        #endregion
    }

}

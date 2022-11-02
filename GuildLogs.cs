using System;
using System.Collections.Generic;
namespace DiscordBot
{


    class GuildLogs
    {
        private Log _log;

        public Log GetLogsInfo()
        {
            _log = new();

            Update_warcraftlogs_data();
            if (!_log.Error)
            {
                return _log;
            }
            else
            {
                return null;
            }
        }

        public Log GetGuildLogChange()
        {
            try
            {
                _log = new();

                Update_warcraftlogs_data();
                if (!_log.Error)
                {
                    return _log;
                }
                return null;
            }

            catch (Exception e)
            {

                string message = $"GetGuildActivityChange Error: {e.Message}";
                Functions.WriteLogs(message, "error");
                return null;
            }

        }



        private void Update_warcraftlogs_data()
        {
            List<Logs_all> logs = Functions.GetWebJson<List<Logs_all>>($"https://www.warcraftlogs.com/v1/reports/guild/{Program.settings.GuildName.ToLower()}/{Functions.GetRealmSlug(Program.settings.RealmName)}/eu?api_key=c2c9093c70e642ac6ec003d4b0904c33");
            if (logs != null)
            {
                if (logs[0].zone == 29)
                {

                    GetLogInfo(logs[0].id, logs[0].title, Functions.FromUnixTimeStampToDateTime(logs[0].start).ToString(), $"https://ru.warcraftlogs.com/reports/{logs[0].id}");
                }
                else
                {
                    _log.Error = true;
                    return;
                }


            }
            else
            {
                _log.Error = true;
                return;
            }



        }

        private void GetLogInfo(string id, string dungeon, string date, string link)
        {
            LogInfo log = Functions.GetWebJson<LogInfo>("https://www.warcraftlogs.com/v1/report/fights/" + id + "?translate=true&api_key=c2c9093c70e642ac6ec003d4b0904c33");
            if (log != null)
            {
                int kills = 0;

                int wipe = 0;
                if (log.fights != null)
                {

                    foreach (Fight fight in log.fights)
                    {
                        if (fight.boss > 0)
                        {
                            if (fight.kill == true)
                            {
                                kills++;
                                _log.BossKilling.Add(Functions.GetEncounter(fight.name).EncounterRU + GetModeInst(fight.difficulty));
                                _log.BestWipeTryPer = 0.0;
                            }
                            else
                            {
                                wipe++;
                                if (!_log.BossKilling.Contains(fight.name))
                                {
                                    if (_log.BestWipeTryPer == 0.0)
                                    {
                                        _log.BestWipeTryPer = Convert.ToDouble(fight.bossPercentage) / 100;
                                        _log.BestWipeTryName = Functions.GetEncounter(fight.name).EncounterRU + GetModeInst(fight.difficulty);
                                    }
                                    else
                                    {
                                        if (Convert.ToDouble(fight.bossPercentage) / 100 < _log.BestWipeTryPer)
                                        {
                                            _log.BestWipeTryPer = Convert.ToDouble(fight.bossPercentage) / 100;
                                            _log.BestWipeTryName = Functions.GetEncounter(fight.name).EncounterRU + GetModeInst(fight.difficulty);
                                        }
                                    }


                                }

                            }
                        }

                    }
                    TimeSpan ts = Functions.FromUnixTimeStampToDateTime(log.end.ToString()) - Functions.FromUnixTimeStampToDateTime(log.start.ToString());
                    _log.RaidTime = $"{ts.Hours} ч {ts.Minutes} м";
                    _log.Date = date;
                    _log.Dungeon = Functions.GetInstance(dungeon).InstanceRU;
                    _log.KillBoss = kills.ToString();
                    _log.WipeBoss = wipe.ToString();
                    _log.Link = link;
                    _log.StartTime = log.start;
                    _log.ID = id;
                    _log.InstanceImg = Functions.GetInstance(dungeon).InstanceImg;
                    _log.Error = false;

                }
                else
                {
                    _log.Error = true;

                }

            }
            else
            {
                _log.Error = true;
            }



        }
        private string GetModeInst(int? difficulty)
        {
            if (difficulty == 3)
            {
                return "(Нормал)";
            }
            else if (difficulty == 4)
            {
                return "(Гер)";
            }
            else if (difficulty == 5)
            {
                return "(Миф)";
            }
            else
            {
                return "";
            }
        }
    }
}

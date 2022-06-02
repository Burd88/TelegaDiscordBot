using System;
using System.Collections.Generic;
namespace DiscordBot
{
    public class Log
    {
        public string Dungeon { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        public string KillBoss { get; set; }
        public string WipeBoss { get; set; }
        public string RaidTime { get; set; }
        public bool Error { get; set; }
        public long StartTime { get; set; }
        public string ID { get; set; }
        public List<string> BossKilling { get; set; }
        public double BestWipeTryPer { get; set; }
        public string BestWipeTryName { get; set; }
        public string InstanceImg { get; set; }

        public Log()
        {
            Dungeon = "";
            Date = "";
            Link = "";
            KillBoss = "";
            WipeBoss = "";
            RaidTime = "";
            StartTime = 0;
            Error = false;
            BossKilling = new();
            BestWipeTryPer = 0.0;
            BestWipeTryName = "";
        }
    }

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
            List<Logs_all> logs = Functions.GetWebJson<List<Logs_all>>($"https://www.warcraftlogs.com/v1/reports/guild/{Program.settings.Guild.ToLower()}/{Functions.GetRealmSlug(Program.settings.Realm)}/eu?api_key=c2c9093c70e642ac6ec003d4b0904c33");
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

    public class Logs
    {
        public string Dungeon { get; set; }
        public string Date_start { get; set; }
        public string Link { get; set; }
        public string Downloader { get; set; }
        public string ID { get; set; }

    }

    public class Logs_all
    {

        public string id { get; set; }
        public string title { get; set; }
        public string owner { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int zone { get; set; }
    }

    public class Logsw
    {


        public string Dungeon { get; set; }

        public string Date_start { get; set; }

        public string Date_end { get; set; }
        public string Link { get; set; }
        public string Downloader { get; set; }
        public int ID { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Fight
    {
        public int id { get; set; }
        public int boss { get; set; }
        public int start_time { get; set; }
        public int end_time { get; set; }
        public string name { get; set; }
        public int zoneID { get; set; }
        public string zoneName { get; set; }
        public int zoneDifficulty { get; set; }
        public int? size { get; set; }
        public int? difficulty { get; set; }
        public bool? kill { get; set; }
        public int? partial { get; set; }
        public bool? inProgress { get; set; }
        public int? bossPercentage { get; set; }
        public int? fightPercentage { get; set; }
        public int? lastPhaseAsAbsoluteIndex { get; set; }
        public int? lastPhaseForPercentageDisplay { get; set; }
        public List<int> maps { get; set; }
        public int? instances { get; set; }
        public int? groups { get; set; }
    }

    public class Friendly
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string server { get; set; }
        public string icon { get; set; }
        public List<Fight> fights { get; set; }
    }

    public class Enemy
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public List<Fight> fights { get; set; }
    }

    public class FriendlyPet
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public int petOwner { get; set; }
        public List<Fight> fights { get; set; }
    }

    public class EnemyPet
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public int petOwner { get; set; }
        public List<Fight> fights { get; set; }
    }

    public class Phase
    {
        public int boss { get; set; }
        public List<string> phases { get; set; }
    }

    public class ExportedCharacter
    {
        public int id { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public string region { get; set; }
    }

    public class LogInfo
    {
        public List<Fight> fights { get; set; }
        public string lang { get; set; }
        public List<Friendly> friendlies { get; set; }
        public List<Enemy> enemies { get; set; }
        public List<FriendlyPet> friendlyPets { get; set; }
        public List<EnemyPet> enemyPets { get; set; }
        public List<Phase> phases { get; set; }
        public int logVersion { get; set; }
        public int gameVersion { get; set; }
        public string title { get; set; }
        public string owner { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public int zone { get; set; }
        public List<ExportedCharacter> exportedCharacters { get; set; }
    }


}

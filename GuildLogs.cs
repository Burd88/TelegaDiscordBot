using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static DiscordBot.Program;
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
        }
    }

    class GuildLogs
    {
        private static Log _log;

        public static Log GetLogsInfo()
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

        public static Log GetGuildLogChange()
        {
            try
            {
                _log = new();

                Task<BotSettings> set = Functions.ReadJson<BotSettings>("BotSettings");
                settings = set.Result;

                Update_warcraftlogs_data();

                if (settings.LastGuildLogTime != 0)
                {
                    if (!_log.Error)
                    {
                        if (_log.StartTime > settings.LastGuildLogTime)
                        {

                            Console.WriteLine($"LastGuildLogTime write : {_log.StartTime}\n");
                            settings.LastGuildLogTime = _log.StartTime;
                            Functions.WriteJSon(settings, "BotSettings");
                            return _log;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"LastGuildLogTime write : {_log.StartTime}\n");
                    settings.LastGuildLogTime = _log.StartTime;
                    Functions.WriteJSon(settings, "BotSettings");
                    return null;
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
        private static void Update_warcraftlogs_data()
        {

            try
            {

                WebRequest request = WebRequest.Create($"https://www.warcraftlogs.com/v1/reports/guild/{Program.settings.Guild.ToLower()}/{Functions.GetRealmSlug(Program.settings.Realm)}/eu?api_key=c2c9093c70e642ac6ec003d4b0904c33");
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new(stream))
                    {
                        string line = reader.ReadToEnd();

                        line = "{ \"logs\": " + line + "}";

                        Warcraftlogs logs = JsonConvert.DeserializeObject<Warcraftlogs>(line);
                        if (logs.logs[0].zone != -1)
                        {
                            GetLogInfo(logs.logs[0].id, logs.logs[0].title, Functions.FromUnixTimeStampToDateTime(logs.logs[0].start).ToString(), $"https://ru.warcraftlogs.com/reports/{logs.logs[0].id}");
                        }
                        else
                        {
                            _log.Error = true;
                            return;
                        }

                    }
                }
                responce.Close();
                _log.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _log.Error = true;
                    string message = $"\nUpdate_warcraftlogs_data Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _log.Error = true;
                string message = $"Update_warcraftlogs_data Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }

        private static void GetLogInfo(string id, string dungeon, string date, string link)
        {

            try
            {

                WebRequest request = WebRequest.Create("https://www.warcraftlogs.com/v1/report/fights/" + id + "?translate=false&api_key=c2c9093c70e642ac6ec003d4b0904c33");
                WebResponse responce = request.GetResponse();

                using (Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = reader.ReadToEnd();

                        LogInfo log = JsonConvert.DeserializeObject<LogInfo>(line);

                        int kills = 0;
                        int wipe = 0;
                        foreach (Fight fight in log.fights)
                        {
                            if (fight.kill == true)
                            {
                                kills++;
                            }
                            else
                            {
                                wipe++;
                            }
                        }
                        TimeSpan ts = Functions.FromUnixTimeStampToDateTime(log.end.ToString()) - Functions.FromUnixTimeStampToDateTime(log.start.ToString());
                        _log.RaidTime = $"{ts.Hours} ч {ts.Minutes} м";
                        _log.Date = date;
                        _log.Dungeon = dungeon;
                        _log.KillBoss = kills.ToString();
                        _log.WipeBoss = wipe.ToString();
                        _log.Link = link;
                        _log.StartTime = log.start;
                        _log.ID = id;

                    }
                }
                responce.Close();
                _log.Error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    _log.Error = true;
                    string message = $"\nGetLogInfo Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                _log.Error = true;
                string message = $"GetLogInfo Error: {e.Message}";
                Functions.WriteLogs(message, "error");
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
    public class Warcraftlogs
    {
        public List<Logs_all> logs { get; set; }

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
        public int size { get; set; }
        public int difficulty { get; set; }
        public bool kill { get; set; }
        public int partial { get; set; }
        public int bossPercentage { get; set; }
        public int fightPercentage { get; set; }
        public int lastPhaseForPercentageDisplay { get; set; }
        public List<int> maps { get; set; }
        public int instances { get; set; }
        public int groups { get; set; }
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
        public List<object> enemyPets { get; set; }
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

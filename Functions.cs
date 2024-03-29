﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using static DiscordBot.Program;

namespace DiscordBot
{
    partial class Functions
    {

        public static DateTime FromUnixTimeStampToDateTime(string unixTimeStamp) // конверстация времени
        {

            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp) / 1000).LocalDateTime;
        }

        public static DateTime FromUnixTimeStampToDateTimeUTC(string unixTimeStamp) // конверстация времени
        {

            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp) / 1000).UtcDateTime;
        }
        public static string Relative_time(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            if (ts.TotalMinutes < 1)//seconds ago
                return "сейчас";
            if (ts.TotalHours < 1)//min ago
                if ((int)ts.TotalMinutes == 1 || (int)ts.TotalMinutes == 21 || (int)ts.TotalMinutes == 31 || (int)ts.TotalMinutes == 41 || (int)ts.TotalMinutes == 51)
                {
                    return (int)ts.TotalMinutes + " Минута назад";
                }
                else if ((int)ts.TotalMinutes == 2 || (int)ts.TotalMinutes == 3 || (int)ts.TotalMinutes == 4 || (int)ts.TotalMinutes == 22 || (int)ts.TotalMinutes == 23 ||
                  (int)ts.TotalMinutes == 24 || (int)ts.TotalMinutes == 32 || (int)ts.TotalMinutes == 33 || (int)ts.TotalMinutes == 34 || (int)ts.TotalMinutes == 42 ||
                  (int)ts.TotalMinutes == 43 || (int)ts.TotalMinutes == 44 || (int)ts.TotalMinutes == 52 || (int)ts.TotalMinutes == 53 || (int)ts.TotalMinutes == 54)
                {
                    return (int)ts.TotalMinutes + " Минуты назад";
                }
                else
                {
                    return (int)ts.TotalMinutes + " Минут назад";
                }
            //  return (int)ts.TotalMinutes == 1 ? "1 Minute ago" : (int)ts.TotalMinutes + " Minutes ago";
            if (ts.TotalDays < 1)//hours ago
                if ((int)ts.TotalHours == 1 || (int)ts.TotalHours == 21)
                {
                    return (int)ts.TotalHours + " Час назад";
                }
                else if ((int)ts.TotalHours == 2 || (int)ts.TotalHours == 3 || (int)ts.TotalHours == 4 || (int)ts.TotalHours == 22 || (int)ts.TotalHours == 23 ||
                  (int)ts.TotalHours == 24)
                {
                    return (int)ts.TotalHours + " Часа назад";
                }
                else
                {
                    return (int)ts.TotalHours + " Часов назад";
                }
            // return (int)ts.TotalHours == 1 ? "1 Hour ago" : (int)ts.TotalHours + " Hours ago";
            if (ts.TotalDays < 30.4368)//7)//days ago
                if ((int)ts.TotalDays == 1)
                {
                    return (int)ts.TotalDays + " День назад";
                }
                else if ((int)ts.TotalDays == 2 || (int)ts.TotalDays == 3 || (int)ts.TotalDays == 4)
                {
                    return (int)ts.TotalDays + " Дня назад";
                }
                else
                {
                    return (int)ts.TotalDays + " Дней назад";
                }
            //  return (int)ts.TotalDays == 1 ? "1 Day ago" : (int)ts.TotalDays + " Days ago";
            if (ts.TotalDays < 30.4368)//weeks ago
                return (int)(ts.TotalDays / 7) == 1 ? "1 Неделя назад" : (int)(ts.TotalDays / 7) + " Недели назад";
            // return (int)(ts.TotalDays / 7) == 1 ? "1 Week ago" : (int)(ts.TotalDays / 7) + " Weeks ago";
            if (ts.TotalDays < 365.242)//months ago
                if ((int)(ts.TotalDays / 30.4368) == 1)
                {
                    return (int)ts.TotalHours + " Месяц назад";
                }
                else if ((int)(ts.TotalDays / 30.4368) == 2 || (int)(ts.TotalDays / 30.4368) == 3 || (int)(ts.TotalDays / 30.4368) == 4)
                {
                    return (int)(ts.TotalDays / 30.4368) + " Месяца назад";
                }
                else
                {
                    return (int)(ts.TotalDays / 30.4368) + " Месяцев назад";
                }
            //   return (int)(ts.TotalDays / 30.4368) == 1 ? "1 Month ago" : (int)(ts.TotalDays / 30.4368) + " Months ago";
            //years ago
            return (int)(ts.TotalDays / 365.242) == 1 ? "1 Год назад" : "Больше года назад";
            //return (int)(ts.TotalDays / 365.242) == 1 ? "1 Year ago" : (int)(ts.TotalDays / 365.242) + " Years ago";
        }
        public static TimeSpan GetRelativeDateTime(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            return ts;

        }

        public static List<RealmList> Realms;
        public static void LoadRealmAll()
        {
            Realms = new List<RealmList>();
            Allrealm_ realms = GetWebJson<Allrealm_>("https://eu.api.blizzard.com/data/wow/search/realm?namespace=dynamic-eu&_page=1&_pageSize=1000&locale={settings.Locale}&access_token=" + Program.tokenWow);
            if (realms != null)
            {
                foreach (Result realm in realms.results)
                {
                    Dictionary<string, string> name = new()
                    {
                        { "it_IT", realm.data.name.it_IT },
                        { "ru_RU", realm.data.name.ru_RU },
                        { "en_GB", realm.data.name.en_GB },
                        { "zh_TW", realm.data.name.zh_TW },
                        { "ko_KR", realm.data.name.ko_KR },
                        { "en_US", realm.data.name.en_US },
                        { "es_MX", realm.data.name.es_MX },
                        { "pt_BR", realm.data.name.pt_BR },
                        { "es_ES", realm.data.name.es_ES },
                        { "zh_CN", realm.data.name.zh_CN },
                        { "fr_FR", realm.data.name.fr_FR },
                        { "de_DE", realm.data.name.fr_FR }
                    };

                    Realms.Add(new RealmList { Name = name[settings.Locale], Slug = realm.data.slug });

                }

                Realms.Sort((a, b) => a.Name.CompareTo(b.Name));

                WriteJSon(Realms, "RealmList");

            }

        }
        public static string GetRealmSlug(string text)
        {
            try
            {
                foreach (RealmList rlm in Realms)
                {
                    if (rlm.Name.ToLower() == text.Trim().ToLower().Replace("'", " "))
                    {
                        //Console.WriteLine("slug :" + rlm.Slug);
                        return rlm.Slug;



                    }
                }
            }
            catch (Exception e)
            {

                string message = $"{e.GetType().Name} Error: {e.Message}";
                WriteLogs(message, "error");
                return null;
            }
            return null;



        }
        [GeneratedRegex("\\P{IsBasicLatin}")]
        private static partial Regex RegexLatin();
        [GeneratedRegex("\\P{IsCyrillic}")]
        private static partial Regex RegexCyrillic();
        public static EncounterLang GetEncounter(string text)
        {
            try
            {
                List<EncounterLang> enconterAll = ReadJson<List<EncounterLang>>("EncounterList");
                if (enconterAll != null)
                {
                    foreach (EncounterLang enc in enconterAll)
                    {
                        if (!RegexLatin().IsMatch(text))
                        {
                            if (enc.EncounterEN != null && enc.EncounterEN.ToLower() == text.Trim().ToLower())
                            {

                                return enc;



                            }
                        }
                        else if (!RegexCyrillic().IsMatch(text))
                        {
                            if (enc.EncounterRU != null && enc.EncounterRU.ToLower() == text.Trim().ToLower())
                            {
                                return enc;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {

                string message = $"{e.GetType().Name} Error: {e.Message}";
                WriteLogs(message, "error");
                return null;
            }
            return null;



        }
        public static InstanceLang GetInstance(string text)
        {
            try
            {
                List<InstanceLang> instanceAll = ReadJson<List<InstanceLang>>("InstanceList");
                foreach (InstanceLang inst in instanceAll)
                {
                    if (!RegexLatin().IsMatch(text))
                    {
                        if (inst.InstanceEN != null && inst.InstanceEN.ToLower() == text.Trim().ToLower())
                        {

                            return inst;
                        }
                    }
                    else if (!RegexCyrillic().IsMatch(text))
                    {
                        if (inst.InstanceRU != null && inst.InstanceRU.ToLower() == text.Trim().ToLower())
                        {
                            return inst;
                        }
                    }
                }
            }
            catch (Exception e)
            {

                string message = $"{e.GetType().Name} Error: {e.Message}";
                WriteLogs(message, "error");
                return null;
            }
            return null;



        }

        public static void WriteLogs(string message, string type)
        {
            if (type == "error")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("----------------------------");
                Console.WriteLine($"{DateTime.Now} :: {type}");
                Console.WriteLine($"{message}");
                Console.WriteLine("----------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (type == "notification")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("----------------------------");
                Console.WriteLine($"{DateTime.Now} :: {type}");
                Console.WriteLine($"{message}");
                Console.WriteLine("----------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (type == "command")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("----------------------------");
                Console.WriteLine($"{DateTime.Now} :: {type}");
                Console.WriteLine($"{message}");
                Console.WriteLine("----------------------------");
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }

        public static void WriteJSon<T>(T data, string filename)
        {
            string writePathJSON = @".\json\" + filename + ".json";

            try
            {

                using StreamWriter file = File.CreateText(writePathJSON);
                Newtonsoft.Json.JsonSerializer serializer = new()
                {
                    Formatting = Formatting.Indented
                };

                serializer.Serialize(file, data);


                /*using (FileStream fs = new(writePathJSON, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {



                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                        WriteIndented = true
                    };

                    await System.Text.Json.JsonSerializer.SerializeAsync(fs, data, options);
                    //   WriteLogs($"Запись {filename} прошла успешно", "notification");

                    fs.Close();

                }
               */


            }

            catch (Exception e)
            {

                string message = $"File : {filename}\n{e.GetType().Name}\nError: {e.Message}";
                WriteLogs(message, "error");
            }
        }
        public static T ReadJson<T>(string filename)
        {
            string PathJSON = @".\json\" + filename + ".json";

            try
            {

                using FileStream fs = new(PathJSON, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
                using StreamReader reader = new(fs);
                string line = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(line);





            }

            catch (Exception e)
            {

                string message = $"File : {filename}\n{e.GetType().Name}\nError: {e.Message}";
                WriteLogs(message, "error");
                return default;
            }
        }
        public static T GetWebJson<T>(string link)
        {
            try
            {


                WebRequest request = WebRequest.Create(link);
                WebResponse responce = request.GetResponse();

                using Stream stream = responce.GetResponseStream();
                using StreamReader reader = new(stream);

                string line = reader.ReadToEnd();


                return JsonConvert.DeserializeObject<T>(line);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    return default;

                }
            }
            catch (Exception)
            {
                return default;

            }
            return default;
        }

        public static string GetBNetMedia(string link)
        {

            GetBNetMEdia activity = GetWebJson<GetBNetMEdia>($"{link}&locale={settings.Locale}&access_token={tokenWow}");
            if (activity != null)
            {
                foreach (Asset asset in activity.assets)
                {
                    return asset.value;

                }
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}

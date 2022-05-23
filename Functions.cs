using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Functions
    {

        public static DateTime FromUnixTimeStampToDateTime(string unixTimeStamp) // конверстация времени
        {

            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp) / 1000).LocalDateTime;
        }

        public static DateTime FromUnixTimeStampToDateTimeUTC(string unixTimeStamp) // конверстация времени
        {

            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(unixTimeStamp) / 1000).UtcDateTime;
        }
        public static string relative_time(DateTime date)
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
        public static TimeSpan getRelativeDateTime(DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            return ts;

        }

        public static List<RealmList> Realms;
        public static void LoadRealmAll()
        {

            try
            {
                Realms = new List<RealmList>();

                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/data/wow/search/realm?namespace=dynamic-eu&_page=1&_pageSize=1000&locale=ru_RU&access_token=" + Program.tokenWow);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {

                        string line = "";

                        while ((line = reader1.ReadLine()) != null)
                        {

                            line = line.Replace("'", " ");


                            Allrealm_ realms = JsonConvert.DeserializeObject<Allrealm_>(line);

                            foreach (Allrealm_Result realm in realms.results)
                            {

                                Dictionary<string, string> name = new Dictionary<string, string>();
                                name.Add("it_IT", realm.data.name.it_IT);
                                name.Add("ru_RU", realm.data.name.ru_RU);
                                name.Add("en_GB", realm.data.name.en_GB);
                                name.Add("zh_TW", realm.data.name.zh_TW);
                                name.Add("ko_KR", realm.data.name.ko_KR);
                                name.Add("en_US", realm.data.name.en_US);
                                name.Add("es_MX", realm.data.name.es_MX);
                                name.Add("pt_BR", realm.data.name.pt_BR);
                                name.Add("es_ES", realm.data.name.es_ES);
                                name.Add("zh_CN", realm.data.name.zh_CN);
                                name.Add("fr_FR", realm.data.name.fr_FR);
                                name.Add("de_DE", realm.data.name.fr_FR);

                                Realms.Add(new RealmList { Name = name["ru_RU"], Slug = realm.data.slug });


                                //


                            }

                        }
                    }
                }
                Realms.Sort((a, b) => a.Name.CompareTo(b.Name));
                // WriteRealmInFile();
                WriteJSon(Realms, "RealmList");
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + ": LoadRealmAll Error: " + e.Message);
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

                string message = $"{e.TargetSite} Error: {e.Message}";
                WriteLogs(message, "error");
                return null;
            }
            return null;



        }
        public static string GetEncounterRu(string text)
        {
            try
            {
                List<EncounterLang> enconterAll = ReadJson<List<EncounterLang>>("EncounterList").Result;
                    foreach (EncounterLang enc in enconterAll)
                {
                    if (enc.EncounterEN.ToLower() == text.Trim().ToLower())
                    {
                        //Console.WriteLine("slug :" + rlm.Slug);
                        return enc.EncounterRU;



                    }
                }
            }
            catch (Exception e)
            {

                string message = $"{e.TargetSite} Error: {e.Message}";
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

        public static async void WriteJSon<T>(T data, string filename)
        {
            string writePathJSON = @".\json\" + filename + ".json";

            try
            {



                using (FileStream fs = new(writePathJSON, FileMode.Create, FileAccess.Write, FileShare.Write))
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



            }

            catch (Exception e)
            {

                string message = $"{e.TargetSite} Error: {e.Message}";
                WriteLogs(message, "error");
            }
        }
        public static async Task<T> ReadJson<T>(string filename)
        {
            string PathJSON = @".\json\" + filename + ".json";

            try
            {

                using (FileStream fs = new(PathJSON, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(fs);



                }





            }

            catch (Exception e)
            {

                string message = $"{e.TargetSite} Error: {e.Message}";
                WriteLogs(message, "error");
                return default;
            }
        }

        public static T GetWebJson<T>(string link, string nameMethod)
        {

            try
            {

                WebRequest requesta = WebRequest.Create(link);
                WebResponse responcea = requesta.GetResponse();

                using (Stream stream = responcea.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {



                            return JsonConvert.DeserializeObject<T>(line);





                        }
                    }
                }
                responcea.Close();

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {

                    string message = $"\n{nameMethod} Error: {e.Message}";
                    WriteLogs(message, "error");
                    return default(T);
                }
            }
            catch (Exception e)
            {

                string message = $"{nameMethod} Error: {e.Message}";
                WriteLogs(message, "error");
                return default(T);
            }
            return default(T);
        }

        #region OLD Method
        /* 
           private static async void WriteRealmInFile()
          {
              string writePathJSON = @".\DiscordBot\RealmList.json";






              try
              {



                  using (FileStream fs = new(writePathJSON, FileMode.Create))
                  {

                      //  var result = users.members.SingleOrDefault(a => a.Id == id);

                      //  if (result == null)
                      //  {
                      //     users.members.Add(new User() { Name = name, Id = id });
                      var options = new JsonSerializerOptions
                      {
                          Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                          WriteIndented = true
                      };
                      await System.Text.Json.JsonSerializer.SerializeAsync(fs, Realms, options);



                      // }
                      //  else
                      //  {
                      //      Console.WriteLine("Есть такой уже");
                      //   }




                  }



              }
              catch (Exception e)
              {
                  Console.WriteLine(DateTime.Now + ": WriteRealmInFile Error: " + e.Message);
              }
          }

         private static async void WriteGuildActivityInFile(List<Activity> data)
          {
              string writePathJSON = @".\DiscordBot\BeforeGuildActivity.json";

              try
              {



                  using (FileStream fs = new(writePathJSON, FileMode.Create, FileAccess.Write, FileShare.Write))
                  {



                      var options = new JsonSerializerOptions
                      {
                          Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                          WriteIndented = true
                      };

                      await System.Text.Json.JsonSerializer.SerializeAsync(fs, data, options);


                      fs.Close();

                  }



              }

              catch (Exception e)
              {

                  string message = $"{e.TargetSite} Error: {e.Message}";
                  Functions.WriteLogs(message, "error");
              }
          }
          private static async void ReadGuildActivityInFile()
          {
              string PathJSON = @".\DiscordBot\BeforeGuildActivity.json";

              try
              {

                  using (FileStream fs = new(PathJSON, FileMode.Open, FileAccess.Read, FileShare.Read))
                  {

                      //beforeActivity = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Activity>>(fs);



                  }





              }

              catch (Exception e)
              {

                  string message = $"{e.TargetSite} Error: {e.Message}";
                  Functions.WriteLogs(message, "error");
              }
          }

          private static async void WriteAdditionalRTInFile(string text)
          {
              string writePathtext = @".\DiscordBot\AdditionalRT.txt";





              try
              {
                  using (StreamWriter sw = new StreamWriter(writePathtext, false, System.Text.Encoding.Default))
                  {
                      await sw.WriteAsync(text);
                  }



              }

              catch (Exception e)
              {

                  string message = $"{e.TargetSite} Error: {e.Message}";
                  Functions.WriteLogs(message, "error");
              }
          }
          static async void ReadAdditionalRTText()
          {
              string writePathtext = @".\DiscordBot\AdditionalRT.txt";

              try
              {

                  using (StreamReader sr = new StreamReader(writePathtext))
                  {
                      string addRT = await sr.ReadToEndAsync();
                      additionalRT = Convert.ToBoolean(addRT);
                      Console.WriteLine($"AdditionalRT= {additionalRT} (Доп РТ: false - не будет, true - будет)");
                  }
              }

              catch (Exception e)
              {

                  string message = $"{e.TargetSite} Error: {e.Message}";
                  Functions.WriteLogs(message, "error");
              }
          }
          private static async void WriteNeedPoolRTInFile(string text)
          {
              string writePathtext = @".\DiscordBot\NeedPoolRT.txt";





              try
              {
                  using (StreamWriter sw = new StreamWriter(writePathtext, false, System.Text.Encoding.Default))
                  {
                      await sw.WriteAsync(text);
                  }



              }

              catch (Exception e)
              {

                  string message = $"{e.TargetSite} Error: {e.Message}";
                  Functions.WriteLogs(message, "error");
              }
          }
          static async void ReadNeedPoolRTText()
          {
              string writePathtext = @".\DiscordBot\NeedPoolRT.txt";

              try
              {

                  using (StreamReader sr = new StreamReader(writePathtext))
                  {
                      string addRT = await sr.ReadToEndAsync();
                      needPoolRT = Convert.ToBoolean(addRT);
                      Console.WriteLine($"NeedPoolRT= {needPoolRT} (Ближайшее РТ: false - опроса не будет, true - опрос будет)");
                  }
              }

              catch (Exception e)
              {

                  string message = $"{e.TargetSite} Error: {e.Message}";
                  Functions.WriteLogs(message, "error");
              }
          }
        */
        #endregion
    }

    #region Classes
    public class Root_RealmSelf
    {
        public string href { get; set; }
    }

    public class Root_RealmLinks
    {
        public Root_RealmSelf self { get; set; }
    }

    public class Root_RealmKey
    {
        public string href { get; set; }
    }

    public class Root_RealmRealm
    {
        public Root_RealmKey key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string slug { get; set; }
    }

    public class Root_Realm
    {
        public Root_RealmLinks _links { get; set; }
        public List<Root_RealmRealm> realms { get; set; }
    }

    public class RealmList
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }

    public class RootRealmlocalSelf
    {
        public string href { get; set; }
    }

    public class RootRealmlocalLinks
    {
        public RootRealmlocalSelf self { get; set; }
    }

    public class RootRealmlocalKey
    {
        public string href { get; set; }
    }

    public class RootRealmlocalRegion
    {
        public RootRealmlocalKey key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }

    public class RootRealmlocalConnectedRealm
    {
        public string href { get; set; }
    }

    public class RootRealmlocalType
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class RootRealmlocal
    {
        public RootRealmlocalLinks _links { get; set; }
        public int id { get; set; }
        public RootRealmlocalRegion region { get; set; }
        public RootRealmlocalConnectedRealm connected_realm { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
        public RootRealmlocalType type { get; set; }
        public bool is_tournament { get; set; }
        public string slug { get; set; }
    }

    public class Allrealm_Key
    {
        public string href { get; set; }
    }

    public class Allrealm_Name
    {
        public string it_IT { get; set; }
        public string ru_RU { get; set; }
        public string en_GB { get; set; }
        public string zh_TW { get; set; }
        public string ko_KR { get; set; }
        public string en_US { get; set; }
        public string es_MX { get; set; }
        public string pt_BR { get; set; }
        public string es_ES { get; set; }
        public string zh_CN { get; set; }
        public string fr_FR { get; set; }
        public string de_DE { get; set; }
    }

    public class Allrealm_Region
    {
        public Allrealm_Name name { get; set; }
        public int id { get; set; }
    }

    public class Allrealm_Category
    {
        public string it_IT { get; set; }
        public string ru_RU { get; set; }
        public string en_GB { get; set; }
        public string zh_TW { get; set; }
        public string ko_KR { get; set; }
        public string en_US { get; set; }
        public string es_MX { get; set; }
        public string pt_BR { get; set; }
        public string es_ES { get; set; }
        public string zh_CN { get; set; }
        public string fr_FR { get; set; }
        public string de_DE { get; set; }
    }

    public class Allrealm_Type
    {
        public Allrealm_Name name { get; set; }
        public string type { get; set; }
    }

    public class Allrealm_Data
    {
        public bool is_tournament { get; set; }
        public string timezone { get; set; }
        public Allrealm_Name name { get; set; }
        public int id { get; set; }
        public Allrealm_Region region { get; set; }
        public Allrealm_Category category { get; set; }
        public string locale { get; set; }
        public Allrealm_Type type { get; set; }
        public string slug { get; set; }
    }

    public class Allrealm_Result
    {
        public Allrealm_Key key { get; set; }
        public Allrealm_Data data { get; set; }
    }

    public class Allrealm_
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int maxPageSize { get; set; }
        public int pageCount { get; set; }
        public List<Allrealm_Result> results { get; set; }
    }
    #endregion


}

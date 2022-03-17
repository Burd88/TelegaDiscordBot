using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace DiscordBot
{
    class MythicPlusAffix
    {
        public static bool error = false;
        public static MythicPlusAffixCurrent GetMythicPlusAffixCurrent()
        {

            try
            {
                WebRequest request = WebRequest.Create("https://raider.io/api/v1/mythic-plus/affixes?region=us&locale=ru");
                WebResponse responce = request.GetResponse();

                using (System.IO.Stream stream = responce.GetResponseStream())

                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            MythicPlusAffixCurrent affixs = JsonConvert.DeserializeObject<MythicPlusAffixCurrent>(line);
                            string text = $"**Миффик+ аффиксы на эту неделю:**";
                            int ind = 1;
                            foreach (AffixDetail affix in affixs.affix_details)
                            {
                                if (ind == 1)
                                {
                                    text = text + $"\n      **(+2) {affix.name}**\n{affix.description}";
                                    ind++;
                                }
                                else if (ind == 2)
                                {
                                    text = text + $"\n      **(+4) {affix.name}**\n{affix.description}";
                                    ind++;
                                }
                                else if (ind == 3)
                                {
                                    text = text + $"\n      **(+7) {affix.name}**\n{affix.description}";
                                    ind++;
                                }
                                else if (ind == 4)
                                {
                                    text = text + $"\n      **(+10) {affix.name}**\n{affix.description}";
                                    ind = 1;
                                }

                            }

                            return affixs;
                        }
                    }
                }
                responce.Close();
                error = false;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    error = true;
                    string message = $"\nGetMythicPlusAffixCurrent Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                    return null;
                }
            }
            catch (Exception e)
            {
                error = true;
                string message = $"GetMythicPlusAffixCurrent Error: {e.Message}";
                Functions.WriteLogs(message, "error");
                return null;
            }

            return null;
        }

    }

    public class AffixDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string wowhead_url { get; set; }
    }

    public class MythicPlusAffixCurrent
    {
        public string region { get; set; }
        public string title { get; set; }
        public string leaderboard_url { get; set; }
        public List<AffixDetail> affix_details { get; set; }
    }


}

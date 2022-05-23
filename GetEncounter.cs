using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{

    class GetEncounter
    {
        public static List<EncounterLang> encounterAll;


        public static void LoadEncounterAll()
        {

            try
            {
                encounterAll = new();

                WebRequest requestchar = WebRequest.Create("https://eu.api.blizzard.com/data/wow/journal-encounter/index?namespace=static-eu&locale=en_US&access_token=" + Program.tokenWow);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {

                        string line = "";

                        while ((line = reader1.ReadLine()) != null)
                        {

                            line = line.Replace("'", " ");


                            EncounterAll encounters = JsonConvert.DeserializeObject<EncounterAll>(line);

                            foreach (Encounter encount in encounters.encounters)
                            {

                                GetEncounterAll(encount.key.href);


                            }

                        }
                    }
                }
                Functions.WriteJSon(encounterAll, "EncounterList");
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + ": LoadEncounterAll Error: " + e.Message);
            }
        }
        public static void GetEncounterAll(string link)
        {

            try
            {
               

                WebRequest requestchar = WebRequest.Create(link+ "&access_token=" + Program.tokenWow);

                WebResponse responcechar = requestchar.GetResponse();

                using (Stream stream1 = responcechar.GetResponseStream())

                {
                    using (StreamReader reader1 = new StreamReader(stream1))
                    {

                        string line = "";

                        while ((line = reader1.ReadLine()) != null)
                        {

                            line = line.Replace("'", " ");


                            EncounterFullInfo encounter = JsonConvert.DeserializeObject<EncounterFullInfo>(line);

                            encounterAll.Add(new EncounterLang { EncounterEN = encounter.name.en_US, EncounterRU = encounter.name.ru_RU, EncounterID = encounter.id });

                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now + ": GetEncounterAll Error: " + e.Message);
            }
        }
    }
 
    public class EncounterAll
    {
        public Links _links { get; set; }
        public List<Encounter> encounters { get; set; }
    }

    public class EncounterLang
    {
        public string EncounterEN { get; set; }
        public string EncounterRU { get; set; }
        public int EncounterID { get; set; }
    }

    public class BodyText
    {
        public string en_US { get; set; }
        public string es_MX { get; set; }
        public string pt_BR { get; set; }
        public string de_DE { get; set; }
        public string en_GB { get; set; }
        public string es_ES { get; set; }
        public string fr_FR { get; set; }
        public string it_IT { get; set; }
        public string ru_RU { get; set; }
        public string ko_KR { get; set; }
        public string zh_TW { get; set; }
        public string zh_CN { get; set; }
    }

    public class CategoryEn
    {
        public string type { get; set; }
    }

    public class CreatureEn
    {
        public int id { get; set; }
        public Name name { get; set; }
        public CreatureDisplay creature_display { get; set; }
    }

    public class CreatureDisplayEn
    {
        public Key key { get; set; }
        public int id { get; set; }
    }

    public class Description
    {
        public string en_US { get; set; }
        public string es_MX { get; set; }
        public string pt_BR { get; set; }
        public string de_DE { get; set; }
        public string en_GB { get; set; }
        public string es_ES { get; set; }
        public string fr_FR { get; set; }
        public string it_IT { get; set; }
        public string ru_RU { get; set; }
        public string ko_KR { get; set; }
        public string zh_TW { get; set; }
        public string zh_CN { get; set; }
    }

    public class InstanceEn
    {
        public Key key { get; set; }
        public Name name { get; set; }
        public int id { get; set; }
    }

    public class ItemEn
    {
        public int id { get; set; }
        public ItemEn2 item { get; set; }
    }

    public class ItemEn2
    {
        public Key key { get; set; }
        public Name name { get; set; }
        public int id { get; set; }
    }

   
    public class ModeEn
    {
        public string type { get; set; }
        public Name name { get; set; }
    }

    public class Name
    {
        public string en_US { get; set; }
        public string es_MX { get; set; }
        public string pt_BR { get; set; }
        public string de_DE { get; set; }
        public string en_GB { get; set; }
        public string es_ES { get; set; }
        public string fr_FR { get; set; }
        public string it_IT { get; set; }
        public string ru_RU { get; set; }
        public string ko_KR { get; set; }
        public string zh_TW { get; set; }
        public string zh_CN { get; set; }
    }

    public class EncounterFullInfo
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public Name name { get; set; }
        public Description description { get; set; }
        public List<CreatureEn> creatures { get; set; }
        public List<ItemEn> items { get; set; }
        public List<SectionEn> sections { get; set; }
        public InstanceEn instance { get; set; }
        public CategoryEn category { get; set; }
        public List<ModeEn> modes { get; set; }
    }

    public class SectionEn
    {
        public int id { get; set; }
        public Title title { get; set; }
        public BodyText body_text { get; set; }
        public List<SectionEn> sections { get; set; }
        public CreatureDisplayEn creature_display { get; set; }
        public SpellEn spell { get; set; }
    }

    

    public class SpellEn
    {
        public Key key { get; set; }
        public Name name { get; set; }
        public int id { get; set; }
    }

    public class Title
    {
        public string en_US { get; set; }
        public string es_MX { get; set; }
        public string pt_BR { get; set; }
        public string de_DE { get; set; }
        public string en_GB { get; set; }
        public string es_ES { get; set; }
        public string fr_FR { get; set; }
        public string it_IT { get; set; }
        public string ru_RU { get; set; }
        public string ko_KR { get; set; }
        public string zh_TW { get; set; }
        public string zh_CN { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class GetInstance
    {
        public static List<InstanceLang> instanceAll;


        public static void LoadInstanceAll()
        {
            instanceAll = new();
            WoWInstance instances = Functions.GetWebJson<WoWInstance>("https://eu.api.blizzard.com/data/wow/journal-instance/index?namespace=static-eu&locale=en_US&access_token=" + Program.tokenWow);
            if (instances != null)
            {
                foreach (Instance instance in instances.instances)
                {
                    GetInstanceAll(instance.key.href, instance.name);
                }
                Functions.WriteJSon(instanceAll, "InstanceList");
            }
        }
        public static void GetInstanceAll(string link,string nameEn)
        {
            InstanceMainInfo instance = Functions.GetWebJson<InstanceMainInfo>(link + "&locale=ru_RU&access_token=" + Program.tokenWow);
            if (instance != null)
            {
                
                    instanceAll.Add(new InstanceLang
                    {
                        InstanceEN = nameEn,
                        InstanceRU = instance.name,
                        InstanceID = instance.id,
                        InstanceImg = Functions.GetBNetMedia(instance.media.key.href)
                    });
                
              
            }
        }
    }

   
    public class InstanceLang
    {
        public string InstanceEN { get; set; }
        public string InstanceRU { get; set; }
        public int InstanceID { get; set; }
        public string InstanceImg { get; set; }
    }

    public class Instance
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }    

    

    public class WoWInstance
    {
        public Links _links { get; set; }
        public List<Instance> instances { get; set; }
    }

  
    public class CategoryInstance
    {
        public string type { get; set; }
    }

    

    public class ExpansionInstance
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
    }  

    public class Location
    {
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Map
    {
        public string name { get; set; }
        public int id { get; set; }
    }


    public class ModeInstance
    {
        public Mode2 mode { get; set; }
        public int players { get; set; }
        public bool is_tracked { get; set; }
    }

    public class Mode2
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class InstanceMainInfo
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Map map { get; set; }
        public string description { get; set; }
        public List<Encounter> encounters { get; set; }
        public ExpansionInstance expansion { get; set; }
        public Location location { get; set; }
        public List<ModeInstance> modes { get; set; }
        public Media media { get; set; }
        public int minimum_level { get; set; }
        public CategoryInstance category { get; set; }
    }


}

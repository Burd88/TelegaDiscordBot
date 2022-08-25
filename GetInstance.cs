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


        public static async Task LoadInstanceAll()
        {
            instanceAll = new();
            WoWInstance instances = Functions.GetWebJson<WoWInstance>("https://eu.api.blizzard.com/data/wow/journal-instance/index?namespace=static-eu&locale=en_US&access_token=" + Program.tokenWow);
            if (instances != null)
            {
                foreach (Instance instance in instances.instances)
                {
                   await Task.Run(() =>  GetInstanceAll(instance.key.href, instance.name));
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

   
}

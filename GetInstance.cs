using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class GetInstance
    {
        public static List<InstanceLang> instanceAll;


        public static async Task LoadInstanceAll()
        {
            instanceAll = new();
            WoWInstance instances = await Functions.GetWebJson<WoWInstance>("https://eu.api.blizzard.com/data/wow/journal-instance/index?namespace=static-eu&locale=en_US");
            Console.WriteLine(instances);
            if (instances != null)
            {
                Console.WriteLine("Instance loaded, wait...");
                foreach (Instance instance in instances.instances)
                {

                    await Task.Run(() => GetInstanceAll(instance.key.href, instance.name));
                }

                Functions.WriteJSon(instanceAll, "InstanceList");

                Console.WriteLine("Instance loaded, ok");
            }
        }
        public static async Task GetInstanceAll(string link, string nameEn)
        {
            InstanceMainInfo instance = await Functions.GetWebJson<InstanceMainInfo>(link + $"&locale={settings.Locale}&");
            if (instance != null)
            {

                //Console.WriteLine($"{nameEn} , {instance.name} , {instance.id} , {Functions.GetBNetMedia(instance.media.key.href)}");
                instanceAll.Add(new InstanceLang
                {

                    InstanceEN = nameEn,
                    InstanceRU = instance.name,
                    InstanceID = instance.id,
                    InstanceImg = await Functions.GetBNetMedia(instance.media.key.href)
                });


            }
        }
    }


}

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
            WoWInstance instances = Functions.GetWebJson<WoWInstance>("https://eu.api.blizzard.com/data/wow/journal-instance/index?namespace=static-eu&locale=en_US&access_token=" + Program.tokenWow);
            if (instances != null)
            {
                foreach (Instance instance in instances.instances)
                {

                    await Task.Run(() => GetInstanceAll(instance.key.href, instance.name));
                }

                Functions.WriteJSon(instanceAll, "InstanceList");

            }
        }
        public static void GetInstanceAll(string link, string nameEn)
        {
            InstanceMainInfo instance = Functions.GetWebJson<InstanceMainInfo>(link + $"&locale={settings.Locale}&access_token=" + Program.tokenWow);
            if (instance != null)
            {

                //Console.WriteLine($"{nameEn} , {instance.name} , {instance.id} , {Functions.GetBNetMedia(instance.media.key.href)}");
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

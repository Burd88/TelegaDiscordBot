using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Functions;
using static DiscordBot.Program;
namespace DiscordBot
{
    internal class GetMythicSeasonIndex
    {
        public static async Task<int> GetCurrentSeason()
        {
            MythicKeystoneSeasonsIndex mythicKeystoneSeasonsIndex = await GetWebJson<MythicKeystoneSeasonsIndex>("https://eu.api.blizzard.com/data/wow/mythic-keystone/season/index?namespace=dynamic-eu&locale=ru_RU");
            
            if (mythicKeystoneSeasonsIndex != null)
            {
                Console.WriteLine(mythicKeystoneSeasonsIndex.current_season.id);
                return mythicKeystoneSeasonsIndex.current_season.id;
            }
            else
            {
                Console.WriteLine("Get Season Index Error");
            }
            return 0;
        }
    }
}

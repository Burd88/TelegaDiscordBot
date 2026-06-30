using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot
{

    class GetEncounter
    {
        public static List<EncounterLang> encounterAll;


        public static async Task LoadEncounterAll()
        {
            encounterAll = new();
            EncounterAll encounters = await Functions.GetWebJson<EncounterAll>("https://eu.api.blizzard.com/data/wow/journal-encounter/index?namespace=static-eu&locale=en_US");

            if (encounters != null)
            {
                Console.WriteLine("Encounter loaded, wait...");
                foreach (Encounter encount in encounters.encounters)
                {
                    await Task.Run(() => GetEncounterAll(encount.key.href));
                }
                Functions.WriteJSon(encounterAll, "EncounterList");
                Console.WriteLine("Encounter loaded, ok");
            }

        }
        public static async void GetEncounterAll(string link)
        {
            EncounterFullInfo encounter = await Functions.GetWebJson<EncounterFullInfo>(link);
            if (encounter != null)
            {
                if (encounter.creatures != null)
                {
                    encounterAll.Add(new EncounterLang
                    {
                        EncounterEN = encounter.name.en_US,
                        EncounterRU = encounter.name.ru_RU,
                        EncounterID = encounter.id,
                        EncounterImg = await Functions.GetBNetMedia(encounter.creatures[0].creature_display.key.href)
                    });
                }
                else
                {
                    encounterAll.Add(new EncounterLang
                    {
                        EncounterEN = encounter.name.en_US,
                        EncounterRU = encounter.name.ru_RU,
                        EncounterID = encounter.id,
                        EncounterImg = ""
                    });
                }
            }
        }
    }
}

using System.Collections.Generic;

namespace DiscordBot
{

    class GetEncounter
    {
        public static List<EncounterLang> encounterAll;


        public static void LoadEncounterAll()
        {
            encounterAll = new();
            EncounterAll encounters = Functions.GetWebJson<EncounterAll>("https://eu.api.blizzard.com/data/wow/journal-encounter/index?namespace=static-eu&locale=en_US&access_token=" + Program.tokenWow);
            if (encounters != null)
            {
                foreach (Encounter encount in encounters.encounters)
                {
                    GetEncounterAll(encount.key.href);
                }
                Functions.WriteJSon(encounterAll, "EncounterList");
            }
        }
        public static void GetEncounterAll(string link)
        {
            EncounterFullInfo encounter = Functions.GetWebJson<EncounterFullInfo>(link + "&access_token=" + Program.tokenWow);
            if (encounter != null)
            {
                if (encounter.creatures != null)
                {
                    encounterAll.Add(new EncounterLang
                    {
                        EncounterEN = encounter.name.en_US,
                        EncounterRU = encounter.name.ru_RU,
                        EncounterID = encounter.id,
                        EncounterImg = Functions.GetBNetMedia(encounter.creatures[0].creature_display.key.href)
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

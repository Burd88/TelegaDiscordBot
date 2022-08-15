using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot 
{
    public class RaidRankings
    {
        [JsonProperty("castle-nathria")]
        public GuildCastleNathria CastleNathria { get; set; }

        [JsonProperty("sanctum-of-domination")]
        public GuildSanctumOfDomination SanctumOfDomination { get; set; }

        [JsonProperty("sepulcher-of-the-first-ones")]
        public GuildSepulcherOfTheFirstOnes SepulcherOfTheFirstOnes { get; set; }
    }
}

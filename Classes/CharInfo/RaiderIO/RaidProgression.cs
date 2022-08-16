using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot 
{
    public class RaidProgression
    {
        [JsonProperty("castle-nathria")]
        public CastleNathria CastleNathria { get; set; }

        [JsonProperty("sanctum-of-domination")]
        public SanctumOfDomination SanctumOfDomination { get; set; }

        [JsonProperty("sepulcher-of-the-first-ones")]
        public SepulcherOfTheFirstOnes SepulcherOfTheFirstOnes { get; set; }
    }
}

﻿using Newtonsoft.Json;

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

        [JsonProperty("vault-of-the-incarnates")]
        public VaultOfTheIncarnates VaultOfTheIncarnates { get; set; }

        [JsonProperty("aberrus-the-shadowed-crucible")]
        public AberrusTheShadowedCrucible aberrustheshadowedcrucible { get; set; }


    }
}

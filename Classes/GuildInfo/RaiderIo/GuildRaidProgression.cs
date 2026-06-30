using Newtonsoft.Json;

namespace DiscordBot
{
    public class GuildRaidProgression
    {
        [JsonProperty("castle-nathria")]
        public GuildRaidProgressionFull CastleNathria { get; set; }

        [JsonProperty("sanctum-of-domination")]
        public GuildRaidProgressionFull SanctumOfDomination { get; set; }

        [JsonProperty("sepulcher-of-the-first-ones")]
        public GuildRaidProgressionFull SepulcherOfTheFirstOnes { get; set; }

        [JsonProperty("aberrus-the-shadowed-crucible")]
        public GuildRaidProgressionFull aberrustheshadowedcrucible { get; set; }

        [JsonProperty("vault-of-the-incarnates")]
        public GuildRaidProgressionFull vaultoftheincarnates { get; set; }

        [JsonProperty("tier-mn-1")]
        public GuildRaidProgressionFull midNightOneSeason { get; set; }
    }
}

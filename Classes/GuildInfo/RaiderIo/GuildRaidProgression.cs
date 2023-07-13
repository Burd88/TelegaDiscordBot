using Newtonsoft.Json;

namespace DiscordBot
{
    public class GuildRaidProgression
    {
        [JsonProperty("castle-nathria")]
        public GuildCastleNathria CastleNathria { get; set; }

        [JsonProperty("sanctum-of-domination")]
        public GuildSanctumOfDomination SanctumOfDomination { get; set; }

        [JsonProperty("sepulcher-of-the-first-ones")]
        public GuildSepulcherOfTheFirstOnes SepulcherOfTheFirstOnes { get; set; }

        [JsonProperty("aberrus-the-shadowed-crucible")]
        public AberrusTheShadowedCrucible aberrustheshadowedcrucible { get; set; }

        [JsonProperty("vault-of-the-incarnates")]
        public VaultOfTheIncarnates vaultoftheincarnates { get; set; }
    }
}

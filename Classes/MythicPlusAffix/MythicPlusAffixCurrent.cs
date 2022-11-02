using System.Collections.Generic;

namespace DiscordBot
{
    public class MythicPlusAffixCurrent
    {
        public string region { get; set; }
        public string title { get; set; }
        public string leaderboard_url { get; set; }
        public List<AffixDetail> affix_details { get; set; }
    }
}

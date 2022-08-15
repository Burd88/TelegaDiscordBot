using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

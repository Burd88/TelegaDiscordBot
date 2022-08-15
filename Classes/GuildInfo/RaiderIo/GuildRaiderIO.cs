using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class GuildRaiderIO
    {
        public string name { get; set; }
        public string faction { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public RaidRankings raid_rankings { get; set; }
        public GuildRaidProgression raid_progression { get; set; }
    }
}

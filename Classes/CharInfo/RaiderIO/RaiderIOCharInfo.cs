using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class RaiderIOCharInfo
    {
        public string name { get; set; }
        public string race { get; set; }
        public string @class { get; set; }
        public string active_spec_name { get; set; }
        public string active_spec_role { get; set; }
        public string gender { get; set; }
        public string faction { get; set; }
        public string achievement_points { get; set; }
        public string honorable_kills { get; set; }
        public string thumbnail_url { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public string profile_banner { get; set; }
        public MythicPlusScores mythic_plus_scores { get; set; }
        public RaidProgression raid_progression { get; set; }
    }
}

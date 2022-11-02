using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class Job
    {
        public string id { get; set; }
        public List<string> rewardPool { get; set; }
        public string type { get; set; }
        public List<int> enemyLevels { get; set; }
        public List<int> standingStages { get; set; }
        public int minMR { get; set; }
        public DateTime expiry { get; set; }
    }
}

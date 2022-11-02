using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
   public class NightWave
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public int season { get; set; }
        public string tag { get; set; }
        public int phase { get; set; }
        public Params @params { get; set; }
        public List<object> possibleChallenges { get; set; }
        public List<ActiveChallenge> activeChallenges { get; set; }
        public List<string> rewardTypes { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class Invasions
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public string node { get; set; }
        public string nodeKey { get; set; }
        public string desc { get; set; }
        public AttackerReward attackerReward { get; set; }
        public string attackingFaction { get; set; }
        public Attacker attacker { get; set; }
        public DefenderReward defenderReward { get; set; }
        public string defendingFaction { get; set; }
        public Defender defender { get; set; }
        public bool vsInfestation { get; set; }
        public int count { get; set; }
        public int requiredRuns { get; set; }
        public double completion { get; set; }
        public bool completed { get; set; }
        public string eta { get; set; }
        public List<string> rewardTypes { get; set; }
    }
}

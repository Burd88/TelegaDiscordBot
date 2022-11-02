using System.Collections.Generic;

namespace DiscordBot
{
    public class SetInvasions
    {
        public string NameInvasions { get; set; }
        public string AttackerName { get; set; }
        public string DefenderNmae { get; set; }
        public double Defpersent { get; set; }
        public double Attackpersent { get; set; }

        public string ETA { get; set; }
        public string RewardAttacker { get; set; }
        public string RewardDeffenser { get; set; }
    }
    public class SortRewardInvasion
    {
        public string RewardAttack { get; set; }
        public string RewardAttackLink { get; set; }
        public string RewardDef { get; set; }
        public string RewardDefLink { get; set; }
        public List<SetInvasions> listInvasions { get; set; }
    }
}

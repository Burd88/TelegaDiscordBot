using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Log
    {
        public string Dungeon { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        public string KillBoss { get; set; }
        public string WipeBoss { get; set; }
        public string RaidTime { get; set; }
        public bool Error { get; set; }
        public long StartTime { get; set; }
        public string ID { get; set; }
        public List<string> BossKilling { get; set; }
        public double BestWipeTryPer { get; set; }
        public string BestWipeTryName { get; set; }
        public string InstanceImg { get; set; }

        public Log()
        {
            Dungeon = "";
            Date = "";
            Link = "";
            KillBoss = "";
            WipeBoss = "";
            RaidTime = "";
            StartTime = 0;
            Error = false;
            BossKilling = new();
            BestWipeTryPer = 0.0;
            BestWipeTryName = "";
        }
    }
}

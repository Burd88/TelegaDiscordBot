using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Fight
    {
        public int id { get; set; }
        public int boss { get; set; }
        public int start_time { get; set; }
        public int end_time { get; set; }
        public string name { get; set; }
        public int zoneID { get; set; }
        public string zoneName { get; set; }
        public int zoneDifficulty { get; set; }
        public int? size { get; set; }
        public int? difficulty { get; set; }
        public bool? kill { get; set; }
        public int? partial { get; set; }
        public bool? inProgress { get; set; }
        public int? bossPercentage { get; set; }
        public int? fightPercentage { get; set; }
        public int? lastPhaseAsAbsoluteIndex { get; set; }
        public int? lastPhaseForPercentageDisplay { get; set; }
        public List<int> maps { get; set; }
        public int? instances { get; set; }
        public int? groups { get; set; }
    }
}

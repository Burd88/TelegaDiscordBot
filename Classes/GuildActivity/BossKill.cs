using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class BossKill
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Creature> creatures { get; set; }
        public List<Item> items { get; set; }
        public List<Section> sections { get; set; }
        public Instance instance { get; set; }
        public CategoryBossKill category { get; set; }
        public List<ModeBN> modes { get; set; }
    }
}

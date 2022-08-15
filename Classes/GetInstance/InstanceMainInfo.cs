using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class InstanceMainInfo
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Map map { get; set; }
        public string description { get; set; }
        public List<Encounter> encounters { get; set; }
        public ExpansionInstance expansion { get; set; }
        public Location location { get; set; }
        public List<ModeInstance> modes { get; set; }
        public Media media { get; set; }
        public int minimum_level { get; set; }
        public CategoryInstance category { get; set; }
    }
}

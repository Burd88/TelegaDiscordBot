using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class GuildMain
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Faction faction { get; set; }
        public int achievement_points { get; set; }
        public int member_count { get; set; }
        public Realm realm { get; set; }
        public Crest crest { get; set; }
        public Roster roster { get; set; }
        public Achievements achievements { get; set; }
        public long created_timestamp { get; set; }
        public GuildMainActivity activity { get; set; }
    }
}

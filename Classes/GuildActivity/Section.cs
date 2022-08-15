using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Section
    {
        public int id { get; set; }
        public string title { get; set; }
        public Spell spell { get; set; }
        public string body_text { get; set; }
        public List<Section> sections { get; set; }
        public CreatureDisplay creature_display { get; set; }
    }

}

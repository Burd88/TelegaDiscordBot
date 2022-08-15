using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Creature
    {
        public int id { get; set; }
        public string name { get; set; }
        public CreatureDisplay creature_display { get; set; }
    }
}

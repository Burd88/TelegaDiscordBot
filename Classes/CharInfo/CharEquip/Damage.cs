using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Damage
    {
        public int min_value { get; set; }
        public int max_value { get; set; }
        public string display_string { get; set; }
        public DamageClass damage_class { get; set; }
    }
}

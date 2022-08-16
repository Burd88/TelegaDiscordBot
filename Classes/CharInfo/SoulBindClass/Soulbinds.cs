using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Soulbinds
    {
        public Soulbind soulbind { get; set; }
        public List<Traits> traits { get; set; }
        public bool? is_active { get; set; }
    }
}

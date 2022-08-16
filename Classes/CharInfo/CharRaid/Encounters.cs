using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Encounters
    {
        public Encounter encounter { get; set; }
        public int completed_count { get; set; }
        public object last_kill_timestamp { get; set; }
    }
}

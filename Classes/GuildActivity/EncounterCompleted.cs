using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class EncounterCompleted
    {
        public Encounter encounter { get; set; }
        public ActivityMode mode { get; set; }
    }
}

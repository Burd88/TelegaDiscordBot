using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class CovenantProgress
    {
        public ChosenCovenant chosen_covenant { get; set; }
        public int renown_level { get; set; }
        public SoulbindsLink soulbinds { get; set; }
    }
}

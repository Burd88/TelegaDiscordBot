using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Progress
    {
        public int completed_count { get; set; }
        public int total_count { get; set; }
        public List<Encounters> encounters { get; set; }
    }
}

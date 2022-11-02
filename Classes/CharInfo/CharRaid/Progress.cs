using System.Collections.Generic;

namespace DiscordBot
{
    public class Progress
    {
        public int completed_count { get; set; }
        public int total_count { get; set; }
        public List<Encounters> encounters { get; set; }
    }
}

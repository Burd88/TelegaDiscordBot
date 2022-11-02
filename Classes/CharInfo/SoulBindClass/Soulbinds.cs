using System.Collections.Generic;

namespace DiscordBot
{
    public class Soulbinds
    {
        public Soulbind soulbind { get; set; }
        public List<Traits> traits { get; set; }
        public bool? is_active { get; set; }
    }
}

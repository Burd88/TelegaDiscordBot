using System.Collections.Generic;

namespace DiscordBot
{
    public class Set
    {
        public ItemSet item_set { get; set; }
        public List<Items> items { get; set; }
        public List<Effect> effects { get; set; }
        public string display_string { get; set; }
    }
}

using System.Collections.Generic;

namespace DiscordBot
{
    public class EquippedItemSet
    {
        public ItemSet item_set { get; set; }
        public List<ItemLoot> items { get; set; }
        public List<Effect> effects { get; set; }
        public string display_string { get; set; }
    }
}

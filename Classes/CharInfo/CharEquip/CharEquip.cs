using System.Collections.Generic;

namespace DiscordBot
{
    public class CharEquip
    {
        public Links _links { get; set; }
        public Character character { get; set; }
        public List<EquippedItem> equipped_items { get; set; }
        public List<EquippedItemSet> equipped_item_sets { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

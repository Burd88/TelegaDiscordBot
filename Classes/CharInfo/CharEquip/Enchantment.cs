using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Enchantment
    {
        public string display_string { get; set; }
        public SourceItem source_item { get; set; }
        public int enchantment_id { get; set; }
        public EnchantmentSlot enchantment_slot { get; set; }
    }
}

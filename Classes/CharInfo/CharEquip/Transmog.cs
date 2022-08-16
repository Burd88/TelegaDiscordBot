using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Transmog
    {
        public ItemLoot item { get; set; }
        public string display_string { get; set; }
        public int item_modified_appearance_id { get; set; }
        public SecondItem second_item { get; set; }
        public int? second_item_modified_appearance_id { get; set; }
    }

}

using System.Collections.Generic;

namespace DiscordBot
{
    public class AttackerReward
    {
        public List<object> items { get; set; }
        public List<CountedItem> countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }

}

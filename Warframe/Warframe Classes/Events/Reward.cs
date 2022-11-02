using System.Collections.Generic;

namespace DiscordBot
{
    public class Reward
    {
        public List<string> items { get; set; }
        public List<object> countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }
}

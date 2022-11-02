using System.Collections.Generic;

namespace DiscordBot
{
    public class Friendly
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string server { get; set; }
        public string icon { get; set; }
        public List<Fight> fights { get; set; }
    }
}

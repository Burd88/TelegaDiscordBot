using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class VoidTrader
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public DateTime expiry { get; set; }
        public string startString { get; set; }
        public bool active { get; set; }
        public string character { get; set; }
        public string location { get; set; }
        public List<Inventory> inventory { get; set; }
        public string psId { get; set; }
        public string endString { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class ActiveChallenge
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public bool isDaily { get; set; }
        public bool isElite { get; set; }
        public string desc { get; set; }
        public string title { get; set; }
        public int reputation { get; set; }
    }
}

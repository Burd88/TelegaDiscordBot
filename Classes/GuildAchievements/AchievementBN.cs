using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class AchievementBN
    {
        public int id { get; set; }
        public Achievement2 achievement { get; set; }
        public Criteria1 criteria { get; set; }
        public object completed_timestamp { get; set; }
    }
}

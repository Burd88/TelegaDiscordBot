using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class MythicKeystoneSeasonsIndex
    {
        public Links _links { get; set; }
        public List<Season> seasons { get; set; }
        public CurrentSeason current_season { get; set; }
    }
}

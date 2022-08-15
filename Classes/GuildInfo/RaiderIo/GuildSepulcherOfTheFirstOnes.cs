﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class GuildSepulcherOfTheFirstOnes
    {
        public Normal normal { get; set; }
        public Heroic heroic { get; set; }
        public Mythic mythic { get; set; }
        public string summary { get; set; }
        public int total_bosses { get; set; }
        public int normal_bosses_killed { get; set; }
        public int heroic_bosses_killed { get; set; }
        public int mythic_bosses_killed { get; set; }
    }
}
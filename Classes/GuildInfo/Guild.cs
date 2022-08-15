﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Guild
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm realm { get; set; }
        public Faction faction { get; set; }
    }
}

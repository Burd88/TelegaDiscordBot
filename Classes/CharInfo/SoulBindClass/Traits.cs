﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Traits
    {
        public Trait trait { get; set; }
        public int tier { get; set; }
        public int display_order { get; set; }
        public ConduitSocket conduit_socket { get; set; }
    }
}

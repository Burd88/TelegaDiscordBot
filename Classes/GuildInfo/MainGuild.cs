﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class MainGuild
    {
        public Links _links { get; set; }
        public Guild guild { get; set; }
        public List<Member> members { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class CharMedia
    {
        public Links _links { get; set; }
        public Character character { get; set; }
        public List<Asset> assets { get; set; }
    }
}
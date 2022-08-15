using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class SectionLocals
    {
        public int id { get; set; }
        public Title title { get; set; }
        public BodyText body_text { get; set; }
        public List<SectionLocals> sections { get; set; }
        public CreatureDisplay creature_display { get; set; }
        public SpellLocals spell { get; set; }
    }
}

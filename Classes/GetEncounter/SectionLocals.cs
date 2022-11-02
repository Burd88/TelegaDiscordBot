using System.Collections.Generic;

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

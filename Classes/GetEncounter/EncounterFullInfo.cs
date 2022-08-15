using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class EncounterFullInfo
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public Name name { get; set; }
        public Description description { get; set; }
        public List<CreatureNameLocals> creatures { get; set; }
        public List<ItemLocals> items { get; set; }
        public List<SectionLocals> sections { get; set; }
        public InstanceNameLocals instance { get; set; }
        public CategoryLow category { get; set; }
        public List<ModeNameLocals> modes { get; set; }
    }
}

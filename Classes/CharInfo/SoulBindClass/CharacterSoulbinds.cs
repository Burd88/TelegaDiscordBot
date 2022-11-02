using System.Collections.Generic;

namespace DiscordBot
{
    public class CharacterSoulbinds
    {
        public Links _links { get; set; }
        public Character character { get; set; }
        public ChosenCovenant chosen_covenant { get; set; }
        public int renown_level { get; set; }
        public List<Soulbinds> soulbinds { get; set; }
    }
}

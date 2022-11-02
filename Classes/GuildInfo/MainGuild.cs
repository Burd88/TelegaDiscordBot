using System.Collections.Generic;

namespace DiscordBot
{
    public class MainGuild
    {
        public Links _links { get; set; }
        public Guild guild { get; set; }
        public List<Member> members { get; set; }
    }
}

using System.Collections.Generic;

namespace DiscordBot
{
    public class GuildActivitys
    {
        public Links _links { get; set; }
        public Guild guild { get; set; }
        public List<Activitys> activities { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Realms
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public bool has_queue { get; set; }
        public Status status { get; set; }
        public Population population { get; set; }
        public List<ConnectRealm> realms { get; set; }
        public MythicLeaderboards mythic_leaderboards { get; set; }
        public Auctions auctions { get; set; }
    }
}

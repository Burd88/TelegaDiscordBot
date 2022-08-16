using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Socket
    {
        public SocketType socket_type { get; set; }
        public ItemLoot item { get; set; }
        public string display_string { get; set; }
        public Media media { get; set; }
    }
}

using System.Collections.Generic;

namespace DiscordBot
{
    public class EnemyPet
    {
        public string name { get; set; }
        public int id { get; set; }
        public int guid { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public int petOwner { get; set; }
        public List<Fight> fights { get; set; }
    }
}

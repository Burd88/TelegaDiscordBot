using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class LogInfo
    {
        public List<Fight> fights { get; set; }
        public string lang { get; set; }
        public List<Friendly> friendlies { get; set; }
        public List<Enemy> enemies { get; set; }
        public List<FriendlyPet> friendlyPets { get; set; }
        public List<EnemyPet> enemyPets { get; set; }
        public List<Phase> phases { get; set; }
        public int logVersion { get; set; }
        public int gameVersion { get; set; }
        public string title { get; set; }
        public string owner { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public int zone { get; set; }
        public List<ExportedCharacter> exportedCharacters { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class Activitys
    {
        public EncounterCompleted encounter_completed { get; set; }
        public ActivityType activity { get; set; }
        public string timestamp { get; set; }
        public CharacterAchievement character_achievement { get; set; }
    }
}

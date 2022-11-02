using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class Events
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public int? maximumScore { get; set; }
        public int? currentScore { get; set; }
        public object smallInterval { get; set; }
        public object largeInterval { get; set; }
        public string description { get; set; }
        public string tooltip { get; set; }
        public string node { get; set; }
        public List<object> concurrentNodes { get; set; }
        public string scoreLocTag { get; set; }
        public List<Reward> rewards { get; set; }
        public bool expired { get; set; }
        public double health { get; set; }
        public List<InterimStep> interimSteps { get; set; }
        public List<object> progressSteps { get; set; }
        public bool isPersonal { get; set; }
        public bool isCommunity { get; set; }
        public List<object> regionDrops { get; set; }
        public List<object> archwingDrops { get; set; }
        public string asString { get; set; }
        public Metadata metadata { get; set; }
        public List<object> completionBonuses { get; set; }
        public string scoreVar { get; set; }
        public DateTime altExpiry { get; set; }
        public DateTime altActivation { get; set; }
        public NextAlt nextAlt { get; set; }
        public string victimNode { get; set; }
        public string affiliatedWith { get; set; }
        public List<Job> jobs { get; set; }
        public List<PreviousJob> previousJobs { get; set; }
        public string previousId { get; set; }
    }
}

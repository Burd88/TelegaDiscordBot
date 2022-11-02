using System.Collections.Generic;

namespace DiscordBot
{
    public class GuildAchievement
    {
        public Links _links { get; set; }
        public Guild1 guild { get; set; }
        public int total_quantity { get; set; }
        public int total_points { get; set; }
        public List<AchievementBN> achievements { get; set; }
        public List<CategoryProgress> category_progress { get; set; }
        public List<RecentEvent> recent_events { get; set; }
    }
}

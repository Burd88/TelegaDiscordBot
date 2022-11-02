namespace DiscordBot
{
    public class Data
    {
        public bool is_tournament { get; set; }
        public string timezone { get; set; }
        public Name name { get; set; }
        public int id { get; set; }
        public RegionNameLocals region { get; set; }
        public CategoryLocals category { get; set; }
        public string locale { get; set; }
        public TypeNameLocals type { get; set; }
        public string slug { get; set; }
    }
}

namespace DiscordBot
{
    public class ConnectRealm
    {
        public int id { get; set; }
        public Region region { get; set; }
        public ConnectedRealm connected_realm { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string locale { get; set; }
        public string timezone { get; set; }
        public Type type { get; set; }
        public bool is_tournament { get; set; }
        public string slug { get; set; }
    }
}

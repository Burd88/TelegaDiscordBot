namespace DiscordBot
{
    public class CharacterGuild
    {
        public Key key { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Realm realm { get; set; }
        public int level { get; set; }
        public PlayableClass playable_class { get; set; }
        public PlayableRace playable_race { get; set; }
    }
}

namespace DiscordBot
{
    public class Stat
    {
        public Type type { get; set; }
        public int value { get; set; }
        public Display display { get; set; }
        public bool? is_negated { get; set; }
        public bool? is_equip_bonus { get; set; }
    }
}

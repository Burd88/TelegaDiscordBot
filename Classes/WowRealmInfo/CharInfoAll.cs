namespace DiscordBot
{
    public class CharInfoAll
    {
        public string Name { get; set; }
        public string Race { get; set; }
        public string Lvl { get; set; }
        public string Guild { get; set; }
        public string ILvl { get; set; }
        public string Class { get; set; }
        public string Spec { get; set; }
        public string Coven { get; set; }
        public string CovenSoul { get; set; }
        public string RaidProgress { get; set; }
        public string MythicPlus { get; set; }
        public string LastLogin { get; set; }
        public bool Error { get; set; }
        public string ImageCharMain { get; set; }
        public string ImageCharMainRaw { get; set; }
        public string ImageCharAvatar { get; set; }
        public string ImageCharInset { get; set; }
        public string Stats { get; set; }
        public string LinkBnet { get; set; }
        public string SetcountItem { get; set; }

        public CharInfoAll()
        {
            Name = "";
            ILvl = "";
            Class = "нет";
            Spec = "нет";
            Coven = "нет";
            Stats = "";
            CovenSoul = "нет";
            LastLogin = "хз";
            Guild = "нет";
            RaidProgress = "0/0";
            MythicPlus = "0";
            ImageCharMain = "";
            ImageCharAvatar = "";
            ImageCharMainRaw = "";
            ImageCharInset = "";
            Error = false;
            LinkBnet = "";
            SetcountItem = "";
        }

    }
}

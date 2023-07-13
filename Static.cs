using System;
using System.Collections.Generic;

namespace DiscordBot
{


    class Static
    {
        public static string tank = "";
        public static string heal = "";
        public static string mdd = "";
        public static string rdd = "";
        public static string middleIlvl = "";
        public static string description = "";

        public static void UpdateStaticRoster()
        {
            var roster = Functions.ReadJson<StaticRoster>("Static");
            if (roster != null)
            {
                GetStaticRoster(roster);
            }
            else
            {
                Functions.WriteLogs("Roster is null, check json file", "error");
            }

        }

        public static void AddMemberStaticRoster(string name)
        {
            string[] str = name.Split("-");
            var roster = Functions.ReadJson<StaticRoster>("Static");
            //  Console.WriteLine(str[0].ToLower());
            var member = roster.Static.Find(x => x.Name.ToLower() == str[0].ToLower());


            if (member == null)
            {
                // Console.WriteLine(str[0].ToLower());
                roster.Static.Add(new StaticChar { Name = str[0], Role = str[1] });
                GetStaticRoster(roster);
            }

        }
        public static void DeleteMemberStaticRoster(string name)
        {

            var roster = Functions.ReadJson<StaticRoster>("Static");
            var member = roster.Static.Find(x => x.Name.ToLower() == name.ToLower());

            if (member != null)
            {
                //Console.WriteLine(name.ToLower() + "\n" + member.Name);
                roster.Static.RemoveAll(x => x.Name.ToLower() == name.ToLower());
                GetStaticRoster(roster);
            }

        }
        public static void GetStaticRoster(StaticRoster roster)
        {

            tank = "";
            heal = "";
            mdd = "";
            rdd = "";
            int tankid = 0;
            int healid = 0;
            int mddid = 0;
            int rddid = 0;
            int everage = 0;
            description = roster.Description;
            List<StaticChar> newroster = new();
            foreach (StaticChar member in roster.Static)
            {
                CharInfo pers = new();
                var persinfo = pers.GetCharInfo(member.Name);
                if (persinfo != null)
                {
                    if (member.Role == "танк")
                    {
                        tankid++;
                        tank += $"**{tankid})** [{persinfo.Name}](https://worldofwarcraft.com/ru-ru/character/eu/howling-fjord/{persinfo.Name.ToLower()})(**{persinfo.ILvl}**)\n**{persinfo.Class}**-**{persinfo.Spec}**\n__Рейд: **{persinfo.RaidProgress}** Миф+: **{persinfo.MythicPlus}** {persinfo.SetcountItem}__\n";
                    }
                    else if (member.Role == "хил")
                    {
                        healid++;
                        heal += $"**{healid})** [{persinfo.Name}](https://worldofwarcraft.com/ru-ru/character/eu/howling-fjord/{persinfo.Name.ToLower()})(**{persinfo.ILvl}**)\n**{persinfo.Class}**-**{persinfo.Spec}**\n__Рейд: **{persinfo.RaidProgress}** Миф+: **{persinfo.MythicPlus}** {persinfo.SetcountItem}__\n";
                    }
                    else if (member.Role == "мдд")
                    {
                        mddid++;
                        mdd += $"**{mddid})** [{persinfo.Name}](https://worldofwarcraft.com/ru-ru/character/eu/howling-fjord/{persinfo.Name.ToLower()})(**{persinfo.ILvl}**)\n**{persinfo.Class}**-**{persinfo.Spec}**\n__Рейд: **{persinfo.RaidProgress}** Миф+: **{persinfo.MythicPlus}** {persinfo.SetcountItem}__\n";
                    }
                    else if (member.Role == "рдд")
                    {
                        rddid++;
                        rdd += $"**{rddid})** [{persinfo.Name}](https://worldofwarcraft.com/ru-ru/character/eu/howling-fjord/{persinfo.Name.ToLower()})(**{persinfo.ILvl}**)\n**{persinfo.Class}**-**{persinfo.Spec}**\n__Рейд: **{persinfo.RaidProgress}** Миф+: **{persinfo.MythicPlus}** {persinfo.SetcountItem}__\n";
                    }
                    everage += Convert.ToInt32(persinfo.ILvl);
                    newroster.Add(new StaticChar { Name = persinfo.Name.Replace("**", ""), Role = member.Role, Class = persinfo.Class, Ilvl = persinfo.ILvl, Raid = persinfo.RaidProgress, Spec = persinfo.Spec });
                }

            }
            middleIlvl = (everage / newroster.Count).ToString();
            StaticRoster newstaticroster = new()
            {
                Static = newroster,
                Description = description
            };
            Functions.WriteJSon<StaticRoster>(newstaticroster, "Static");
        }


    }




}

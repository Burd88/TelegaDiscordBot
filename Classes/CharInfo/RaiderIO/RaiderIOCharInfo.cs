using DiscordBot;
using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class RaiderIOCharInfo
    {
        public string name { get; set; }
        public string race { get; set; }
        public string @class { get; set; }
        public string active_spec_name { get; set; }
        public string active_spec_role { get; set; }
        public string gender { get; set; }
        public string faction { get; set; }
        public int achievement_points { get; set; }
        public string thumbnail_url { get; set; }
        public string region { get; set; }
        public string realm { get; set; }
        public DateTime last_crawled_at { get; set; }
        public string profile_url { get; set; }
        public string profile_banner { get; set; }
        public List<MythicPlusScoresBySeason> mythic_plus_scores_by_season { get; set; }
        public RaidProgression raid_progression { get; set; }
        public int statusCode { get; set; }
        public String error {  get; set; }
        public String message { get; set; } 
    }


}

public class MythicPlusScoresBySeason
{
    public string season { get; set; }
    public Scores scores { get; set; }
    public Segments segments { get; set; }
}


public class Scores
{
    public double all { get; set; }
    public double dps { get; set; }
    public int healer { get; set; }
    public int tank { get; set; }
    public double spec_0 { get; set; }
    public int spec_1 { get; set; }
    public int spec_2 { get; set; }
    public int spec_3 { get; set; }
}

public class Segments
{
    public All all { get; set; }
    public Dps dps { get; set; }
    public Healer healer { get; set; }
    public Tank tank { get; set; }
    public Spec0 spec_0 { get; set; }
    public Spec1 spec_1 { get; set; }
    public Spec2 spec_2 { get; set; }
    public Spec3 spec_3 { get; set; }
}

public class All
{
    public double score { get; set; }
    public string color { get; set; }
}

public class Healer
{
    public int score { get; set; }
    public string color { get; set; }
}

public class Spec0
{
    public double score { get; set; }
    public string color { get; set; }
}

public class Spec1
{
    public int score { get; set; }
    public string color { get; set; }
}

public class Spec2
{
    public int score { get; set; }
    public string color { get; set; }
}

public class Spec3
{
    public int score { get; set; }
    public string color { get; set; }
}

public class Tank
{
    public int score { get; set; }
    public string color { get; set; }
}


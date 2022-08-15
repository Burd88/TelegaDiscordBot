using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot 
{
    public class AchievChar
    {
        public Links _links { get; set; }
        public int id { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int points { get; set; }
        public bool is_account_wide { get; set; }
        public Criteria criteria { get; set; }
        public string reward_description { get; set; }
        public Media media { get; set; }
        public int display_order { get; set; }
    }
}

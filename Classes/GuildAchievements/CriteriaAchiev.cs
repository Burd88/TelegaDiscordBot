using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class CriteriaAchiev
    {
        public int id { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public Operator @operator { get; set; }
        public List<Child> child_criteria { get; set; }
    }
}

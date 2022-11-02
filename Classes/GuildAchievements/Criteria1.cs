using System.Collections.Generic;

namespace DiscordBot
{
    public class Criteria1
    {
        public int id { get; set; }
        public bool is_completed { get; set; }
        public List<ChildCriteria1> child_criteria { get; set; }
        public int? amount { get; set; }
    }
}

using System.Collections.Generic;

namespace DiscordBot
{
    public class Allrealm_
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int maxPageSize { get; set; }
        public int pageCount { get; set; }
        public List<Result> results { get; set; }
    }
}

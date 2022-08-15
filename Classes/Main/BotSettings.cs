using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class BotSettings
    {
        public string DiscordBotToken { get; set; }
        public string TelegramBotToken { get; set; }
        public string BatNetToken { get; set; }
        public string BatNetSecretKey { get; set; }
        public string Realm { get; set; }
        public string RealmSlug { get; set; }

        public bool NeedPoolRT { get; set; }
        public bool AddtionalRT { get; set; }
        public string RealmStatusType { get; set; }
        public string Guild { get; set; }
        public ulong DiscordMainChatId { get; set; }

        public ulong DiscordMainChannelId { get; set; }
        public ulong DiscordActivityChannelId { get; set; }
        public ulong DiscordRosterChannelId { get; set; }
        public ulong DiscordLogChannelId { get; set; }
        public ulong DiscordAffixChannelId { get; set; }
        public ulong DiscordTestChatId { get; set; }
        public ulong TestDiscordMainChannelId { get; set; }
        public long TelegramMainChatID { get; set; }
        public long TelegramTestChatID { get; set; }
        public long LastGuildAchiveTime { get; set; }
        public long LastGuildActiveTime { get; set; }
        public long LastGuildLogTime { get; set; }
    }
}

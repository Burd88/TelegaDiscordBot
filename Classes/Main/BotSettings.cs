namespace DiscordBot
{
    public class BotSettings
    {
        public string DiscordBotToken { get; set; }
        public string TelegramBotToken { get; set; }
        public string BatNetToken { get; set; }
        public string BatNetSecretKey { get; set; }
        public string RealmName { get; set; }
        public string RealmSlug { get; set; }
        public string RealmStatusType { get; set; }
        public string GuildName { get; set; }
        public string Locale { get; set; }
        public bool EnablePoolRT { get; set; }
        public bool EnableCheckReboot { get; set; }
        public bool EnableCheckAffix { get; set; }
        public bool EnableCheckActivity { get; set; }
        public bool EnableCheckAchievements { get; set; }
        public bool EnableCheckLastLog { get; set; }
        public bool EnableCheckRoster { get; set; }
        public bool EnableCheckDiscordRole { get; set; }
        public bool EnableAddtionalRT { get; set; }

        public ulong DiscordChatId { get; set; }
        public ulong DiscordTestChatId { get; set; }
        public ulong DiscordMainChannelId { get; set; }
        public ulong DiscordActivityChannelId { get; set; }
        public ulong DiscordRosterChannelId { get; set; }
        public ulong DiscordLogChannelId { get; set; }
        public ulong DiscordAffixChannelId { get; set; }
        public ulong DiscordRebootChannelId { get; set; }
        public ulong DiscordPoolRTChannelId { get; set; }
        public ulong DiscordStaticChannelId { get; set; }
        public ulong DiscordLogMessageId { get; set; }
        public ulong DiscordStaticMessageId { get; set; }
        public ulong DiscordTestMainChannelId { get; set; }
        public long TelegramChatID { get; set; }
        public long TelegramTestChatID { get; set; }
        public bool TelegramNotificationEnable { get; set; }
        public long LastGuildAchiveTime { get; set; }
        public long LastGuildActiveTime { get; set; }

        public ulong DiscordWarframeChatId { get; set; }
        public ulong DiscordWarframeEventsChannelId { get; set; }
        public ulong DiscordWarframeEventsMessageId { get; set; }
        public ulong DiscordWarframeInvasionsChannelId { get; set; }
        public ulong DiscordWarframeInvasionsMessageId { get; set; }
        public ulong DiscordWarframeVoidTraderChannelId { get; set; }
        public ulong DiscordWarframeVoidTraderMessageId { get; set; }
        public ulong DiscordWarframeNightWaveChannelId { get; set; }
        public ulong DiscordWarframeNightWaveMessageId { get; set; }

    }
}

using Discord;
using System;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CheckReboot
    {
        public static async void OnTimerHandlerCheckReboot(object obj)
        {
            if (settings.EnableCheckReboot)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                {
                    
                    WowRealmInfo realmcheck = new();
                    
                    string[] text = realmcheck.GetRealmInfoForTimer();
                    
                    if (text != null)
                    {
                        _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                        var chan = _mainChat.GetChannel(settings.DiscordRebootChannelId) as IMessageChannel;
                        var builder = new EmbedBuilder()
                             .WithTitle($"**{text[0]}**")
                             .WithDescription($"**{text[1]}**");

                        await chan.SendMessageAsync(null, false, builder.Build());
                        if (settings.TelegramNotificationEnable)
                        {
                            await telegramClient.SendTextMessageAsync(settings.TelegramChatID, $"{text[0]}\n{text[1]}", parseMode: ParseMode.Html);
                        }
                        string message = ("Отправленно оповещение о Тех.Работах!");
                        Functions.WriteLogs(message, "notification");
                    }

                }
            }


        }
    }
}

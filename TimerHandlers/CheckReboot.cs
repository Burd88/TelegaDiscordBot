using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static DiscordBot.Program;

namespace DiscordBot 
{
    class CheckReboot
    {
        public static async void OnTimerHandlerCheckReboot(object obj)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                WowRealmInfo realmcheck = new();
                string[] text = realmcheck.GetRealmInfoForTimer();
                if (text != null)
                {
                    _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                    var chan = _mainChat.GetChannel(settings.DiscordMainChannelId) as IMessageChannel;
                    var builder = new EmbedBuilder()
                         .WithTitle($"**Информация о техобслуживании!**")
                         .AddField($"{text[0]}", $"**{text[1]}**");

                    await chan.SendMessageAsync(null, false, builder.Build());
                    if (telegramBotNotification)
                    {
                        await telegramClient.SendTextMessageAsync(settings.TelegramMainChatID, $"{text[0]}\n{text[1]}", parseMode: ParseMode.Html);
                    }
                    string message = ("Отправленно оповещение о Тех.Работах!");
                    Functions.WriteLogs(message, "notification");
                }

            }

        }
    }
}

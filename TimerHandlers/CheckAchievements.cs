using Discord;
using System;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CheckAchievements
    {
        public static async void OnTimerHandlerCheckAchievements(object obj)
        {
            if (settings.EnableCheckAchievements)
            {
                try
                {
                    var builder = new EmbedBuilder();
                    GuildAchievements guildAchive = new();
                    var achiev = guildAchive.GetGuildAchievementChange();
                    if (achiev != null && achiev.Count != 0)
                    {
                        foreach (Achievement achieve in achiev)
                        {

                            builder = new EmbedBuilder()
                                .WithTitle($"Гильдия получила достижение!")
                                .WithThumbnailUrl(achieve.Icon).WithColor(Discord.Color.DarkRed)
                                .AddField("Название:", achieve.Name, false)
                                .AddField("Категоря:", achieve.Category, false);
                            var embed = builder.Build();
                            _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                            var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                            await chan.SendMessageAsync(null, false, embed);
                            if (settings.TelegramNotificationEnable)
                            {
                                await telegramClient.SendTextMessageAsync(
                                     chatId: settings.TelegramChatID,

                                    $"<b>Гильдия получила достижение!!</b>\nНазвание: {achieve.Name}\nКатегоря: {achieve.Category}"
                                     , parseMode: ParseMode.Html
                                     );
                            }
                        }
                        string message = ("Отправленно оповещение о получении достижения гильдии!");
                        Functions.WriteLogs(message, "notification");
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        string message = $"\nOnTimerHandlerAchievements Error: {e.Message}";
                        Functions.WriteLogs(message, "error");
                    }
                }
                catch (Exception e)
                {
                    string message = $"OnTimerHandlerAchievements Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }

        }
    }
}

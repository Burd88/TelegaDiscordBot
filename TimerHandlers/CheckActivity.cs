using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CheckActivity
    {
        public static async void OnTimerHandlerCheckActivity(object obj)
        {
            try
            {
                var builder = new EmbedBuilder();
                GuildActivity activity = new();
                var activNew = activity.GetGuildActivityChange();

                if (activNew != null && activNew.Count != 0)
                {
                    foreach (Activity activ in activNew)
                    {
                        if (activ.Type == "CHARACTER_ACHIEVEMENT")
                        {
                            if (activ.Icon == null)
                            {
                                builder = new EmbedBuilder()
                                    .WithTitle($"**{activ.Name}**")
                                    .WithDescription($"получил(а) достижение!")
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Название:", activ.Mode, false);
                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                var embed = builder.Build();
                                await chan.SendMessageAsync(null, false, embed);
                                if (telegramBotNotification)
                                {
                                    if (activ.Categor != "Рейды Legion" && activ.Categor != "Рейды Azeroth" && activ.Categor != "Рейды Draenor" && activ.Categor != "Рейды Pandaria")
                                    {

                                        await telegramClient.SendTextMessageAsync(
                                         chatId: settings.TelegramMainChatID,
                                         text: $"Всё збс, это <b>Достижение</b>!\n<a href =\"https://www.youtube.com/watch?v=d-diB65scQU&ab_channel=BobbyMcFerrinVEVO\">Don't Worry Be Happy</a>\nПолучил(а): <b>{activ.Name}</b>\nНазвание: <b>{activ.Mode}</b>",
                                         parseMode: ParseMode.Html, disableWebPagePreview: true);
                                    }
                                }

                            }
                            else
                            {
                                if (activ.Award == null)
                                {
                                    builder = new EmbedBuilder()
                                        .WithTitle($"**{activ.Name}**")
                                        .WithThumbnailUrl(activ.Icon)
                                        .WithDescription($"получил(а) достижение!")
                                        .WithColor(Discord.Color.DarkRed)
                                        .AddField("Название:", activ.Mode, false)
                                        .AddField("Категоря:", activ.Categor, false);
                                    _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                    var embed = builder.Build();
                                    await chan.SendMessageAsync(null, false, embed);
                                    if (telegramBotNotification)
                                    {
                                        if (activ.Categor != "Рейды Legion" && activ.Categor != "Рейды Azeroth" && activ.Categor != "Рейды Draenor" && activ.Categor != "Рейды Pandaria")
                                        {

                                            await telegramClient.SendTextMessageAsync(
                                       chatId: settings.TelegramMainChatID,
                                       text: $"Всё збс, это <b>Достижение</b>!\n<a href =\"https://www.youtube.com/watch?v=d-diB65scQU&ab_channel=BobbyMcFerrinVEVO\">Don't Worry Be Happy</a>\nПолучил(а): <b>{activ.Name}</b>\nНазвание: <b>{activ.Mode}</b>\nКатегоря: <b>{activ.Categor}</b>",
                                       parseMode: ParseMode.Html, disableWebPagePreview: true);
                                        }
                                    }
                                }

                                else if (activ.Award != null)
                                {
                                    builder = new EmbedBuilder()
                                        .WithTitle($"**{activ.Name}**")
                                        .WithThumbnailUrl(activ.Icon)
                                        .WithDescription($"получил(а) достижение!")
                                        .WithColor(Discord.Color.DarkRed)
                                        .AddField("Название:", activ.Mode, false)
                                        .AddField("Категоря:", activ.Categor, false)
                                        .AddField("Награда:", activ.Award, false);
                                    _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                    var embed = builder.Build();
                                    await chan.SendMessageAsync(null, false, embed);
                                    if (telegramBotNotification)
                                    {
                                        if (activ.Categor != "Рейды Legion" && activ.Categor != "Рейды Azeroth" && activ.Categor != "Рейды Draenor" && activ.Categor != "Рейды Pandaria")
                                        {

                                            await telegramClient.SendTextMessageAsync(
                                       chatId: settings.TelegramMainChatID,
                                       text: $"Всё збс, это <b>Достижение</b>!\n<a href =\"https://www.youtube.com/watch?v=d-diB65scQU&ab_channel=BobbyMcFerrinVEVO\">Don't Worry Be Happy</a>\nПолучил(а): <b>{activ.Name}</b>\nНазвание: <b>{activ.Mode}</b>\nКатегоря: <b>{activ.Categor}</b>\nНаграда: <b>{activ.Award}</b>",
                                       parseMode: ParseMode.Html, disableWebPagePreview: true);
                                        }
                                    }
                                }

                            }
                        }
                        else if (activ.Type == "ENCOUNTER")
                        {
                            if (activ.Icon == null)
                            {
                                builder = new EmbedBuilder()
                                    .WithTitle($"**Гильдия одержала победу!**")
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Босс:", activ.Mode, false)
                                    .AddField("Режим:", activ.Categor, false);

                                ;
                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                var embed = builder.Build();
                                await chan.SendMessageAsync(null, false, embed);
                                if (telegramBotNotification)
                                {
                                    await telegramClient.SendTextMessageAsync(
                                   chatId: settings.TelegramMainChatID,
                                   text: $"<b>Гильдия одержала победу!</b>\nБосс: {activ.Mode}\nРежим: {activ.Categor}"
                                   , parseMode: ParseMode.Html, disableWebPagePreview: true
                                   );
                                }
                            }
                            else if (activ.Icon != null)
                            {
                                builder = new EmbedBuilder()
                                    .WithTitle($"**Гильдия одержала победу!**")
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Рейд:", activ.Categor, false)
                                    .AddField("Босс:", activ.Name, false)
                                    .AddField("Режим:", activ.Mode, false).WithImageUrl(activ.Icon);

                                ;
                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                var embed = builder.Build();
                                await chan.SendMessageAsync(null, false, embed);
                                if (telegramBotNotification)
                                {
                                    await telegramClient.SendPhotoAsync(
                                  chatId: settings.TelegramMainChatID,
                                 photo: activ.Icon,
                                 caption: $"<b>Гильдия одержала победу!</b>\nРейд: <b>{activ.Categor}</b>\nБосс: <b>{activ.Name}</b>\nРежим: <b>{activ.Mode}</b>"
                                  , parseMode: ParseMode.Html
                                  );
                                }
                            }

                        }


                    }
                    string message = ("Отправленно оповещение об активности гильдии!");
                    Functions.WriteLogs(message, "notification");
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    string message = $"\nOnTimerHandlerActivity Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                string message = $"OnTimerHandlerActivity Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static DiscordBot.Program;

namespace DiscordBot 
{
    class TelegramTextCommands
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient telegramBot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message!.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            string chatName;
            if (update.Message.Chat.Title != null)
            {
                chatName = update.Message.Chat.Title;
            }
            else
            {
                chatName = update.Message.Chat.FirstName + " " + update.Message.Chat.LastName;
            }

            var messageText = update.Message.Text;
            if (update.Message.Text.Contains("/") && update.Message.Text.Contains("@WowApiBot"))
            {
                Console.WriteLine($"Received a '{messageText}' message in chat {chatName}.");
            }

            switch (update.Message.Text)
            {
                case var s when s.Contains("/guild"):
                    GuildInfo guildInfo = new();
                    var fullInfo = guildInfo.GetGuildInfo();


                    if (!fullInfo.Error)
                    {


                        await telegramClient.SendTextMessageAsync(
                                  chatId,
                                 text: $"Информация о Гильдии : <b>{fullInfo.Name}</b>" +
                            $"\nФракция: <b>{fullInfo.Faction}</b>" +
                            $"\nЛидер: <b>{fullInfo.Leader}</b>" +
                            $"\nЧленов гильдии: <b>{fullInfo.MemberCount}</b>" +
                            $"\nДостижения: <b>{fullInfo.Achievement}</b>" +
                            $"\nРейд Прогресс: <b>{fullInfo.RaidProgress}</b>" +
                            $"\nМесто: <b>{fullInfo.RaidRankRealm.Replace("**Сервер**:", "")}</b>" +
                            $"\nОснована: <b>{fullInfo.TimeCreate}</b>"
                            , parseMode: ParseMode.Html, disableWebPagePreview: true);
                    }
                    else
                    {

                        await telegramClient.SendTextMessageAsync(
                                   chatId, "<b>Ошибка</b>\nПроблема на сервере.\nПопробуй позже.", parseMode: ParseMode.Html);
                    }
                    break;
                case var s when s.Contains("/token"):

                    var tokenprice = Functions.GetWebJson<TokenWarcraft>("https://eu.api.blizzard.com/data/wow/token/index?namespace=dynamic-eu&locale=ru_RU&access_token=" + Program.tokenWow);
                    if (tokenprice != null)
                    {
                        await telegramClient.SendTextMessageAsync(
                              chatId, $"<b>\"Жетон WoW\"</b>\nЦена: <b>{tokenprice.price / 10000}</b> золотых\nВремя обновления: {Functions.FromUnixTimeStampToDateTimeUTC(tokenprice.last_updated_timestamp.ToString())} (UTC)", parseMode: ParseMode.Html);

                    }
                    break;

                case var s when s.Contains("/lastlog"):
                    try
                    {
                        GuildLogs checklogs = new();
                        var newLog = checklogs.GetGuildLogChange();
                        if (newLog != null)
                        {
                            string text = $"Лог от {newLog.Date}" +
                         $"\n<b>Рейд:</b> {newLog.Dungeon}" +
                         $"\n<b>Убитые боссы ( {newLog.KillBoss} ):</b> {string.Join(",", newLog.BossKilling)}" +
                         $"\n<b>Вайпов:</b> {newLog.WipeBoss}" +
                         $"\n<b>Лучший трай:</b> {newLog.BestWipeTryName}({newLog.BestWipeTryPer}%)" +
                         $"\n<b>Продолжительность рейда:</b> {newLog.RaidTime}" +
                         $"\n<b><a href =\"https://www.wipefest.gg/report/{newLog.ID}\">WipeFest</a></b>" +
                         $"\n<b><a href =\"https://wowanalyzer.com/report/{newLog.ID}\">WoWAnalyzer</a></b>" +
                         $"\n<b><a href =\"https://ru.warcraftlogs.com/guild/reports-list/47723/\">Все логи</a></b>";
                            var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Просмотр", newLog.Link));
                            await telegramClient.SendTextMessageAsync(chatId, text, replyMarkup: keyboard, parseMode: ParseMode.Html, disableWebPagePreview: true);

                        }
                    }
                    catch (WebException e)
                    {
                        if (e.Status == WebExceptionStatus.ProtocolError)
                        {
                            string message = $"\nOnTimerHandlerLog Error: {e.Message}";
                            Functions.WriteLogs(message, "error");
                        }
                    }
                    catch (Exception e)
                    {
                        string message = $"OnTimerHandlerLog Error: {e.Message}";
                        Functions.WriteLogs(message, "error");
                    }
                    break;
            }
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }




    }
}

using Discord;
using System;
using System.Collections.Generic;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CheckRoster
    {
        public static void OnTimerHandlerCheckRoster(object obj)
        {
            if (settings.EnableCheckRoster)
            {
                try
                {
                    GuildInfo guild = new();
                    guild.GetGuildRosterChange();
                    if (guild.GetInviteRoster() != null)
                    {
                        GetLeaveInvChar(guild.GetInviteRoster(), "invite");
                    }
                    if (guild.GetLeaveRoster() != null)
                    {
                        GetLeaveInvChar(guild.GetLeaveRoster(), "leave");
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        string message = $"\nOnTimerHandlerRoster Error: {e.Message}";
                        Functions.WriteLogs(message, "error");
                    }
                }
                catch (Exception e)
                {
                    string message = $"OnTimerHandlerRoster Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }

        }
        private static async void GetLeaveInvChar(List<RosterLeaveInv> list, string type)
        {


            foreach (RosterLeaveInv inv in list)
            {
                var builder = new EmbedBuilder();
                CharInfo pers = new();
                string text = null;
                string message = null;
                if (type == "leave")
                {
                    text = $"Покинул(а) гильдию, информация о персонаже:";
                    message = "Отправленно оповещение о тех кто покинул в гильдию!";
                }
                else if (type == "invite")
                {
                    text = $"Вступил(а) в гильдию, информация о персонаже :";
                    message = "Отправленно оповещение о Вступлении в гильдию!";
                }
                var fullInfo = pers.GetCharInfo(inv.Name);
                if (fullInfo != null)
                {
                    if (Convert.ToInt32(inv.LVL) == 70)
                    {

                        builder = new EmbedBuilder()
                            .WithTitle($"**{fullInfo.Name}**({inv.LVL} уровень) {fullInfo.Race}")
                            .WithUrl(fullInfo.LinkBnet)
                            .WithDescription($"**{text}**")
                            .WithColor(Discord.Color.DarkRed)
                            .AddField("Уровень\nпредметов:", fullInfo.ILvl, true)
                            .WithImageUrl(fullInfo.ImageCharInset)
                            .AddField("Класс:", fullInfo.Class, true).AddField("Специализация:", fullInfo.Spec, true)
                            //.AddField("Ковенант:", fullInfo.Coven, true).AddField("Медиум:", fullInfo.CovenSoul, true)
                            .AddField("Рейд прогресс:", fullInfo.RaidProgress, true).AddField("Счет Мифик+:", fullInfo.MythicPlus, true);
                        if (settings.TelegramNotificationEnable)
                        {
                            await telegramClient.SendPhotoAsync(
                            photo: fullInfo.ImageCharInset,
                            chatId: settings.TelegramChatID,
                            caption: $"{fullInfo.Name}({inv.LVL} уровень) {fullInfo.Race}\n{text}\nУровень предметов: {fullInfo.ILvl}\nКласс: {fullInfo.Class}\nСпециализация: {fullInfo.Spec}" +
                            $"\nКовенант: {fullInfo.Coven}\nМедиум: {fullInfo.CovenSoul}" +
                            $"\nРейд прогресс: {fullInfo.RaidProgress}\nСчет Мифик+: {fullInfo.MythicPlus}"
                            , parseMode: ParseMode.Html);
                        }
                    }
                    else if (Convert.ToInt32(inv.LVL) < 70)
                    {
                        builder = new EmbedBuilder().WithTitle($"**{fullInfo.Name}**({inv.LVL} уровень) {fullInfo.Race}").WithUrl(fullInfo.LinkBnet).WithDescription($"**{text}**").WithColor(Discord.Color.DarkRed).AddField("Уровень\nпредметов:", fullInfo.ILvl, true).WithImageUrl(fullInfo.ImageCharInset)
                          .AddField("Класс:", fullInfo.Class, true).AddField("Специализация:", fullInfo.Spec, true);
                        if (settings.TelegramNotificationEnable)
                        {
                            await telegramClient.SendPhotoAsync(
                            photo: fullInfo.ImageCharInset,
                            chatId: settings.TelegramChatID,
                            caption: $"{fullInfo.Name}({inv.LVL} уровень) {fullInfo.Race}\n{text}\nУровень предметов: {fullInfo.ILvl}\nКласс: {fullInfo.Class}\nСпециализация: {fullInfo.Spec}",
                            parseMode: ParseMode.Html);
                        }

                    }
                    _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                    var chan = _mainChat.GetChannel(settings.DiscordRosterChannelId) as IMessageChannel;
                    var embed = builder.Build();


                    await chan.SendMessageAsync(null, false, embed);


                    Functions.WriteLogs(message, "notification");
                }
            }
        }

    }
}

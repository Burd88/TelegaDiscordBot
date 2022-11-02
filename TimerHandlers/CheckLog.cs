using Discord;
using System;
using System.Net;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CheckLog
    {
        public static async void OnTimerHandlerCheckLog(object obj)
        {
            if (settings.EnableCheckLastLog)
            {
                try
                {

                    GuildLogs checklogs = new();
                    var newLog = checklogs.GetGuildLogChange();

                    if (newLog != null)
                    {
                        var builder = new EmbedBuilder()
                            .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                            .WithImageUrl(newLog.InstanceImg)
                             .WithTitle($"Крайний рейд-лог Гильдии \"Сердце Греха\"\n{newLog.Date}")
                             .WithDescription($"[Открыть на сайте]({newLog.Link})" +
                             $"\n**Рейд:** {newLog.Dungeon}" +
                             $"\n**Убитые боссы ( {newLog.KillBoss} ):** {string.Join(",", newLog.BossKilling)}" +
                             $"\n**Вайпов:** {newLog.WipeBoss}" +
                             $"\n**Лучший трай:** {newLog.BestWipeTryName}({newLog.BestWipeTryPer}%)" +
                             $"\n**Продолжительность рейда:** {newLog.RaidTime}" +
                             $"\n[WipeFest](https://www.wipefest.gg/report/{newLog.ID})" +
                             $"\n[WoWAnalyzer](https://wowanalyzer.com/report/{newLog.ID})" +
                            $"\n[Все логи](https://ru.warcraftlogs.com/guild/reports-list/47723/)")
                             .WithColor(Discord.Color.DarkRed)

                             .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");

                        if (settings != null && builder != null)
                        {
                            if (settings.DiscordLogChannelId != 0)
                            {
                                if (settings.DiscordLogMessageId != 0)
                                {
                                    if (discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(settings.DiscordLogChannelId).GetMessageAsync(settings.DiscordLogMessageId).Result != null)
                                    {
                                        await discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(settings.DiscordLogChannelId).ModifyMessageAsync(settings.DiscordLogMessageId, msg => msg.Embed = builder.Build());
                                    }
                                    else
                                    {
                                        _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                                        var chan = _mainChat.GetChannel(settings.DiscordLogChannelId) as IMessageChannel;
                                        var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                        settings.DiscordLogMessageId = mess.Id;
                                        Functions.WriteJSon(settings, "BotSettings");
                                    }

                                }
                                else
                                {
                                    _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordLogChannelId) as IMessageChannel;
                                    var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                    settings.DiscordLogMessageId = mess.Id;
                                    Functions.WriteJSon(settings, "BotSettings");
                                }

                            }
                            //  await discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(settings.DiscordLogChannelId).ModifyMessageAsync(958994640487481396, msg => msg.Embed = builder.Build());

                        }

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
            }

        }
    }
}

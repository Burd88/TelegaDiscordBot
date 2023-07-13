using Discord;
using System;
using System.Collections.Generic;
using System.Net;
using static DiscordBot.Program;
namespace DiscordBot

{
    class WarframeCheckEvents
    {
        private static List<string> eventsList;
        private static void GetEvents()
        {
            eventsList = new();
            List<Events> events = Functions.GetWebJson<List<Events>>($"https://api.warframestat.us/pc/ru/events");
            if (events != null)
            {
                if (events.Count != 0)
                {
                    foreach (Events evnt in events)
                    {
                        TimeSpan ex = (evnt.expiry - DateTime.UtcNow);
                        //Console.WriteLine(ex);
                        string statusevent;
                        if (evnt.node != null && evnt.currentScore != 0)
                        {
                            statusevent = "Место: **" + evnt.node + "**\nСтатус: " + evnt.currentScore + "/" + evnt.maximumScore;
                        }
                        else if (evnt.node != null && evnt.currentScore == 0)
                        {
                            statusevent = "Место: **" + evnt.node + "**";
                        }
                        else
                        {
                            statusevent = "Место: **" + evnt.victimNode + "**\n" + "До победы осталось: " + evnt.health + "%";
                        }
                        eventsList.Add($"**{evnt.description}**\n{evnt.tooltip}\nЗакончиться через: {ex.Days}д {ex.Hours}ч {ex.Minutes}м \n{statusevent}");
                    }
                }
                else
                {
                    eventsList.Add($"**В данный момент никаких событий нету!**");
                }




            }




        }
        public static async void OnTimerHandlerCheckWarframeEvents(object obj)
        {

            try
            {

                GetEvents();

                if (eventsList != null)
                {
                    string str1 = "";
                    foreach (string str in eventsList)
                    {
                        str1 = str1 + str + "\n\n";
                    }

                    var builder = new EmbedBuilder()
                        .WithThumbnailUrl("https://cdn.discordapp.com/icons/640231332735090698/053d253f7c60097ceaba5317830f6518.webp?size=100")

                         .WithTitle($"События")
                         .WithDescription($"{str1}")
                         .WithColor(Discord.Color.DarkRed).WithFooter(footer => footer.Text = $"Обновлено: {DateTime.Now} (+4 Мск) ");

                    // .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                    //await discordClient.GetGuild(373317758634557451).GetTextChannel(373317758634557453).ModifyMessageAsync(1016981991700303903, msg => msg.Embed = builder.Build());
                    //   _mainChat = discordClient.GetGuild(373317758634557451);

                    //   var chan = _mainChat.GetChannel(373317758634557453) as IMessageChannel;

                    //   var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;

                    if (settings != null && builder != null)
                    {
                        if (settings.DiscordWarframeEventsChannelId != 0)
                        {
                            if (settings.DiscordWarframeEventsMessageId != 0)
                            {
                                if (discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeEventsChannelId).GetMessageAsync(settings.DiscordWarframeEventsMessageId).Result != null)
                                {
                                    await discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeEventsChannelId).ModifyMessageAsync(settings.DiscordWarframeEventsMessageId, msg => msg.Embed = builder.Build());
                                }
                                else
                                {
                                    _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordWarframeEventsChannelId) as IMessageChannel;
                                    var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                    settings.DiscordWarframeEventsMessageId = mess.Id;
                                    Functions.WriteJSon(settings, "BotSettings");
                                }

                            }
                            else
                            {
                                _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordWarframeEventsChannelId) as IMessageChannel;
                                var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                settings.DiscordWarframeEventsMessageId = mess.Id;
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
                    string message = $"\nOnTimerHandlerCheckWarframeEvents Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                string message = $"OnTimerHandlerCheckWarframeEvents Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }

    }

}


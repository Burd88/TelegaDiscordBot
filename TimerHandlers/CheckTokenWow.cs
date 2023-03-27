using Discord;
using System;
using System.Net;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CheckTokenWow
    {
       
        public static async void OnTimerHandlerCheckTokenWow(object obj)
        {
            if (settings.EnableCheckTokenWow)
            {
                try
                {

                    var tokenprice = Functions.GetWebJson<TokenWarcraft>("https://eu.api.blizzard.com/data/wow/token/index?namespace=dynamic-eu&locale={settings.Locale}&access_token=" + Program.tokenWow);
                    if (tokenprice != null)
                    {
                       
                        var builder = new EmbedBuilder()
                            .WithThumbnailUrl("https://wowtokenprices.com/assets/wowtoken-compressed.png")
                             .WithTitle($"**Жетон WoW**")
                             .WithDescription($"Цена: **{tokenprice.price / 10000}** золотых")//\nВремя обновления: {Functions.FromUnixTimeStampToDateTimeUTC(tokenprice.last_updated_timestamp.ToString())} (UTC)")
                             .WithColor(Discord.Color.DarkRed)

                             .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");

                        if (settings != null && builder != null)
                        {
                            if (settings.DiscordTokenWowChannelId != 0)
                            {
                                if (settings.DiscordTokenWowMessageId != 0)
                                {
                                    if (discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(settings.DiscordTokenWowChannelId).GetMessageAsync(settings.DiscordTokenWowMessageId).Result != null)
                                    {
                                        await discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(settings.DiscordTokenWowChannelId).ModifyMessageAsync(settings.DiscordTokenWowMessageId, msg => msg.Embed = builder.Build());
                                    }
                                    else
                                    {
                                        _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                                        var chan = _mainChat.GetChannel(settings.DiscordTokenWowChannelId) as IMessageChannel;
                                        var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                        settings.DiscordTokenWowMessageId = mess.Id;
                                        Functions.WriteJSon(settings, "BotSettings");
                                    }

                                }
                                else
                                {
                                    _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordTokenWowChannelId) as IMessageChannel;
                                    var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                    settings.DiscordTokenWowMessageId = mess.Id;
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

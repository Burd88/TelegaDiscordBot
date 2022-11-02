using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class WarframeCheckNightWave
    {
        public static async void OnTimerHandlerCheckWarframeNightWave(object obj)
        {

            try
            {

                NightWave nightWave = Functions.GetWebJson<NightWave>($"https://api.warframestat.us/pc/ru/nightwave");
                
                string nameSeasonWave = "";
                string endTimeSeasonWave = "";
                string dailyWave = "";
                string weeklyWave = "";
                string weeklyWaveElite = "";
                if (nightWave != null)
                {
                    nameSeasonWave = $"{nightWave.tag} ({nightWave.season} сезон";
                    TimeSpan ex = (nightWave.expiry - DateTime.UtcNow);
                    endTimeSeasonWave = $"Закончиться через : **{ex.Days}д {ex.Hours}ч {ex.Minutes}м**";
                   
                    if (nightWave.active)
                    {
                        foreach(ActiveChallenge activeWave in nightWave.activeChallenges)
                        {
                            TimeSpan exptime = (activeWave.expiry - DateTime.UtcNow);
                            if (activeWave.isDaily)
                            {

                                dailyWave += $"     **Название:** {activeWave.title} (**{exptime.Days}д {exptime.Hours}ч {exptime.Minutes}м**)\n      **Описание**: {activeWave.desc}\n       **Награда**: {activeWave.reputation}\n\n";
                            }
                            else
                            {
                                if (activeWave.isElite)
                                {
                                    weeklyWaveElite += $"     **Название:** {activeWave.title} (**{exptime.Days}д {exptime.Hours}ч {exptime.Minutes}м**)\n      **Описание**: {activeWave.desc}\n       **Награда**: {activeWave.reputation}\n\n";
                                }
                                else
                                {
                                    weeklyWave += $"     **Название:** {activeWave.title} (**{exptime.Days}д {exptime.Hours}ч {exptime.Minutes}м**)\n      **Описание**: {activeWave.desc}\n       **Награда**: {activeWave.reputation}\n\n";
                                }
                            }
                        }  
                    }
                    else
                    {
                        
                    }


                    var builder = new EmbedBuilder()
                        .WithThumbnailUrl("https://cdn.discordapp.com/icons/640231332735090698/053d253f7c60097ceaba5317830f6518.webp?size=100")

                         .WithTitle($"Ночная Волна : {nameSeasonWave}")
                         .WithDescription($"{endTimeSeasonWave}\n\n")
                         .AddField($"**Ежедневные:**", dailyWave,false)
                         .AddField($"**Еженедельные:**", weeklyWave, false)
                         .AddField($"**Еженедельные(Elite):**", weeklyWaveElite, false)
                         
                         .WithColor(Discord.Color.DarkRed).WithFooter(footer => footer.Text = $"Обновлено: {DateTime.Now} (+4 Мск) ");
                    /* foreach (SetInvasions str in invasionsList)
                     {
                         // str1 = str1 + index + $") **{str.NameInvasions}**\nАтакует: {str.AttackerName} ({str.Attackpersent}%) Награда: {str.RewardAttacker}\n" +
                         //        $"Защищает: {str.DefenderNmae} ({str.Defpersent}%) Награда: {str.RewardDeffenser}\nОжидаю завершения через: {str.ETA.Replace("d", "д").Replace("h", "ч").Replace("m", "м").Replace("s", "с")}\n\n";
                         //  index++;
                         builder.AddField(new EmbedFieldBuilder
                         {
                             Name = $"**{ str.NameInvasions }**",
                             Value = $"Атакует: {str.AttackerName} ({str.Attackpersent}%) Награда: {str.RewardAttacker}\n" +
                             $"Защищает: {str.DefenderNmae} ({str.Defpersent}%) Награда: {str.RewardDeffenser}\nОжидаю завершения через: {str.ETA.Replace("d", "д").Replace("h", "ч").Replace("m", "м").Replace("s", "с")}",
                             IsInline = true
                         });
                     }*/

                    // .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                    // await discordClient.GetGuild(373317758634557451).GetTextChannel(373317758634557453).ModifyMessageAsync(1017003311649529926, msg => msg.Embed = builder.Build());
                    // _mainChat = discordClient.GetGuild(373317758634557451);
                    //  
                    //  var chan = _mainChat.GetChannel(373317758634557453) as IMessageChannel;

                    // var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;

                    if (settings != null && builder != null)
                    {
                        if (settings.DiscordWarframeNightWaveChannelId != 0)
                        {
                            if (settings.DiscordWarframeNightWaveMessageId != 0)
                            {
                                if (discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeNightWaveChannelId).GetMessageAsync(settings.DiscordWarframeNightWaveMessageId).Result != null)
                                {
                                    await discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeNightWaveChannelId).ModifyMessageAsync(settings.DiscordWarframeNightWaveMessageId, msg => msg.Embed = builder.Build());
                                }
                                else
                                {
                                    _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordWarframeNightWaveChannelId) as IMessageChannel;
                                    var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                    settings.DiscordWarframeNightWaveMessageId = mess.Id;
                                    Functions.WriteJSon(settings, "BotSettings");
                                }

                            }
                            else
                            {
                                _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordWarframeNightWaveChannelId) as IMessageChannel;
                                var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                settings.DiscordWarframeNightWaveMessageId = mess.Id;
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
                    string message = $"\nOnTimerHandlerCheckWarframeVoidTrader Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                string message = $"OnTimerHandlerCheckWarframeVoidTrader Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
    }
}

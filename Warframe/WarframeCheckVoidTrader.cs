using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static DiscordBot.Program;
namespace DiscordBot

{
    class WarframeCheckVoidTrader
    {
       
      
        public static async void OnTimerHandlerCheckWarframeVoidTrader(object obj)
        {

            try
            {

                VoidTrader voidTrader = Functions.GetWebJson<VoidTrader>($"https://api.warframestat.us/pc/ru/voidTrader");

                
                if (voidTrader != null)
                {
                 
                    string str1 = "";
                    if (voidTrader.active)
                    {
                        str1 += $"Торговец: **{voidTrader.character}**(Активен)\n" +
                            $"Локация: **{voidTrader.location}**\n" +
                            $"Пропадет через: **{voidTrader.endString.Replace("d", "д").Replace("h", "ч").Replace("m", "м").Replace("s", "с")}**\nВ продаже:\n";
                        foreach (Inventory inv in voidTrader.inventory)
                        {
                            str1 += $"**{inv.item}**({inv.ducats}/{inv.credits})\n";
}
                    }
                    else
                    {
                        str1 += $"Торговец: **{voidTrader.character}**(Не активен)\n" +
                           $"Локация: **{voidTrader.location}**\n" +
                           $"Появится через: **{voidTrader.startString.Replace("d", "д").Replace("h", "ч").Replace("m", "м").Replace("s", "с")}**";
                    }
                    

                    var builder = new EmbedBuilder()
                        .WithThumbnailUrl("https://cdn.discordapp.com/icons/640231332735090698/053d253f7c60097ceaba5317830f6518.webp?size=100")

                         .WithTitle($"Торговцы")
                         .WithDescription($"{str1}")
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
                             if (settings.DiscordWarframeVoidTraderChannelId != 0)
                             {
                                 if (settings.DiscordWarframeVoidTraderMessageId != 0)
                                 {
                                     if (discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeVoidTraderChannelId).GetMessageAsync(settings.DiscordWarframeVoidTraderMessageId).Result != null)
                                     {
                                         await discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeVoidTraderChannelId).ModifyMessageAsync(settings.DiscordWarframeVoidTraderMessageId, msg => msg.Embed = builder.Build());
                                     }
                                     else
                                     {
                                         _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                         var chan = _mainChat.GetChannel(settings.DiscordWarframeVoidTraderChannelId) as IMessageChannel;
                                         var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                         settings.DiscordWarframeVoidTraderMessageId = mess.Id;
                                         Functions.WriteJSon(settings, "BotSettings");
                                     }

                                 }
                                 else
                                 {
                                     _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                     var chan = _mainChat.GetChannel(settings.DiscordWarframeVoidTraderChannelId) as IMessageChannel;
                                     var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                     settings.DiscordWarframeVoidTraderMessageId = mess.Id;
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


using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static DiscordBot.Program;
namespace DiscordBot

{
    class WarframeCheckInvasions
    {
        private static List<SortRewardInvasion> invasionsList;
        private static void GetInvasions()
        {
            invasionsList = new();
            List<Invasions> invasions = Functions.GetWebJson<List<Invasions>>($"https://api.warframestat.us/pc/ru/invasions");
            if (invasions != null)
            {
                foreach (Invasions invas in invasions)
                {
                    if (!invas.completed)
                    {
                        //   Console.WriteLine(invas.attackerReward.itemString +" "+ invas.defenderReward.itemString);
                        if (invasionsList.Count == 0)
                        {

                            invasionsList.Add(new SortRewardInvasion
                            {
                                RewardAttack = invas.attackerReward.itemString,
                                RewardDef = invas.defenderReward.itemString,
                                RewardAttackLink = invas.attackerReward.thumbnail,
                                RewardDefLink = invas.defenderReward.thumbnail,
                                listInvasions = new List<SetInvasions>() { new SetInvasions{NameInvasions = invas.node.Replace(")", "").Substring(invas.node.IndexOf("(")+1) + " (" + invas.desc + ")" ,AttackerName = invas.attackingFaction
                            , Attackpersent= Math.Round(invas.completion,2) , DefenderNmae = invas.defendingFaction, Defpersent = Math.Round(100 - Math.Round(invas.completion, 2),2),
                            ETA = invas.eta, RewardAttacker = $"\n[{invas.attackerReward.itemString}]({invas.attackerReward.thumbnail})"    , RewardDeffenser = $"\n[{invas.defenderReward.itemString}]({invas.defenderReward.thumbnail})" }   }
                            });

                        }
                        else
                        {
                            bool add = false;
                            foreach (SortRewardInvasion srt in invasionsList.ToList())
                            {

                                if (invas.attackerReward.itemString.ToLower() == srt.RewardAttack.ToLower() && invas.defenderReward.itemString.ToLower() == srt.RewardDef.ToLower())
                                {

                                    srt.listInvasions.Add(new SetInvasions
                                    {
                                        NameInvasions = invas.node.Replace(")", "").Substring(invas.node.IndexOf("(") + 1) + " (" + invas.desc + ")",
                                        AttackerName = invas.attackingFaction
                            ,
                                        Attackpersent = Math.Round(invas.completion, 2),
                                        DefenderNmae = invas.defendingFaction,
                                        Defpersent = Math.Round(100 - Math.Round(invas.completion, 2), 2),
                                        ETA = invas.eta,
                                        RewardAttacker = $"\n[{invas.attackerReward.itemString}]({invas.attackerReward.thumbnail})",
                                        RewardDeffenser = $"\n[{invas.defenderReward.itemString}]({invas.defenderReward.thumbnail})"
                                    });
                                    add = false;
                                    break;

                                }

                                else
                                {
                                    add = true;
                                }


                            }
                            if (add)
                            {



                                invasionsList.Add(new SortRewardInvasion
                                {
                                    RewardAttack = invas.attackerReward.itemString,
                                    RewardDef = invas.defenderReward.itemString,
                                    RewardAttackLink = invas.attackerReward.thumbnail,
                                    RewardDefLink = invas.defenderReward.thumbnail,
                                    listInvasions = new List<SetInvasions>() { new SetInvasions{NameInvasions = invas.node.Replace(")", "").Substring(invas.node.IndexOf("(")+1) + " (" + invas.desc + ")" ,AttackerName = invas.attackingFaction
                            , Attackpersent= Math.Round(invas.completion,2) , DefenderNmae = invas.defendingFaction, Defpersent = Math.Round(100 - Math.Round(invas.completion, 2),2),
                            ETA = invas.eta, RewardAttacker = $"\n[{invas.attackerReward.itemString}]({invas.attackerReward.thumbnail})"    , RewardDeffenser = $"\n[{invas.defenderReward.itemString}]({invas.defenderReward.thumbnail})" }   }
                                });


                            }
                        }
                    }
                }



            }



        }
        public static async void OnTimerHandlerCheckWarframeInvasions(object obj)
        {

            try
            {

                GetInvasions();


                if (invasionsList != null)
                {
                    // Console.WriteLine(invasionsList.Count());
                    string str1 = "";
                    foreach (SortRewardInvasion inv in invasionsList)
                    {

                        if (inv.RewardAttack != "")
                        {
                            str1 += $"Награда: **[{inv.RewardAttack}]({inv.RewardAttackLink}) или [{inv.RewardDef}]({inv.RewardDefLink})**\n";
                        }
                        else
                        {
                            str1 += $"Награда: **[{inv.RewardDef}]({inv.RewardDefLink})**\n";
                        }
                        foreach (SetInvasions sinv in inv.listInvasions.ToList())
                        {
                            str1 += $"**{sinv.NameInvasions}** : {sinv.AttackerName}({sinv.Attackpersent}%) vs {sinv.DefenderNmae} ({sinv.Defpersent}%) \n";// Ожидаю завершения через: {sinv.ETA.Replace("d", "д").Replace("h", "ч").Replace("m", "м").Replace("s", "с")}\n";
                        }
                        str1 += "\n";

                    }

                    var builder = new EmbedBuilder()
                        .WithThumbnailUrl("https://cdn.discordapp.com/icons/640231332735090698/053d253f7c60097ceaba5317830f6518.webp?size=100")

                         .WithTitle($"Вторжения")
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
                        if (settings.DiscordWarframeInvasionsChannelId != 0)
                        {
                            if (settings.DiscordWarframeInvasionsMessageId != 0)
                            {
                                if (discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeInvasionsChannelId).GetMessageAsync(settings.DiscordWarframeInvasionsMessageId).Result != null)
                                {
                                    await discordClient.GetGuild(settings.DiscordWarframeChatId).GetTextChannel(settings.DiscordWarframeInvasionsChannelId).ModifyMessageAsync(settings.DiscordWarframeInvasionsMessageId, msg => msg.Embed = builder.Build());
                                }
                                else
                                {
                                    _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordWarframeInvasionsChannelId) as IMessageChannel;
                                    var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                    settings.DiscordWarframeInvasionsMessageId = mess.Id;
                                    Functions.WriteJSon(settings, "BotSettings");
                                }

                            }
                            else
                            {
                                _mainChat = discordClient.GetGuild(settings.DiscordWarframeChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordWarframeInvasionsChannelId) as IMessageChannel;
                                var mess = chan.SendMessageAsync(null, false, embed: builder.Build()).Result;
                                settings.DiscordWarframeInvasionsMessageId = mess.Id;
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
                    string message = $"\nOnTimerHandlerCheckWarframeInvasions Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                string message = $"OnTimerHandlerCheckWarframeInvasions Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }

    }

}


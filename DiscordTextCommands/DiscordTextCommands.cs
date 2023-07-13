using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class DiscordTextCommands
    {
        public static async Task<Task> CommandsHandler(SocketMessage msg)
        {

            if (msg.Content.Contains("!"))
            {
                Console.WriteLine($"Комманда от {msg.Author} с текстом {msg.Content}");
            }

            var messege = msg.Content.ToLower();
            if (!msg.Author.IsBot)

                switch (messege)
                {
                    case "!test":
                        {
                            PoolRT.userDiscList = new();

                            await foreach (var s in discordClient.GetGuild(settings.DiscordTestChatId).GetUsersAsync())
                            {
                                foreach (var l in s)
                                {
                                    if (l.Nickname != null)
                                    {
                                        PoolRT.userDiscList.Add(new UserDiscord { ID = l.Id, Name = l.Nickname });
                                        // await msg.Author.SendMessageAsync(l.Nickname);
                                    }
                                    else
                                    {
                                        PoolRT.userDiscList.Add(new UserDiscord { ID = l.Id, Name = l.Username });
                                        //  await msg.Author.SendMessageAsync(l.Username);
                                    }

                                }


                            }
                            PoolRT.go = new();
                            PoolRT.nogo = new();
                            PoolRT.mbgo = new();

                            var buttonbuilder = new ComponentBuilder()
                                .WithButton("Приду", "go", style: ButtonStyle.Success)
                                .WithButton("Не приду", "nogo", style: ButtonStyle.Danger)
                                .WithButton("Опоздаю", "mbgo")
                                ;
                            var textbuilder = new EmbedBuilder()
                                  .WithTitle("**Через полтора часа идем в рейд \"Гробница Предвечных\"! Тебя ждать?**")
                                  .WithColor(Discord.Color.DarkRed)
                                  .WithDescription("Тактики знать **Обязательно**! \nПри себе иметь: **Фласки, Поты, Руны, Чары на предметах**! \nЕду Предоставим на +20!")
                                  .AddField("Тактики", "Тактики можно смотреть в разделе \"**[Тактики](https://discord.com/channels/219741774556430336/938739958695489596)**\"", false)
                                  .WithImageUrl("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSeDhzUwFEg6HESybW2BScFMwIMJy7pbpEcXA&usqp=CAU");


                            _mainChat = discordClient.GetGuild(settings.DiscordTestChatId);
                            var chan = _mainChat.GetChannel(settings.DiscordTestMainChannelId) as IMessageChannel;
                            PoolRT.discordmessagepool = chan.SendMessageAsync("@here Всем привет!", false, embed: textbuilder.Build(), components: buttonbuilder.Build()).Result;
                            PoolRT.discordmessageresultpool = chan.SendMessageAsync($"Результат:\nПриду: \nНе приду: \nОпоздаю: ").Result;
                            break;

                        }
                    case "!needpool":
                        {

                            if (settings.EnablePoolRT)
                            {
                                settings.EnablePoolRT = false;
                                await msg.Author.SendMessageAsync("Отключены опросы перед РТ");
                            }
                            else
                            {
                                settings.EnablePoolRT = true;
                                await msg.Author.SendMessageAsync("Включены опросы перед РТ");
                            }
                            Functions.WriteJSon(settings, "BotSettings");

                            break;

                        }
                    case "!addpool":
                        {

                            if (settings.EnableAddtionalRT)
                            {
                                settings.EnableAddtionalRT = false;
                                await msg.Author.SendMessageAsync("Отключены опросы перед допРТ во вторник");
                            }
                            else
                            {
                                settings.EnableAddtionalRT = true;
                                await msg.Author.SendMessageAsync("Включены опросы перед допРТ во вторник");
                            }
                            Functions.WriteJSon(settings, "BotSettings");

                            break;

                        }
                    case "!setrole":
                        {

                            SetDiscordUserRole.OnTimerHandlerSetDiscordUserRole(msg.Author.Id);

                            await msg.Author.SendMessageAsync("Роли назначаются ждите");
                            break;

                        }


                    case "!help" or "!рудз":
                        {
                            var builder = new EmbedBuilder();




                            builder = new EmbedBuilder().WithTitle("**Информация о моих командах: **")
                            .AddField("!мир или !realm", "Отображает статус игрового мира (пока работает только для РФ)", false)
                            .AddField("!аффикс или !affix", "Отображает аффиксы мифик+ подземелий (EU)", false)
                            .AddField("!guild или !гильдия", "Отображает информацию о гильдии", false)
                            .AddField("!чар или !char", "Отображает информацию о персонаже \n пример только для рф: !чар даркил " +
                            "\n пример для разных игровых миров: /чар Eletjul-Twisting Nether", false);
                            var emb = builder.Build();
                            await msg.Author.SendMessageAsync(null, false, emb);


                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }
                            break;
                        }
                    case "!мир" or "!realm":
                        {
                            var builder = new EmbedBuilder();
                            WowRealmInfo realmcheck = new();
                            var realStatus = realmcheck.GetRealmInfo();



                            if (realStatus.Error == false)
                            {

                                builder = new EmbedBuilder().WithTitle("**Информация об игровом мире**").AddField("Название:", realStatus.RealmName, true).AddField("Статус:", realStatus.RealmStatus, true);
                                var emb = builder.Build();
                                await msg.Author.SendMessageAsync(null, false, emb);

                            }
                            else if (realStatus.Error == true)
                            {
                                builder = new EmbedBuilder().WithTitle("**Информация об игровом мире**").AddField("Ошибка:", "Проблема на сервере.\nПопробуй позже.", true);
                                var emb = builder.Build();

                                await msg.Author.SendMessageAsync(null, false, emb);

                            }
                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }
                            break;
                        }
                    case var s when s.Contains("!affix") || s.Contains("!аффикс") || s.Contains("!афикс"):

                        {
                            var builder = new EmbedBuilder();

                            var affixs = Functions.GetWebJson<MythicPlusAffixCurrent>("https://raider.io/api/v1/mythic-plus/affixes?region=us&locale=ru");
                            if (affixs != null)
                            {

                                builder = new EmbedBuilder().WithTitle($"**{affixs.title}**").WithDescription("Мифик+ аффиксы на эту неделю.")
                                    .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                                    .AddField("(+2)", $"**[{affixs.affix_details[0].name}]({affixs.affix_details[0].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[0].description}**")
                                    .AddField("(+4)", $"**[{affixs.affix_details[1].name}]({affixs.affix_details[1].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[1].description}**")
                                    .AddField("(+7)", $"**[{affixs.affix_details[2].name}]({affixs.affix_details[2].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[2].description}**")
                                    .AddField("(+10)", $"**[{affixs.affix_details[3].name}]({affixs.affix_details[3].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[3].description}**");
                                var emb = builder.Build();
                                await msg.Author.SendMessageAsync(null, false, emb);
                                // var chan = discordClient.GetChannel(settings.DiscordAffixChannelId) as IMessageChannel;
                                /// await chan.SendMessageAsync(null, false, emb);
                            }
                            else
                            {
                                builder = new EmbedBuilder().WithTitle("**Информация об аффиксах на неделю**").AddField("Ошибка:", "Проблема на сервере.\nПопробуй позже.", true);
                                var emb = builder.Build();

                                await msg.Author.SendMessageAsync(null, false, emb);
                            }

                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }
                            break;
                        }

                    case var s when s.Contains("!tactics") || s.Contains("!тактики"):
                        {

                            var builder = new EmbedBuilder();



                            builder = new EmbedBuilder().WithTitle($"Тактики для рейда \"Гробница Предвечных\"").WithDescription($"Тактики основанны на данных ПТР серверов")
                            .WithColor(Discord.Color.DarkRed)
                            .AddField("Даусинь", $"**[Смотреть](https://youtu.be/8HrWXL6HJsY)**", true)
                            .AddField("Сколекс", $"**[Смотреть](https://youtu.be/XPb6mdIBFuE)**", true)
                            .AddField("Зи'мокс", $"**[Смотреть](https://youtu.be/feouDVYbUdI)**", true)
                            .AddField("Лихувим", $"**[Смотреть](https://youtu.be/F0CZpGMVWNk)**", true)
                            .AddField("Галондрий", $"**[Смотреть](https://youtu.be/aKdIMEWl-c0)**", true)
                            .AddField("Прототип Пантеона", $"**[Смотреть](https://youtu.be/v3627LcKJqk)**", true)
                            .AddField("Андуин Ринн", $"**[Смотреть](https://youtu.be/BsODvqPNVSE)**", true);



                            var embed = builder.Build();

                            await msg.Author.SendMessageAsync(null, false, embed);




                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }
                            break;
                        }
                    case var s when s.Contains("!чар ") || s.Contains("!char "):
                        {
                            CharInfo pers = new();

                            string name = messege.Replace("!char ", "").Replace("!чар ", "").Trim();
                            var builder = new EmbedBuilder();
                            var fullInfo = pers.GetCharInfo(name);

                            if (fullInfo != null)
                            {
                                builder = new EmbedBuilder()
                                    .WithTitle($"{fullInfo.Name}({fullInfo.Lvl} уровень) {fullInfo.Race}")
                                    .WithUrl(fullInfo.LinkBnet).WithDescription($"Информация о персонаже  :")
                                    .WithColor(Discord.Color.DarkRed).AddField("Уровень\nпредметов:", fullInfo.ILvl, true)
                                    .WithImageUrl(fullInfo.ImageCharMainRaw)
                                    .AddField("Класс:", fullInfo.Class, true)
                                    .AddField("Специализация:", fullInfo.Spec, true)
                                    .AddField("Гильдия:", fullInfo.Guild, true)
                                    .AddField("Ковенант:", fullInfo.Coven, true)
                                    .AddField("Медиум:", fullInfo.CovenSoul, true)
                                    .AddField("Рейд прогресс:", fullInfo.RaidProgress, true)
                                    .AddField("Счет Мифик+:", fullInfo.MythicPlus, true)
                                    .AddField("Статы:", fullInfo.Stats, true).AddField("В игре:", fullInfo.LastLogin, true);

                                var embed = builder.Build();

                                await msg.Author.SendMessageAsync(null, false, embed);

                            }
                            else
                            {

                                await msg.Author.SendMessageAsync("**Ошибка**\nНе корректное имя: " + name + ".\nЛибо проблемы на сервере.");

                            }


                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }
                            break;
                        }
                    case "!lastlog":
                        {
                            GuildLogs checklogs = new();
                            var newLog = checklogs.GetLogsInfo();

                            if (!newLog.Error)
                            {


                                var builder = new EmbedBuilder().WithTitle($"Последний загруженный лог от {newLog.Date}").WithDescription($"[Просмотр]({newLog.Link})\n\nИнформация о сражении в **{newLog.Dungeon}** :").WithColor(Discord.Color.DarkRed)

                                   .AddField("Боссов убито:", newLog.KillBoss, true)
                                   .AddField("Вайпов:", newLog.WipeBoss, true)
                                   .AddField("Продолжительность:", newLog.RaidTime, true)
                                   .AddField("WipeFest", $"[Просмотр](https://www.wipefest.gg/report/{newLog.ID})", true)
                                   .AddField("WoWAnalyzer", $"[Просмотр](https://wowanalyzer.com/report/{newLog.ID})", true)
                                   .AddField("Ссылка на все логи:", $"[Просмотр](https://ru.warcraftlogs.com/guild/reports-list/47723/)", true).WithFooter(footer => footer.Text = "Гильдия \"Сердце греха\".");

                                var embed = builder.Build();

                                await msg.Author.SendMessageAsync(null, false, embed);


                            }
                            else
                            {
                                await msg.Author.SendMessageAsync("**Ошибка**\nПроблема на сервере.\nПопробуй позже.");
                            }
                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }
                            break;
                        }
                    case "!guild" or "!гильдия":
                        {
                            var builder = new EmbedBuilder();
                            GuildInfo guildInfo = new();
                            var fullInfo = guildInfo.GetGuildInfo();


                            if (!fullInfo.Error)
                            {

                                builder = new EmbedBuilder()
                                    .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                                    .WithDescription($"Информация о Гильдии : [**{fullInfo.Name}**](https://worldofwarcraft.com/ru-ru/guild/eu/{settings.RealmSlug}/{settings.GuildName.ToLower().Replace(" ", "-")})")
                                    .WithColor(Discord.Color.DarkRed).AddField("Лидер:", fullInfo.Leader, true)
                                    .AddField("Членов гильдии:", fullInfo.MemberCount, true)
                                    .AddField("Достижения:", fullInfo.Achievement, true)
                                    .AddField("Рейд Прогресс:", fullInfo.RAidFull, true)
                                    .AddField("Основана", fullInfo.TimeCreate, false);

                                var embed = builder.Build();

                                await msg.Author.SendMessageAsync(null, false, embed);
                            }
                            else
                            {

                                await msg.Author.SendMessageAsync("**Ошибка**\nПроблема на сервере.\nПопробуй позже.");
                            }



                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }


                            break;
                        }
                    case "!update":
                        {
                            Static updatestatic = new();
                            Static.UpdateStaticRoster();

                            var builder = new EmbedBuilder()
                                    .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                                    .WithTitle("Состав рейд-статика")
                                    .WithDescription(Static.description)
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                                    .AddField("Танки:", Static.tank, false)
                                    .AddField("Хилы:", Static.heal, false)
                                    .AddField("Рдд:", Static.rdd, false)
                                    .AddField("Мдд:", Static.mdd, false)
                                    .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                            await discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());
                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }

                            break;
                        }
                    case var s when s.Contains("!del "):
                        {


                            string name = messege.Replace("!del ", "").Trim();
                            Static updatestatic = new();
                            Static.DeleteMemberStaticRoster(name);

                            var builder = new EmbedBuilder()
                                    .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                                    .WithTitle("Состав рейд-статика")
                                    .WithDescription(Static.description)
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                                    .AddField("Танки:", Static.tank, false)
                                    .AddField("Хилы:", Static.heal, false)
                                    .AddField("Рдд:", Static.rdd, false)
                                    .AddField("Мдд:", Static.mdd, false)
                                    .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                            await discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());
                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }

                            break;
                        }
                    case var s when s.Contains("!add "):
                        {


                            string name = messege.Replace("!add ", "").Trim();
                            Static updatestatic = new();
                            Static.AddMemberStaticRoster(name);

                            var builder = new EmbedBuilder()
                                    .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                                    .WithTitle("Состав рейд-статика")
                                    .WithDescription(Static.description)
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                                    .AddField("Танки:", Static.tank, false)
                                    .AddField("Хилы:", Static.heal, false)
                                    .AddField("Рдд:", Static.rdd, false)
                                    .AddField("Мдд:", Static.mdd, false)
                                    .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                            await discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());
                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }

                            break;
                        }
                }
            return Task.CompletedTask;
        }






    }
}

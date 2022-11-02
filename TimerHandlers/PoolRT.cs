using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static DiscordBot.Program;

namespace DiscordBot
{
    class PoolRT
    {
        public static List<string> go;
        public static List<string> nogo;
        public static List<string> mbgo;
        public static List<UserDiscord> userDiscList;
        public static Message telegrammessagepool;
        public static IUserMessage discordmessagepool;
        public static IUserMessage discordmessageresultpool;
        public static bool poolready = false;
        public static async void OnTimerHandlerPoolRT(object obj)
        {
            if (settings.EnablePoolRT)
            {

                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday)// || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                {
                    if (DateTime.Now.Hour == 21)
                    {
                        if (DateTime.Now.Minute == 00)
                        {
                            if (DateTime.Now.Second == 00)
                            {

                                userDiscList = new();

                                await foreach (var s in discordClient.GetGuild(settings.DiscordChatId).GetUsersAsync())
                                {
                                    foreach (var l in s)
                                    {
                                        if (l.Nickname != null)
                                        {
                                            userDiscList.Add(new UserDiscord { ID = l.Id, Name = l.Nickname });
                                            // await msg.Author.SendMessageAsync(l.Nickname);
                                        }
                                        else
                                        {
                                            userDiscList.Add(new UserDiscord { ID = l.Id, Name = l.Username });
                                            //  await msg.Author.SendMessageAsync(l.Username);
                                        }

                                    }


                                }
                                go = new();
                                nogo = new();
                                mbgo = new();

                                var buttonbuilder = new ComponentBuilder()
                                    .WithButton("Приду", "go", style: ButtonStyle.Success)
                                    .WithButton("Не приду", "nogo", style: ButtonStyle.Danger)
                                    .WithButton("Опоздаю", "mbgo")
                                    ;
                                var textbuilder = new EmbedBuilder()
                                      .WithTitle("**Через час идем в рейд \"Гробница Предвечных\"! Тебя ждать?**")
                                      .WithColor(Discord.Color.DarkRed)
                                      .WithDescription("Тактики знать **Обязательно**! \nПри себе иметь: **Фласки, Поты, Руны, Чары на предметах**! \nЕду Предоставим на +20!")
                                      .AddField("Тактики", "Тактики можно смотреть в разделе \"**[Тактики](https://discord.com/channels/219741774556430336/938739958695489596)**\"", false)
                                      .WithImageUrl("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSeDhzUwFEg6HESybW2BScFMwIMJy7pbpEcXA&usqp=CAU");

                                _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                                var chan = _mainChat.GetChannel(821981413858607104) as IMessageChannel;
                                discordmessagepool = chan.SendMessageAsync("@here Всем привет!", false, embed: textbuilder.Build(), components: buttonbuilder.Build()).Result;
                                discordmessageresultpool = chan.SendMessageAsync($"Приду: \nНе приду: \nОпоздаю: ").Result;

                                telegrammessagepool = new();
                                Console.WriteLine($"Сработал таймер для опроса о рт: {DateTime.Now}");
                                string[] options = new string[3];
                                options[0] = "Да";
                                options[1] = "Нет";
                                options[2] = "Опоздаю";
                                telegrammessagepool = await telegramClient.SendPollAsync(settings.TelegramChatID, "Через час идем в рейд \"Гробница Предвечных\"! Тебя ждать?", options, false);
                                poolready = true;
                            }

                        }

                    }
                    if (DateTime.Now.Hour == 22)
                    {
                        if (DateTime.Now.Minute == 30)
                        {
                            if (DateTime.Now.Second == 00)
                            {
                                if (poolready)
                                {
                                    await discordmessagepool.DeleteAsync();
                                    await discordmessageresultpool.DeleteAsync();
                                    await telegramClient.DeleteMessageAsync(settings.TelegramChatID, telegrammessagepool.MessageId);
                                    poolready = false;
                                }

                            }


                        }

                    }
                }
            }
            if (settings.EnableAddtionalRT)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                {
                    if (DateTime.Now.Hour == 20)
                    {
                        if (DateTime.Now.Minute == 00)
                        {
                            if (DateTime.Now.Second == 00)
                            {

                                userDiscList = new();

                                await foreach (var s in discordClient.GetGuild(settings.DiscordChatId).GetUsersAsync())
                                {
                                    foreach (var l in s)
                                    {
                                        if (l.Nickname != null)
                                        {
                                            userDiscList.Add(new UserDiscord { ID = l.Id, Name = l.Nickname });
                                            // await msg.Author.SendMessageAsync(l.Nickname);
                                        }
                                        else
                                        {
                                            userDiscList.Add(new UserDiscord { ID = l.Id, Name = l.Username });
                                            //  await msg.Author.SendMessageAsync(l.Username);
                                        }

                                    }


                                }
                                go = new();
                                nogo = new();
                                mbgo = new();

                                var buttonbuilder = new ComponentBuilder()
                                    .WithButton("Приду", "go", style: ButtonStyle.Success)
                                    .WithButton("Не приду", "nogo", style: ButtonStyle.Danger)
                                    .WithButton("Опоздаю", "mbgo")
                                    ;
                                var textbuilder = new EmbedBuilder()
                                      .WithTitle("**Через час идем в рейд \"Гробница Предвечных\"! Тебя ждать?**")
                                      .WithColor(Discord.Color.DarkRed)
                                      .WithDescription("Тактики знать **Обязательно**! \nПри себе иметь: **Фласки, Поты, Руны, Чары на предметах**! \nЕду Предоставим на +20!")
                                      .AddField("Тактики", "Тактики можно смотреть в разделе \"**[Тактики](https://discord.com/channels/219741774556430336/938739958695489596)**\"", false)
                                      .WithImageUrl("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSeDhzUwFEg6HESybW2BScFMwIMJy7pbpEcXA&usqp=CAU");

                                _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                                var chan = _mainChat.GetChannel(821981413858607104) as IMessageChannel;
                                discordmessagepool = chan.SendMessageAsync("@here Всем привет!", false, embed: textbuilder.Build(), components: buttonbuilder.Build()).Result;
                                discordmessageresultpool = chan.SendMessageAsync($"Результат:\nПриду: \nНе приду: \nОпоздаю: ").Result;

                                telegrammessagepool = new();
                                Console.WriteLine($"Сработал таймер для опроса о рт: {DateTime.Now}");
                                string[] options = new string[3];
                                options[0] = "Да";
                                options[1] = "Нет";
                                options[2] = "Опоздаю";
                                telegrammessagepool = await telegramClient.SendPollAsync(settings.TelegramChatID, "Через час идем в рейд \"Гробница Предвечных\"! Тебя ждать?", options, false);
                                poolready = true;
                            }

                        }

                    }
                    if (DateTime.Now.Hour == 21)
                    {
                        if (DateTime.Now.Minute == 30)
                        {
                            if (DateTime.Now.Second == 00)
                            {
                                if (poolready)
                                {
                                    await discordmessagepool.DeleteAsync();
                                    await discordmessageresultpool.DeleteAsync();
                                    await telegramClient.DeleteMessageAsync(settings.TelegramChatID, telegrammessagepool.MessageId);
                                    poolready = false;
                                }

                            }


                        }

                    }
                }
            }




        }

        public static async Task MyButtonHandler(SocketMessageComponent component)
        {
            //  _mainChat = discordClient.GetGuild(settings.DiscordTestChatId);
            //  var chan = _mainChat.GetChannel(settings.TestDiscordMainChannelId) as IMessageChannel;
            switch (component.Data.CustomId)
            {

                // Since we set our buttons custom id as 'custom-id', we can check for it like this:
                case "go":
                    // Lets respond by sending a message saying they clicked the button
                    try
                    {
                        string name = userDiscList.Find(x => x.ID == component.User.Id).Name;


                        var member = go.Find(x => x.ToLower() == name.ToLower());
                        if (member == null)
                        {
                            go.Add(name);
                        }
                        nogo.RemoveAll(x => x.ToLower() == name.ToLower());
                        mbgo.RemoveAll(x => x.ToLower() == name.ToLower());

                        await discordmessageresultpool.ModifyAsync(x => x.Content = $"Приду: {string.Join(",", go)}\nНе приду: {string.Join(",", nogo)}\nОпоздаю: {string.Join(",", mbgo)}");




                    }
                    catch { }
                    await component.DeferAsync();
                    break;
                case "nogo":
                    try
                    {
                        string namenot = userDiscList.Find(x => x.ID == component.User.Id).Name;

                        var membernot = nogo.Find(x => x.ToLower() == namenot.ToLower());
                        if (membernot == null)
                        {
                            nogo.Add(namenot);

                        }
                        go.RemoveAll(x => x.ToLower() == namenot.ToLower());
                        mbgo.RemoveAll(x => x.ToLower() == namenot.ToLower());

                        await discordmessageresultpool.ModifyAsync(x => x.Content = $"\nПриду: {string.Join(",", go)}\nНе приду: {string.Join(",", nogo)}\nОпоздаю: {string.Join(",", mbgo)}");



                    }
                    catch { }
                    await component.DeferAsync();
                    break;
                case "mbgo":
                    try
                    {
                        string namemb = userDiscList.Find(x => x.ID == component.User.Id).Name;

                        var membermb = mbgo.Find(x => x.ToLower() == namemb.ToLower());
                        if (membermb == null)
                        {
                            mbgo.Add(namemb);
                        }
                        go.RemoveAll(x => x.ToLower() == namemb.ToLower());
                        nogo.RemoveAll(x => x.ToLower() == namemb.ToLower());


                        await discordmessageresultpool.ModifyAsync(x => x.Content = $"Приду: {string.Join(",", go)}\nНе приду: {string.Join(",", nogo)}\nОпоздаю: {string.Join(",", mbgo)}");



                    }
                    catch { }
                    await component.DeferAsync();
                    break;
            }
        }
    }
}

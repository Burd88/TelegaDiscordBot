using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DiscordBot
{
    class Program
    {
        public static DiscordSocketClient discordClient;
        private static TelegramBotClient telegramClient;
        public static BotSettings settings;


        public static SocketGuild _mainChat;

        // private QueuedUpdateReceiver updateReceiver;
        [Obsolete]
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        [Obsolete]
        private async Task MainAsync()
        {
            settings = new BotSettings();
            Task<BotSettings> set = Functions.ReadJson<BotSettings>("BotSettings");
            settings = set.Result;

            int num = 0;
            AutorizationsBattleNet();
            Thread.Sleep(10000);
            //GetEncounter.LoadEncounterAll();
            //GetInstance.LoadInstanceAll();
            settings.RealmSlug = Functions.GetRealmSlug(settings.Realm);
            Console.WriteLine(
                $"DiscordBotToken : {settings.DiscordBotToken}\n" +
                $"TelegramBotToken : {settings.TelegramBotToken}\n" +
                $"BatNetToken : {settings.BatNetToken}\n" +
                $"BatNetSecretKey : {settings.BatNetSecretKey}\n" +
                $"Realm : {settings.Realm}\n" +
                $"Realm Slug : {settings.RealmSlug}\n" +
                $"RealmStatusType : {settings.RealmStatusType}\n" +
                $"Guild : {settings.Guild}\n" +
                $"DiscordMainChatId : {settings.DiscordMainChatId}\n" +
                $"DiscordActivityChannelId : {settings.DiscordActivityChannelId}\n" +
                $"DiscordRosterChannelId : {settings.DiscordRosterChannelId}\n" +
                $"DiscordMainChannel : {settings.DiscordMainChannelId}\n" +
                $"DiscordLogChannelId : {settings.DiscordLogChannelId}\n" +
                $"DiscordAffixChannelId : {settings.DiscordAffixChannelId}\n" +
                $"DiscordTestChatId : {settings.DiscordTestChatId}\n" +
                $"TestDiscordMainChannelId : {settings.TestDiscordMainChannelId}\n" +
                $"TelegramMainChatID : {settings.TelegramMainChatID}\n" +
                $"TelegramTestChatID : {settings.TelegramTestChatID}\n" +
                $"LastGuildAchiveTime : {settings.LastGuildAchiveTime}\n" +
                $"LastGuildActiveTime : {settings.LastGuildActiveTime}\n" +
                $"LastGuildLogTime : {settings.LastGuildLogTime}\n");

            Functions.WriteJSon(settings, "BotSettings");
            Thread.Sleep(5000);
            discordClient = new DiscordSocketClient();
            discordClient.MessageReceived += CommandsHandler;
            discordClient.Log += Log;
            discordClient.Ready += Client_Ready;
            discordClient.ButtonExecuted += MyButtonHandler;
            discordClient.SlashCommandExecuted += SlashCommandHandler;

            await discordClient.LoginAsync(TokenType.Bot, settings.DiscordBotToken);
            await discordClient.StartAsync();


            TimerCallback tmAutoriz = new(OnTimerHandlerAutorizationsBattleNet);
            Timer timerAutoriz = new(tmAutoriz, num, 3000, 1000 * 60 * 60);

            //  TimerCallback tmPoolRT = new(OnTimerHandlerPoolRT);
            //  Timer timerPoolRT = new(tmPoolRT, num, 0, 1000);

            TimerCallback tmCheckReboot = new(OnTimerHandlerheckReboot);
            Timer timerheckReboot = new(tmCheckReboot, num, 15000, 10000);

            TimerCallback tmCheckAffix = new(OnTimerHandlerheckAffix);
            Timer timerheckAffix = new(tmCheckAffix, num, 15000, 1000);

            TimerCallback tmactivity = new(OnTimerHandlerActivity);
            Timer timerActivity = new(tmactivity, num, 15000, 30000);

            TimerCallback tmlog = new(OnTimerHandlerLog);
            Timer timerlog = new(tmlog, num, 15000, 15000);

            TimerCallback tmachieve = new(OnTimerHandlerAchievements);
            Timer timerAchievements = new(tmachieve, num, 15000, 30000);

            TimerCallback tmroster = new(OnTimerHandlerRoster);
            Timer timerRoster = new(tmroster, num, 15000, 300000);

            TimerCallback tmsetRole = new(OnTimerHandlerSetUserRole);
            Timer timerSetRole = new(tmsetRole, num, 10000, 60000 * 15);

            TimerCallback tmUpdateStatic = new(OnTimerHandlerUpdateStatic);
            Timer timerUpdateStatic = new(tmUpdateStatic, num, 10000, 60000 * 20);
            telegramClient = new TelegramBotClient(settings.TelegramBotToken);
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
               );

            var telebot = await telegramClient.GetMeAsync();
            Console.WriteLine($"\nTelegram Bot started @{telebot.Username}\n");

            Console.ReadLine();
            cts.Cancel();
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "аффикс":
                    await HandleAffixCommand(command);
                    break;
                case "чар":
                    await HandleCharCommand(command);
                    break;
            }
        }

        private async Task HandleCharCommand(SocketSlashCommand command)
        {
            CharInfo pers = new();


            var builder = new EmbedBuilder();
            var fullInfo = pers.GetCharInfo((string)command.Data.Options.First().Value);

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



                await command.RespondAsync(embed: builder.Build(), ephemeral: true);

            }
            else
            {
                await command.RespondAsync(text: "**Ошибка**\nНе корректное имя: **" + (string)command.Data.Options.First().Value + "**.\nЛибо проблемы на сервере.", ephemeral: true);


            }

        }

        private async Task HandleAffixCommand(SocketSlashCommand command)
        {

            var affixs = Functions.GetWebJson<MythicPlusAffixCurrent>("https://raider.io/api/v1/mythic-plus/affixes?region=us&locale=ru");
            if (affixs != null)
            {

                var builder = new EmbedBuilder()
                      .WithTitle($"**{affixs.title}**")
                      .WithDescription("Мифик+ аффиксы на эту неделю.")
                      .AddField("(+2)", $"**[{affixs.affix_details[0].name}]({affixs.affix_details[0].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[0].description}**")
                      .AddField("(+4)", $"**[{affixs.affix_details[1].name}]({affixs.affix_details[1].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[1].description}**")
                      .AddField("(+7)", $"**[{affixs.affix_details[2].name}]({affixs.affix_details[2].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[2].description}**")
                      .AddField("(+10)", $"**[{affixs.affix_details[3].name}]({affixs.affix_details[3].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[3].description}**");


                await command.RespondAsync(embed: builder.Build(), ephemeral: true);
            }
            else
            {
                var builder = new EmbedBuilder().WithTitle("**Информация об аффиксах на неделю**").AddField("Ошибка:", "Проблема на сервере.\nПопробуй позже.", true);


                await command.RespondAsync(embed: builder.Build(), ephemeral: true);
            }
        }

        [Obsolete]
        private async Task Client_Ready()
        {
            // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
            var guild = discordClient.GetGuild(settings.DiscordMainChatId);

            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var affix = new SlashCommandBuilder()
                .WithName("аффикс")
                .WithDescription("Отображает аффиксы мифик+ подземелий(EU)");

            var charcommand = new SlashCommandBuilder()
                .WithName("чар")
                .WithDescription("Информация о персонаже")
                .AddOption("имя", ApplicationCommandOptionType.String, "Введите мя персонажа. Для Ревущего Фьорда просто \"имя персонажа\".Для других миров \"имя-игровой мир\"", isRequired: true)
                ;

            try
            {

                await guild.CreateApplicationCommandAsync(affix.Build());
                await discordClient.Rest.CreateGuildCommand(charcommand.Build(), settings.DiscordMainChatId);
            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient telegramBot, Update update, CancellationToken cancellationToken)
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
                chatName = update.Message.Chat.FirstName + update.Message.Chat.LastName;
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

                    var tokenprice = Functions.GetWebJson<Token>("https://eu.api.blizzard.com/data/wow/token/index?namespace=dynamic-eu&locale=ru_RU&access_token=" + Program.tokenWow);
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

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

        Message telegrammessagepool;
        IUserMessage discordmessagepool;
        IUserMessage discordmessageresultpool;
        bool poolready = false;
        private async void OnTimerHandlerPoolRT(object obj)
        {
            if (settings.NeedPoolRT == true)
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

                                await foreach (var s in discordClient.GetGuild(settings.DiscordMainChatId).GetUsersAsync())
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

                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(821981413858607104) as IMessageChannel;
                                discordmessagepool = chan.SendMessageAsync("@here Всем привет!", false, embed: textbuilder.Build(), components: buttonbuilder.Build()).Result;
                                discordmessageresultpool = chan.SendMessageAsync($"Приду: \nНе приду: \nОпоздаю: ").Result;

                                telegrammessagepool = new();
                                Console.WriteLine($"Сработал таймер для опроса о рт: {DateTime.Now}");
                                string[] options = new string[3];
                                options[0] = "Да";
                                options[1] = "Нет";
                                options[2] = "Опоздаю";
                                telegrammessagepool = await telegramClient.SendPollAsync(settings.TelegramMainChatID, "Через час идем в рейд \"Гробница Предвечных\"! Тебя ждать?", options, false);
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
                                    await telegramClient.DeleteMessageAsync(settings.TelegramMainChatID, telegrammessagepool.MessageId);
                                    poolready = false;
                                }

                            }


                        }

                    }
                }
            }
            if (settings.AddtionalRT == true)
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

                                await foreach (var s in discordClient.GetGuild(settings.DiscordMainChatId).GetUsersAsync())
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

                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(821981413858607104) as IMessageChannel;
                                discordmessagepool = chan.SendMessageAsync("@here Всем привет!", false, embed: textbuilder.Build(), components: buttonbuilder.Build()).Result;
                                discordmessageresultpool = chan.SendMessageAsync($"Результат:\nПриду: \nНе приду: \nОпоздаю: ").Result;

                                telegrammessagepool = new();
                                Console.WriteLine($"Сработал таймер для опроса о рт: {DateTime.Now}");
                                string[] options = new string[3];
                                options[0] = "Да";
                                options[1] = "Нет";
                                options[2] = "Опоздаю";
                                telegrammessagepool = await telegramClient.SendPollAsync(settings.TelegramMainChatID, "Через час идем в рейд \"Гробница Предвечных\"! Тебя ждать?", options, false);
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
                                    await telegramClient.DeleteMessageAsync(settings.TelegramMainChatID, telegrammessagepool.MessageId);
                                    poolready = false;
                                }

                            }


                        }

                    }
                }
            }




        }

        private List<string> go;
        private List<string> nogo;
        private List<string> mbgo;
        private async Task MyButtonHandler(SocketMessageComponent component)
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
        private static void OnTimerHandlerAutorizationsBattleNet(object obj)
        {
            AutorizationsBattleNet();
        }
        private static async void OnTimerHandlerheckReboot(object obj)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                WowRealmInfo realmcheck = new();
                string[] text = realmcheck.GetRealmInfoForTimer();
                if (text != null)
                {
                    _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                    var chan = _mainChat.GetChannel(settings.DiscordMainChannelId) as IMessageChannel;
                    var builder = new EmbedBuilder()
                         .WithTitle($"**Информация о техобслуживании!**")
                         .AddField($"{text[0]}", $"**{text[1]}**");

                    await chan.SendMessageAsync(null, false, builder.Build());
                    await telegramClient.SendTextMessageAsync(settings.TelegramMainChatID, $"{text[0]}\n{text[1]}", parseMode: ParseMode.Html);
                    string message = ("Отправленно оповещение о Тех.Работах!");
                    Functions.WriteLogs(message, "notification");
                }

            }

        }
        private static async void OnTimerHandlerheckAffix(object obj)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {

                if (DateTime.Now.Hour == 14)
                {
                    if (DateTime.Now.Minute == 00)
                    {
                        if (DateTime.Now.Second == 00)
                        {

                            var affixs = Functions.GetWebJson<MythicPlusAffixCurrent>("https://raider.io/api/v1/mythic-plus/affixes?region=us&locale=ru");
                            if (affixs != null)
                            {

                                var builder = new EmbedBuilder()
                                      .WithTitle($"**{affixs.title}**")
                                      .WithDescription("Мифик+ аффиксы на эту неделю обновлены.")
                                      .AddField("(+2)", $"**[{affixs.affix_details[0].name}]({affixs.affix_details[0].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[0].description}**")
                                      .AddField("(+4)", $"**[{affixs.affix_details[1].name}]({affixs.affix_details[1].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[1].description}**")
                                      .AddField("(+7)", $"**[{affixs.affix_details[2].name}]({affixs.affix_details[2].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[2].description}**")
                                      .AddField("(+10)", $"**[{affixs.affix_details[3].name}]({affixs.affix_details[3].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[3].description}**");
                                var emb = builder.Build();
                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordAffixChannelId) as IMessageChannel;
                                await chan.SendMessageAsync(null, false, emb);
                                string message = ($"Описание аффиксов на неделю отправлено!");
                                Functions.WriteLogs(message, "notification");
                                var text = "Мифик+ аффиксы на эту неделю обновлены.\n" +
                                  $"<b>(+2) {affixs.affix_details[0].name}</b>:\n {affixs.affix_details[0].description}\n" +
                                    $"<b>(+4) {affixs.affix_details[1].name}</b>:\n {affixs.affix_details[1].description}\n" +
                                   $"<b>(+7) {affixs.affix_details[2].name}</b>:\n {affixs.affix_details[2].description}\n" +
                                   $"<b>(+10) {affixs.affix_details[3].name}</b>:\n {affixs.affix_details[3].description}\n";

                                await telegramClient.SendTextMessageAsync(
                                    chatId: settings.TelegramMainChatID,
                                    text: text,
                                    parseMode: ParseMode.Html, disableWebPagePreview: true);
                            }



                        }
                    }
                }

            }

        }
        private static void OnTimerHandlerRoster(object obj)
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
                    if (Convert.ToInt32(inv.LVL) == 60)
                    {

                        builder = new EmbedBuilder()
                            .WithTitle($"**{fullInfo.Name}**({inv.LVL} уровень) {fullInfo.Race}")
                            .WithUrl(fullInfo.LinkBnet)
                            .WithDescription($"**{text}**")
                            .WithColor(Discord.Color.DarkRed)
                            .AddField("Уровень\nпредметов:", fullInfo.ILvl, true)
                            .WithImageUrl(fullInfo.ImageCharInset)
                            .AddField("Класс:", fullInfo.Class, true).AddField("Специализация:", fullInfo.Spec, true)
                            .AddField("Ковенант:", fullInfo.Coven, true).AddField("Медиум:", fullInfo.CovenSoul, true)
                            .AddField("Рейд прогресс:", fullInfo.RaidProgress, true).AddField("Счет Мифик+:", fullInfo.MythicPlus, true);
                        await telegramClient.SendPhotoAsync(
                            photo: fullInfo.ImageCharInset,
                            chatId: settings.TelegramMainChatID,
                            caption: $"{fullInfo.Name}({inv.LVL} уровень) {fullInfo.Race}\n{text}\nУровень предметов: {fullInfo.ILvl}\nКласс: {fullInfo.Class}\nСпециализация: {fullInfo.Spec}" +
                            $"\nКовенант: {fullInfo.Coven}\nМедиум: {fullInfo.CovenSoul}" +
                            $"\nРейд прогресс: {fullInfo.RaidProgress}\nСчет Мифик+: {fullInfo.MythicPlus}"
                            , parseMode: ParseMode.Html);
                    }
                    else if (Convert.ToInt32(inv.LVL) < 60)
                    {
                        builder = new EmbedBuilder().WithTitle($"**{fullInfo.Name}**({inv.LVL} уровень) {fullInfo.Race}").WithUrl(fullInfo.LinkBnet).WithDescription($"**{text}**").WithColor(Discord.Color.DarkRed).AddField("Уровень\nпредметов:", fullInfo.ILvl, true).WithImageUrl(fullInfo.ImageCharInset)
                          .AddField("Класс:", fullInfo.Class, true).AddField("Специализация:", fullInfo.Spec, true);
                        await telegramClient.SendPhotoAsync(
                            photo: fullInfo.ImageCharInset,
                            chatId: settings.TelegramMainChatID,
                            caption: $"{fullInfo.Name}({inv.LVL} уровень) {fullInfo.Race}\n{text}\nУровень предметов: {fullInfo.ILvl}\nКласс: {fullInfo.Class}\nСпециализация: {fullInfo.Spec}",
                            parseMode: ParseMode.Html);


                    }
                    _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                    var chan = _mainChat.GetChannel(settings.DiscordRosterChannelId) as IMessageChannel;
                    var embed = builder.Build();


                    await chan.SendMessageAsync(null, false, embed);


                    Functions.WriteLogs(message, "notification");
                }
            }
        }
        private static async void OnTimerHandlerActivity(object obj)
        {
            try
            {
                var builder = new EmbedBuilder();
                GuildActivity activity = new();
                var activNew = activity.GetGuildActivityChange();

                if (activNew != null && activNew.Count != 0)
                {
                    foreach (Activity activ in activNew)
                    {
                        if (activ.Type == "CHARACTER_ACHIEVEMENT")
                        {
                            if (activ.Icon == null)
                            {
                                builder = new EmbedBuilder()
                                    .WithTitle($"**{activ.Name}**")
                                    .WithDescription($"получил(а) достижение!")
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Название:", activ.Mode, false);
                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                var embed = builder.Build();
                                await chan.SendMessageAsync(null, false, embed);

                                if (activ.Categor != "Рейды Legion" && activ.Categor != "Рейды Azeroth" && activ.Categor != "Рейды Draenor" && activ.Categor != "Рейды Pandaria")
                                {

                                    await telegramClient.SendTextMessageAsync(
                                     chatId: settings.TelegramMainChatID,
                                     text: $"Всё збс, это <b>Достижение</b>!\n<a href =\"https://www.youtube.com/watch?v=d-diB65scQU&ab_channel=BobbyMcFerrinVEVO\">Don't Worry Be Happy</a>\nПолучил(а): <b>{activ.Name}</b>\nНазвание: <b>{activ.Mode}</b>",
                                     parseMode: ParseMode.Html, disableWebPagePreview: true);
                                }
                            }
                            else
                            {
                                if (activ.Award == null)
                                {
                                    builder = new EmbedBuilder()
                                        .WithTitle($"**{activ.Name}**")
                                        .WithThumbnailUrl(activ.Icon)
                                        .WithDescription($"получил(а) достижение!")
                                        .WithColor(Discord.Color.DarkRed)
                                        .AddField("Название:", activ.Mode, false)
                                        .AddField("Категоря:", activ.Categor, false);
                                    _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                    var embed = builder.Build();
                                    await chan.SendMessageAsync(null, false, embed);

                                    if (activ.Categor != "Рейды Legion" && activ.Categor != "Рейды Azeroth" && activ.Categor != "Рейды Draenor" && activ.Categor != "Рейды Pandaria")
                                    {

                                        await telegramClient.SendTextMessageAsync(
                                   chatId: settings.TelegramMainChatID,
                                   text: $"Всё збс, это <b>Достижение</b>!\n<a href =\"https://www.youtube.com/watch?v=d-diB65scQU&ab_channel=BobbyMcFerrinVEVO\">Don't Worry Be Happy</a>\nПолучил(а): <b>{activ.Name}</b>\nНазвание: <b>{activ.Mode}</b>\nКатегоря: <b>{activ.Categor}</b>",
                                   parseMode: ParseMode.Html, disableWebPagePreview: true);
                                    }

                                }

                                else if (activ.Award != null)
                                {
                                    builder = new EmbedBuilder()
                                        .WithTitle($"**{activ.Name}**")
                                        .WithThumbnailUrl(activ.Icon)
                                        .WithDescription($"получил(а) достижение!")
                                        .WithColor(Discord.Color.DarkRed)
                                        .AddField("Название:", activ.Mode, false)
                                        .AddField("Категоря:", activ.Categor, false)
                                        .AddField("Награда:", activ.Award, false);
                                    _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                    var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                    var embed = builder.Build();
                                    await chan.SendMessageAsync(null, false, embed);

                                    if (activ.Categor != "Рейды Legion" && activ.Categor != "Рейды Azeroth" && activ.Categor != "Рейды Draenor" && activ.Categor != "Рейды Pandaria")
                                    {

                                        await telegramClient.SendTextMessageAsync(
                                   chatId: settings.TelegramMainChatID,
                                   text: $"Всё збс, это <b>Достижение</b>!\n<a href =\"https://www.youtube.com/watch?v=d-diB65scQU&ab_channel=BobbyMcFerrinVEVO\">Don't Worry Be Happy</a>\nПолучил(а): <b>{activ.Name}</b>\nНазвание: <b>{activ.Mode}</b>\nКатегоря: <b>{activ.Categor}</b>\nНаграда: <b>{activ.Award}</b>",
                                   parseMode: ParseMode.Html, disableWebPagePreview: true);
                                    }
                                }

                            }
                        }
                        else if (activ.Type == "ENCOUNTER")
                        {
                            if (activ.Icon == null)
                            {
                                builder = new EmbedBuilder()
                                    .WithTitle($"**Гильдия одержала победу!**")
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Босс:", activ.Mode, false)
                                    .AddField("Режим:", activ.Categor, false);

                                ;
                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                var embed = builder.Build();
                                await chan.SendMessageAsync(null, false, embed);
                                await telegramClient.SendTextMessageAsync(
                                   chatId: settings.TelegramMainChatID,
                                   text: $"<b>Гильдия одержала победу!</b>\nБосс: {activ.Mode}\nРежим: {activ.Categor}"
                                   , parseMode: ParseMode.Html, disableWebPagePreview: true
                                   );
                            }
                            else if (activ.Icon != null)
                            {
                                builder = new EmbedBuilder()
                                    .WithTitle($"**Гильдия одержала победу!**")
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Рейд:", activ.Categor, false)
                                    .AddField("Босс:", activ.Name, false)
                                    .AddField("Режим:", activ.Mode, false).WithImageUrl(activ.Icon);

                                ;
                                _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                                var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                                var embed = builder.Build();
                                await chan.SendMessageAsync(null, false, embed);
                                await telegramClient.SendPhotoAsync(
                                  chatId: settings.TelegramMainChatID,
                                 photo: activ.Icon,
                                 caption: $"<b>Гильдия одержала победу!</b>\nРейд: <b>{activ.Categor}</b>\nБосс: <b>{activ.Name}</b>\nРежим: <b>{activ.Mode}</b>"
                                  , parseMode: ParseMode.Html
                                  );
                            }

                        }


                    }
                    string message = ("Отправленно оповещение об активности гильдии!");
                    Functions.WriteLogs(message, "notification");
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    string message = $"\nOnTimerHandlerActivity Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                string message = $"OnTimerHandlerActivity Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
        private static async void OnTimerHandlerLog(object obj)
        {
            try
            {
                GuildLogs checklogs = new();
                var newLog = checklogs.GetGuildLogChange();
                if (newLog != null)
                {
                    var builder = new EmbedBuilder()
                        .WithThumbnailUrl("https://render.worldofwarcraft.com/eu/guild/crest/102/emblem-102-dfa55a-b1002e.jpg")
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
                        await discordClient.GetGuild(settings.DiscordMainChatId).GetTextChannel(settings.DiscordLogChannelId).ModifyMessageAsync(958994640487481396, msg => msg.Embed = builder.Build());

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
        private static async void OnTimerHandlerAchievements(object obj)
        {
            try
            {
                var builder = new EmbedBuilder();
                GuildAchievements guildAchive = new();
                var achiev = guildAchive.GetGuildAchievementChange();
                if (achiev != null && achiev.Count != 0)
                {
                    foreach (Achievement achieve in achiev)
                    {

                        builder = new EmbedBuilder()
                            .WithTitle($"Гильдия получила достижение!")
                            .WithThumbnailUrl(achieve.Icon).WithColor(Discord.Color.DarkRed)
                            .AddField("Название:", achieve.Name, false)
                            .AddField("Категоря:", achieve.Category, false);
                        var embed = builder.Build();
                        _mainChat = discordClient.GetGuild(settings.DiscordMainChatId);
                        var chan = _mainChat.GetChannel(settings.DiscordActivityChannelId) as IMessageChannel;
                        await chan.SendMessageAsync(null, false, embed);
                        await telegramClient.SendTextMessageAsync(
                                 chatId: settings.TelegramMainChatID,

                                $"<b>Гильдия получила достижение!!</b>\nНазвание: {achieve.Name}\nКатегоря: {achieve.Category}"
                                 , parseMode: ParseMode.Html
                                 );
                    }
                    string message = ("Отправленно оповещение о получении достижения гильдии!");
                    Functions.WriteLogs(message, "notification");
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    string message = $"\nOnTimerHandlerAchievements Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {
                string message = $"OnTimerHandlerAchievements Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
           
            return Task.CompletedTask;
        }

        public static List<RosterLeaveInv> rosterGuild = new();

        private async void OnTimerHandlerSetUserRole(object obj)
        {
            try
            {
                // Console.WriteLine("\n Начинаю сверять роли ВОВ-ДИСКОРД \n");
                var allUser = discordClient.GetGuild(settings.DiscordMainChatId).GetUsersAsync(RequestOptions.Default);





                await foreach (var user in allUser)
                {
                    var roleGuildMaster = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Глава гильдии").Id;
                    var roleLieutenant = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Заместитель").Id;
                    var roleOfficer = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Офицер").Id;
                    var roleRaider = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Рейдер").Id;
                    var roleVeteran = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Ветеран").Id;
                    var roleWarrior = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Воин").Id;
                    var roleTwink = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Твинк").Id;
                    var roleNewbie = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Новичок").Id;
                    var roleAdmin = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Админ").Id;
                    var roleTest = discordClient.GetGuild(settings.DiscordMainChatId).Roles.FirstOrDefault(x => x.Name == "Тест").Id;
                    List<ulong> roleList = new() { roleGuildMaster, roleLieutenant, roleOfficer, roleRaider, roleVeteran, roleWarrior, roleTwink, roleNewbie };

                    foreach (IGuildUser us in user)
                    {
                        if (us.Id != 0)

                        {
                            if (!us.IsBot)
                            {
                                if (rosterGuild.Count != 0)
                                {

                                    if (us.Nickname != null)
                                    {
                                        var nick = rosterGuild.Find(x => us.Nickname.ToLower().Contains(x.Name.ToLower()));

                                        if (nick != null)
                                        {

                                            if (us.RoleIds.Count != 1)
                                            {
                                                //Console.WriteLine(us.RoleIds.Count);
                                                if (us.RoleIds.Count > 2)
                                                {
                                                    bool admin = false;
                                                    foreach (var role in us.RoleIds)
                                                    {
                                                        if (role == roleAdmin)
                                                        {
                                                            admin = true;
                                                            // Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, Admin");
                                                        }
                                                    }
                                                    if (!admin)
                                                    {

                                                        await us.RemoveRolesAsync(roleList);
                                                        await us.AddRoleAsync(roleList[nick.Rank]);
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[nick.Rank]).Name}");

                                                    }
                                                }
                                                else if (us.RoleIds.Count == 2)
                                                {
                                                    bool admin = false;
                                                    foreach (var role in us.RoleIds)
                                                    {
                                                        if (role == roleAdmin)
                                                        {
                                                            admin = true;
                                                            //   Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, Admin");
                                                        }
                                                    }
                                                    if (!admin)
                                                    {
                                                        //   Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, {client.GetGuild(settings.DiscordMainChatId).GetRole(us.RoleIds.ElementAt(1)).Id}    1  {roleNewbie}   2 {nick.Rank}   {roleList[nick.Rank]}    3   {roleList[7]}");

                                                        if (us.RoleIds.ElementAt(1) != roleList[nick.Rank])
                                                        {

                                                            await us.RemoveRolesAsync(roleList);
                                                            await us.AddRoleAsync(roleList[nick.Rank]);
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[nick.Rank]).Name}");

                                                        }

                                                    }
                                                    else
                                                    {
                                                        await us.AddRoleAsync(roleList[nick.Rank]);
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[nick.Rank]).Name}");
                                                    }

                                                }

                                            }
                                            else
                                            {

                                                await us.AddRoleAsync(roleList[nick.Rank]);
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[nick.Rank]).Name}");
                                            }

                                        }
                                        else
                                        {
                                            if (us.RoleIds.Count != 1)
                                            {
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.WriteLine("Пользователь не в гильдии!  Ник: " + us.Nickname + " Имя: " + us.Username + " ID: " + us.Id);
                                                await us.RemoveRolesAsync(roleList);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        var name = rosterGuild.Find(x => us.Username.ToLower().Contains(x.Name.ToLower()));

                                        if (name != null)
                                        {
                                            if (us.RoleIds.Count != 1)
                                            {
                                                //Console.WriteLine(us.RoleIds.Count);
                                                if (us.RoleIds.Count > 2)
                                                {
                                                    bool admin = false;
                                                    foreach (var role in us.RoleIds)
                                                    {
                                                        if (role == roleAdmin)
                                                        {
                                                            admin = true;
                                                            //    Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, Admin");
                                                        }
                                                    }
                                                    if (!admin)
                                                    {

                                                        await us.RemoveRolesAsync(roleList);
                                                        await us.AddRoleAsync(roleList[name.Rank]);
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[name.Rank]).Name}");

                                                    }
                                                }
                                                else if (us.RoleIds.Count == 2)
                                                {
                                                    bool admin = false;
                                                    foreach (var role in us.RoleIds)
                                                    {
                                                        if (role == roleAdmin)
                                                        {
                                                            admin = true;
                                                            //   Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, Admin");
                                                        }
                                                    }
                                                    if (!admin)
                                                    {
                                                        //Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, {client.GetGuild(settings.DiscordMainChatId).GetRole(us.RoleIds.ElementAt(1)).Id}     {roleNewbie}  {roleList[name.Rank]}    {roleList[7]}");
                                                        if (us.RoleIds.ElementAt(1) != roleList[name.Rank])
                                                        {

                                                            await us.RemoveRolesAsync(roleList);
                                                            await us.AddRoleAsync(roleList[name.Rank]);
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[name.Rank]).Name}");

                                                        }

                                                    }
                                                    else
                                                    {
                                                        await us.AddRoleAsync(roleList[name.Rank]);
                                                        Console.ForegroundColor = ConsoleColor.Green;
                                                        Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[name.Rank]).Name}");
                                                    }

                                                }

                                            }
                                            else
                                            {

                                                await us.AddRoleAsync(roleList[name.Rank]);
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine($"Изменение роли на сервере для Username: {us.Username}, Nickname: {us.Nickname}, назначаемая роль: {discordClient.GetGuild(settings.DiscordMainChatId).GetRole(roleList[name.Rank]).Name}");
                                            }
                                        }
                                        else
                                        {
                                            if (us.RoleIds.Count != 1)
                                            {
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.WriteLine("Пользователь не в гильдии!  Ник: " + us.Nickname + " Имя: " + us.Username + " ID: " + us.Id);
                                                await us.RemoveRolesAsync(roleList);
                                            }

                                        }

                                    }

                                }
                            }
                            else
                            {
                                //Console.ForegroundColor = ConsoleColor.Red;
                                //Console.WriteLine("Это бот! " + "Ник: " + us.Nickname + " Имя: " + us.Username + " ID: " + us.Id);
                            }





                        }
                    }




                }
                //await client.GetUser(i).SendMessageAsync("Сделанно");
                // Console.WriteLine("\n Закончил сверку ролей ВОВ-ДИСКОРД \n");
            }
            catch (Exception e)
            {
                Functions.WriteLogs(e.Message, "error");

            }




        }
        private async void OnTimerHandlerUpdateStatic(object obj)
        {
            try
            {
                Static updatestatic = new();
                updatestatic.UpdateStaticRoster();

                var builder = new EmbedBuilder()
                        .WithThumbnailUrl("https://render.worldofwarcraft.com/eu/guild/crest/102/emblem-102-dfa55a-b1002e.jpg")
                        .WithTitle("Состав рейд-статика")
                        .WithDescription(Static.description)
                        .WithColor(Discord.Color.DarkRed)
                        .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                        .AddField("Танки:", Static.tank, false)
                        .AddField("Хилы:", Static.heal, false)
                        .AddField("Рдд:", Static.rdd, false)
                        .AddField("Мдд", Static.mdd, false)
                        .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                await discordClient.GetGuild(settings.DiscordMainChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());

            }
            catch (Exception e)
            {
                Functions.WriteLogs(e.Message, "error");

            }




        }

        List<UserDiscord> userDiscList;

        private async Task<Task> CommandsHandler(SocketMessage msg)
        {

            if (msg.Content.Contains("!"))
            {
                Console.WriteLine($"Комманда от {msg.Author} с текстом {msg.Content}");
            }

            var messege = msg.Content.ToLower();
            if (!msg.Author.IsBot)

                switch (messege)
                {
                    case "!test1":
                        {
                            userDiscList = new();

                            await foreach (var s in discordClient.GetGuild(settings.DiscordTestChatId).GetUsersAsync())
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


                            break;

                        }
                    case "!test":
                        {

                            go = new();
                            nogo = new();
                            mbgo = new();

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
                            var chan = _mainChat.GetChannel(settings.TestDiscordMainChannelId) as IMessageChannel;
                            discordmessagepool = chan.SendMessageAsync("@here Всем привет!", false, embed: textbuilder.Build(), components: buttonbuilder.Build()).Result;
                            discordmessageresultpool = chan.SendMessageAsync($"Результат:\nПриду: \nНе приду: \nОпоздаю: ").Result;
                            break;

                        }
                    case "!needpool":
                        {

                            if (settings.NeedPoolRT)
                            {
                                settings.NeedPoolRT = false;
                                await msg.Author.SendMessageAsync("Отключены опросы перед РТ");
                            }
                            else
                            {
                                settings.NeedPoolRT = true;
                                await msg.Author.SendMessageAsync("Включены опросы перед РТ");
                            }
                            Functions.WriteJSon(settings, "BotSettings");

                            break;

                        }
                    case "!addpool":
                        {

                            if (settings.AddtionalRT)
                            {
                                settings.AddtionalRT = false;
                                await msg.Author.SendMessageAsync("Отключены опросы перед допРТ во вторник");
                            }
                            else
                            {
                                settings.AddtionalRT = true;
                                await msg.Author.SendMessageAsync("Включены опросы перед допРТ во вторник");
                            }
                            Functions.WriteJSon(settings, "BotSettings");

                            break;

                        }
                    case "!setrole":
                        {

                            OnTimerHandlerSetUserRole(msg.Author.Id);

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
                            else if (realStatus.Error == false)
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
                                    .WithThumbnailUrl("https://render.worldofwarcraft.com/eu/guild/crest/102/emblem-102-dfa55a-b1002e.jpg")
                                    .WithDescription($"Информация о Гильдии : [**{fullInfo.Name}**](https://worldofwarcraft.com/ru-ru/guild/eu/{settings.RealmSlug}/{settings.Guild.ToLower().Replace(" ", "-")})")
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

                            

                            if(!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }
                            

                            break;
                        }
                    case "!update":
                        {
                            Static updatestatic = new();
                            updatestatic.UpdateStaticRoster();

                            var builder = new EmbedBuilder()
                                    .WithThumbnailUrl("https://render.worldofwarcraft.com/eu/guild/crest/102/emblem-102-dfa55a-b1002e.jpg")
                                    .WithTitle("Состав рейд-статика")
                                    .WithDescription(Static.description)
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                                    .AddField("Танки:", Static.tank, false)
                                    .AddField("Хилы:", Static.heal, false)
                                    .AddField("Рдд:", Static.rdd, false)
                                    .AddField("Мдд:", Static.mdd, false)
                                    .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                            await discordClient.GetGuild(settings.DiscordMainChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());
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
                            updatestatic.DeleteMemberStaticRoster(name);

                            var builder = new EmbedBuilder()
                                    .WithThumbnailUrl("https://render.worldofwarcraft.com/eu/guild/crest/102/emblem-102-dfa55a-b1002e.jpg")
                                    .WithTitle("Состав рейд-статика")
                                    .WithDescription(Static.description)
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                                    .AddField("Танки:", Static.tank, false)
                                    .AddField("Хилы:", Static.heal, false)
                                    .AddField("Рдд:", Static.rdd, false)
                                    .AddField("Мдд:", Static.mdd, false)
                                    .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                            await discordClient.GetGuild(settings.DiscordMainChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());
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
                            updatestatic.AddMemberStaticRoster(name);

                            var builder = new EmbedBuilder()
                                    .WithThumbnailUrl("https://render.worldofwarcraft.com/eu/guild/crest/102/emblem-102-dfa55a-b1002e.jpg")
                                    .WithTitle("Состав рейд-статика")
                                    .WithDescription(Static.description)
                                    .WithColor(Discord.Color.DarkRed)
                                    .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                                    .AddField("Танки:", Static.tank, false)
                                    .AddField("Хилы:", Static.heal, false)
                                    .AddField("Рдд:", Static.rdd, false)
                                    .AddField("Мдд:", Static.mdd, false)
                                    .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                            await discordClient.GetGuild(settings.DiscordMainChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());
                            if (!msg.Channel.Name.Contains("@"))
                            {
                                await msg.DeleteAsync();
                            }

                            break;
                        }
                }
            return Task.CompletedTask;
        }

        public static string tokenWow;

        class Token_for_api
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string token_type { get; set; }
        }
        class UsersIdForTelegram
        {
            public List<User> members { get; set; }
        }

        class User
        {
            public string Title { get; set; }
            public string Name { get; set; }
            public string Id { get; set; }
        }
        public class BotSettings
        {
            public string DiscordBotToken { get; set; }
            public string TelegramBotToken { get; set; }
            public string BatNetToken { get; set; }
            public string BatNetSecretKey { get; set; }
            public string Realm { get; set; }
            public string RealmSlug { get; set; }

            public bool NeedPoolRT { get; set; }
            public bool AddtionalRT { get; set; }
            public string RealmStatusType { get; set; }
            public string Guild { get; set; }
            public ulong DiscordMainChatId { get; set; }

            public ulong DiscordMainChannelId { get; set; }
            public ulong DiscordActivityChannelId { get; set; }
            public ulong DiscordRosterChannelId { get; set; }
            public ulong DiscordLogChannelId { get; set; }
            public ulong DiscordAffixChannelId { get; set; }
            public ulong DiscordTestChatId { get; set; }
            public ulong TestDiscordMainChannelId { get; set; }
            public long TelegramMainChatID { get; set; }
            public long TelegramTestChatID { get; set; }
            public long LastGuildAchiveTime { get; set; }
            public long LastGuildActiveTime { get; set; }
            public long LastGuildLogTime { get; set; }
        }
        public class UserDiscord
        {
            public ulong ID { get; set; }
            public string Name { get; set; }
        }
        private static async void AutorizationsBattleNet()
        {


            try
            {

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.ConnectionClose = true;
                    using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), "https://eu.battle.net/oauth/token"))
                    {

                        string base64authorization = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{settings.BatNetToken}:{settings.BatNetSecretKey}"));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        request.Content = new StringContent("grant_type=client_credentials");
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        Token_for_api my_token = JsonConvert.DeserializeObject<Token_for_api>(response.Content.ReadAsStringAsync().Result);

                        tokenWow = my_token.access_token;

                        // Console.ForegroundColor = ConsoleColor.Green;
                        //     Console.WriteLine($"Battle.net Token success : {tokenWow}");
                        Functions.LoadRealmAll();



                    }

                }

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {

                    string message = $"Status Code : {((HttpWebResponse)e.Response).StatusCode}" +
                        $"\nStatus Description : {((HttpWebResponse)e.Response).StatusDescription}" +
                        $"\nAutorizationsBattleNet Error: {e.Message}";
                    Functions.WriteLogs(message, "error");
                }
            }
            catch (Exception e)
            {

                string message = $"AutorizationsBattleNet Error: {e.Message}";
                Functions.WriteLogs(message, "error");
            }



        }

    }
}

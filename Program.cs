using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Text;
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
using Discord.Commands;

namespace DiscordBot
{
    class Program
    {
        private static StringBuilder textAll = new();
        public static DiscordSocketClient discordClient;
        public static TelegramBotClient telegramClient;
        public static BotSettings settings;
        public static bool telegramBotNotification = false;
        public static string tokenWow;
        public static SocketGuild _mainChat;

        // private QueuedUpdateReceiver updateReceiver;
        //   [Obsolete]
        //static void Main(string[] args)
        //   => new Program().MainAsync().GetAwaiter().GetResult();

        // [Obsolete]
        static async Task Main(string[] args)
        {
            settings = new BotSettings();
            settings = Functions.ReadJson<BotSettings>("BotSettings");

            telegramBotNotification = false;
            
            tokenWow = await AutorizationBattleNet.OnTimerHandlerAutorizationBattleNet();
            Functions.LoadRealmAll();

            //GetEncounter.LoadEncounterAll();
            //GetInstance.LoadInstanceAll();
            settings.RealmSlug = Functions.GetRealmSlug(settings.Realm);
            textAll = new($"BattleNetAutorizationToken  : {tokenWow}\n" +
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
            Console.WriteLine(textAll.ToString());

            Functions.WriteJSon(settings, "BotSettings");

            discordClient = new DiscordSocketClient();
            discordClient.MessageReceived += DiscordTextCommands.CommandsHandler;
            discordClient.Log += Log;
            discordClient.Ready += SlashCommandHandler.Client_Ready;
            discordClient.ButtonExecuted += PoolRT.MyButtonHandler;
            discordClient.SlashCommandExecuted += SlashCommandHandler.CommandHandler;

            await discordClient.LoginAsync(TokenType.Bot, settings.DiscordBotToken);
            await discordClient.StartAsync();

            int num = 0;
            // TimerCallback tmAutoriz = new(AutorizationBattleNet.OnTimerHandlerAutorizationBattleNet);
            // Timer timerAutoriz = new(tmAutoriz, num, 3000, 1000 * 60 * 60);

            //  TimerCallback tmPoolRT = new(PoolRT.OnTimerHandlerPoolRT);
            //  Timer timerPoolRT = new(tmPoolRT, num, 0, 1000);

            TimerCallback tmCheckReboot = new(CheckReboot.OnTimerHandlerCheckReboot);
            Timer timerheckReboot = new(tmCheckReboot, num, 15000, 10000);

            TimerCallback tmCheckAffix = new(CheckAffix.OnTimerHandlerCheckAffix);
            Timer timerheckAffix = new(tmCheckAffix, num, 15000, 1000);

            TimerCallback tmactivity = new(CheckActivity.OnTimerHandlerCheckActivity);
            Timer timerActivity = new(tmactivity, num, 15000, 30000);

            TimerCallback tmlog = new(CheckLog.OnTimerHandlerCheckLog);
            Timer timerlog = new(tmlog, num, 15000, 15000);

            TimerCallback tmachieve = new(CheckAchievements.OnTimerHandlerCheckAchievements);
            Timer timerAchievements = new(tmachieve, num, 15000, 30000);

            TimerCallback tmroster = new(CheckRoster.OnTimerHandlerCheckRoster);
            Timer timerRoster = new(tmroster, num, 15000, 300000);

            TimerCallback tmsetRole = new(SetDiscordUserRole.OnTimerHandlerSetDiscordUserRole);
            Timer timerSetRole = new(tmsetRole, num, 10000, 60000 * 15);

            //TimerCallback tmUpdateStatic = new(UpdateStatic.OnTimerHandlerUpdateStatic);
            //Timer timerUpdateStatic = new(tmUpdateStatic, num, 10000, 60000 * 20);


            telegramClient = new TelegramBotClient(settings.TelegramBotToken);
            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            telegramClient.StartReceiving(
                TelegramTextCommands.HandleUpdateAsync,
                TelegramTextCommands.HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
               );

            var telebot = await telegramClient.GetMeAsync();
            Console.WriteLine($"\nTelegram Bot started @{telebot.Username}\n");
         
            Console.ReadLine();
            cts.Cancel();
        }

       

        private static Task Log(LogMessage msg)
        {

            Console.WriteLine(msg.ToString());



            return Task.CompletedTask;
        }






    }
}

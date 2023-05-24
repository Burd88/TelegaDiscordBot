using Discord;
using Discord.WebSocket;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;


namespace DiscordBot
{
    class Program
    {
        private static StringBuilder textAll = new();
        public static DiscordSocketClient discordClient;
        public static TelegramBotClient telegramClient;
        public static BotSettings settings;

        public static string tokenWow = null;
        public static SocketGuild _mainChat;


        static async Task Main(string[] args)
        {
            settings = new BotSettings();
            settings = Functions.ReadJson<BotSettings>("BotSettings");


            if (settings.BatNetSecretKey != "0" && settings.BatNetToken != "0")
            {
                tokenWow = await AutorizationBattleNet.OnTimerHandlerAutorizationBattleNet();

            }
            if (tokenWow != null)
            {
                Functions.LoadRealmAll();
                var encouter = GetEncounter.LoadEncounterAll();
                var instance = GetInstance.LoadInstanceAll();
                settings.RealmSlug = Functions.GetRealmSlug(settings.RealmName);
            }




            textAll = new($"BattleNetAutorizationToken  : {tokenWow}\n" +
                $"EnableCheckReboot : {settings.EnableCheckReboot}\n" +
                $"EnableCheckAffix : {settings.EnableCheckAffix}\n" +
                $"EnableCheckRoster : {settings.EnableCheckRoster}\n" +
                $"EnableCheckLastLog : {settings.EnableCheckLastLog}\n" +
                $"EnableCheckActivity : {settings.EnableCheckActivity}\n" +
                $"EnableCheckAchievements : {settings.EnableCheckAchievements}\n" +
                $"EnableCheckDiscordRole : {settings.EnableCheckDiscordRole}\n" +
                $"EnablePoolRT : {settings.EnablePoolRT}\n" +
                $"EnableAddtionalRT : {settings.EnableAddtionalRT}\n");
            Console.WriteLine(textAll.ToString());
            
            Functions.WriteJSon(settings, "BotSettings");
            if (settings.DiscordBotToken != "0")
            {
                var config = new DiscordSocketConfig()
                {

                    GatewayIntents = GatewayIntents.All
                };
                discordClient = new DiscordSocketClient(config);
                discordClient.MessageReceived += DiscordTextCommands.CommandsHandler;
                discordClient.Log += Log;
                discordClient.Ready += SlashCommandHandler.Client_Ready;
                discordClient.ButtonExecuted += PoolRT.MyButtonHandler;
                discordClient.SlashCommandExecuted += SlashCommandHandler.CommandHandler;

                await discordClient.LoginAsync(TokenType.Bot, settings.DiscordBotToken);
                await discordClient.StartAsync();
            }


            // TimerCallback tmAutoriz = new(AutorizationBattleNet.OnTimerHandlerAutorizationBattleNet);
            // Timer timerAutoriz = new(tmAutoriz, null, 3000, 1000 * 60 * 60);


            TimerCallback tmPoolRT = new(PoolRT.OnTimerHandlerPoolRT);
            Timer timerPoolRT = new(tmPoolRT, null, 0, 1000);


            TimerCallback tmCheckReboot = new(CheckReboot.OnTimerHandlerCheckReboot);
            Timer timerheckReboot = new(tmCheckReboot, null, 15000, 1000);


            TimerCallback tmCheckTokenWoW = new(CheckTokenWow.OnTimerHandlerCheckTokenWow);
            Timer timerheckTokenWoW = new(tmCheckTokenWoW, null, 15000, 600000);

            TimerCallback tmCheckAffix = new(CheckAffix.OnTimerHandlerCheckAffix);
            Timer timerheckAffix = new(tmCheckAffix, null, 15000, 1000);


            TimerCallback tmactivity = new(CheckActivity.OnTimerHandlerCheckActivity);
            Timer timerActivity = new(tmactivity, null, 15000, 30000);


            TimerCallback tmlog = new(CheckLog.OnTimerHandlerCheckLog);
            Timer timerlog = new(tmlog, null, 15000, 15000);

            TimerCallback tmachieve = new(CheckAchievements.OnTimerHandlerCheckAchievements);
            Timer timerAchievements = new(tmachieve, null, 15000, 30000);


            TimerCallback tmroster = new(CheckRoster.OnTimerHandlerCheckRoster);
            Timer timerRoster = new(tmroster, null, 15000, 300000);


            TimerCallback tmsetRole = new(SetDiscordUserRole.OnTimerHandlerSetDiscordUserRole);
            Timer timerSetRole = new(tmsetRole, null, 10000, 60000 * 15);



            //TimerCallback tmUpdateStatic = new(UpdateStatic.OnTimerHandlerUpdateStatic);
            //Timer timerUpdateStatic = new(tmUpdateStatic, null, 10000, 60000 * 20);


            TimerCallback tmevents = new(WarframeCheckEvents.OnTimerHandlerCheckWarframeEvents);
            Timer timerevents = new(tmevents, null, 10000, 35000);

            TimerCallback tmevinvas = new(WarframeCheckInvasions.OnTimerHandlerCheckWarframeInvasions);
            Timer timerinvasions = new(tmevinvas, null, 11000, 30000);

            TimerCallback tmevtrader = new(WarframeCheckVoidTrader.OnTimerHandlerCheckWarframeVoidTrader);
            Timer timertrader = new(tmevtrader, null, 15000, 25000);

            TimerCallback tmevNightWave = new(WarframeCheckNightWave.OnTimerHandlerCheckWarframeNightWave);
            Timer timerNightWave = new(tmevNightWave, null, 13000, 25000);

            if (settings.TelegramBotToken != "0")
            {
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
            }


            Console.ReadLine();
            
        }



        private static Task Log(LogMessage msg)
        {

            Console.WriteLine(msg.ToString());



            return Task.CompletedTask;
        }






    }
}

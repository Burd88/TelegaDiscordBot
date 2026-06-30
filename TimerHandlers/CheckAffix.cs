using Discord;
using System;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static DiscordBot.Program;

namespace DiscordBot
{
    class CheckAffix
    {
        public static async void OnTimerHandlerCheckAffix(object obj)
        {
            if (settings.EnableCheckAffix)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                {

                    if (DateTime.Now.Hour == 11)
                    {
                        if (DateTime.Now.Minute == 00)
                        {
                            if (DateTime.Now.Second == 00)
                            {

                                var affixs = await Functions.GetWebJson<MythicPlusAffixCurrent>("https://raider.io/api/v1/mythic-plus/affixes?region=eu&locale=ru");
                                if (affixs != null)
                                {
                                    try
                                    {
                                        var builder = new EmbedBuilder()
                                         .WithTitle($"**{affixs.title}**")
                                         .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                                         .WithDescription("Мифик+ аффиксы на эту неделю обновлены.")
                                         .AddField("(4-11)", $"**[{affixs.affix_details[0].name}]({affixs.affix_details[0].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[0].description}**")
                                         .AddField("(7+)", $"**[{affixs.affix_details[1].name}]({affixs.affix_details[1].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[1].description}**")
                                         .AddField("(10+)", $"**[{affixs.affix_details[2].name}]({affixs.affix_details[2].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[2].description}**")
                                         .AddField("(12+)", $"**[{affixs.affix_details[3].name}]({affixs.affix_details[3].wowhead_url.Replace("wowhead", "ru.wowhead")}): {affixs.affix_details[3].description}**");
                                        var emb = builder.Build();
                                        _mainChat = discordClient.GetGuild(settings.DiscordChatId);
                                        var chan = _mainChat.GetChannel(settings.DiscordAffixChannelId) as IMessageChannel;
                                        await chan.SendMessageAsync(null, false, emb);
                                     /*   string message = ($"Описание аффиксов на неделю отправлено!");
                                        Functions.WriteLogs(message, "notification");
                                        var text = "Мифик+ аффиксы на эту неделю обновлены.\n" +
                                         $"<b>(+2) {affixs.affix_details[0].name}</b>:\n {affixs.affix_details[0].description}\n" +
                                            $"<b>(+4) {affixs.affix_details[1].name}</b>:\n {affixs.affix_details[1].description}\n" +
                                           $"<b>(+7) {affixs.affix_details[2].name}</b>:\n {affixs.affix_details[2].description}\n" +
                                        $"<b>(+10) {affixs.affix_details[3].name}</b>:\n {affixs.affix_details[3].description}\n" +
                                        $"<b>(+12) Вероломство Ксал'атат</b>:\n Ксал'атат предает игроков и нарушает заключенные сделки, увеличивая запас здоровья и урон противников на 10%.\n";
                                        //if (settings.TelegramNotificationEnable)
                                        //   {
                                        await telegramClient.SendMessage(
                                          chatId: settings.TelegramTestChatID,
                                          text: text,
                                          parseMode: ParseMode.Html);
                                        //  }*/
                                    }
                                    catch (Exception ex)
                                    {
                                        Functions.WriteLogs(ex.Message, "error");
                                    }

                                }



                            }
                        }
                    }

                }
            }


        }
    }
}

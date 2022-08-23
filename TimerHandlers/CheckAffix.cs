using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using static DiscordBot.Program;

namespace DiscordBot 
{
    class CheckAffix
    {
        public static async void OnTimerHandlerCheckAffix(object obj)
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
                                      .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
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
                                if (telegramBotNotification)
                                {
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

        }
    }
}

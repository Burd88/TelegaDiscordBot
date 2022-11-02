using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordBot
{
    class AffixCommand
    {
        public static async Task HandleAffixCommand(SocketSlashCommand command)
        {

            var affixs = Functions.GetWebJson<MythicPlusAffixCurrent>("https://raider.io/api/v1/mythic-plus/affixes?region=us&locale=ru");
            if (affixs != null)
            {

                var builder = new EmbedBuilder()
                      .WithTitle($"**{affixs.title}**")
                      .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
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
    }
}

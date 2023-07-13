using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    class CharInfoCommand
    {
        public static async Task HandleCharCommand(SocketSlashCommand command)
        {
            CharInfo pers = new();



            var fullInfo = pers.GetCharInfo((string)command.Data.Options.First().Value);

            if (fullInfo != null)
            {
                //Console.WriteLine(fullInfo.ImageCharMainRaw);
                var builder = new EmbedBuilder()
                     .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                     .WithImageUrl(fullInfo.ImageCharMainRaw)
                     .WithTitle($"{fullInfo.Name}({fullInfo.Lvl} уровень) {fullInfo.Race}")
                     .WithUrl(fullInfo.LinkBnet).WithDescription($"Информация о персонаже  : \n{fullInfo.SetNameItem}")
                     .WithColor(Discord.Color.DarkRed).AddField("Уровень\nпредметов:", fullInfo.ILvl, true)

                     .AddField("Класс:", fullInfo.Class, true)
                     .AddField("Специализация:", fullInfo.Spec, true)
                     .AddField("Гильдия:", fullInfo.Guild, true)
                     //.AddField("Ковенант:", fullInfo.Coven, true)
                     //  .AddField("Медиум:", fullInfo.CovenSoul, true)
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
    }
}

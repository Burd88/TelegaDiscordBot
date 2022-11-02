using Discord;
using System;
using static DiscordBot.Program;

namespace DiscordBot
{
    class UpdateStatic
    {
        public static async void OnTimerHandlerUpdateStatic(object obj)
        {
            try
            {
                Static updatestatic = new();
                updatestatic.UpdateStaticRoster();

                var builder = new EmbedBuilder()
                        .WithThumbnailUrl("https://cdn.discordapp.com/avatars/931442555332198400/bb4f0a2c3f5534cc199b54cc6b805d1a.webp?size=100")
                        .WithTitle("Состав рейд-статика")
                        .WithDescription(Static.description)
                        .WithColor(Discord.Color.DarkRed)
                        .AddField("Средний Илвл статика:", Static.middleIlvl, false)
                        .AddField("Танки:", Static.tank, false)
                        .AddField("Хилы:", Static.heal, false)
                        .AddField("Рдд:", Static.rdd, false)
                        .AddField("Мдд", Static.mdd, false)
                        .WithFooter(footer => footer.Text = $"Гильдия \"Сердце греха\".\nОбновлено: {DateTime.Now} (+4 Мск) ");
                await discordClient.GetGuild(settings.DiscordChatId).GetTextChannel(944575829105594438).ModifyMessageAsync(944583210434719775, msg => msg.Embed = builder.Build());

            }
            catch (Exception e)
            {
                Functions.WriteLogs(e.Message, "error");

            }




        }
    }
}

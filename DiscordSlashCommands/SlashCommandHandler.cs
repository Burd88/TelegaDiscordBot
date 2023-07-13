using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot
{
    class SlashCommandHandler
    {
        public static async Task Client_Ready()
        {
            // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
            var guild = discordClient.GetGuild(settings.DiscordChatId);

            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var affix = new SlashCommandBuilder()
                .WithName("аффикс")
                .WithDescription("Отображает аффиксы мифик+ подземелий(EU)");

            var charcommand = new SlashCommandBuilder()
                .WithName("чар")
                .WithDescription("Информация о персонаже")
                .AddOption("имя", ApplicationCommandOptionType.String, "Введите имя персонажа. Для Ревущего Фьорда просто \"имя персонажа\".Для других миров \"имя-игровой мир\"", isRequired: true)
                ;

            try
            {

                await guild.CreateApplicationCommandAsync(affix.Build());
                await discordClient.Rest.CreateGuildCommand(charcommand.Build(), settings.DiscordChatId);
            }
            catch (CommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Data, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }
        }
        public static async Task CommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "аффикс":
                    await AffixCommand.HandleAffixCommand(command);
                    break;
                case "чар":
                    await CharInfoCommand.HandleCharCommand(command);
                    break;
            }
        }
    }
}

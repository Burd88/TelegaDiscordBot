using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Program;

namespace DiscordBot 
{
    class SetDiscordUserRole
    {
        private static List<RosterLeaveInv> rosterGuild;

        public static async void OnTimerHandlerSetDiscordUserRole(object obj)
        {
            try
            {
                //  Console.WriteLine("\n Начинаю сверять роли ВОВ-ДИСКОРД \n");
                var allUser = discordClient.GetGuild(settings.DiscordMainChatId).GetUsersAsync(RequestOptions.Default);

                rosterGuild = new();
                rosterGuild = Functions.ReadJson<List<RosterLeaveInv>>("Roster");



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
    }
}

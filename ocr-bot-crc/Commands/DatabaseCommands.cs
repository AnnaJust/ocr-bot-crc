using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ocr_bot_crc.Database.Services;
using ocr_bot_crc.Database.Models;

namespace ocr_bot.commands
{
    public class DatabaseCommands : BaseCommandModule
    {
        [Command("save")]
        public async Task SaveProfileCommand(CommandContext ctx, string language = "eng")
        {
            var DBEngine = new DBEngineService();

            var databaseUser = new DatabaseUser
            {
                UserName = ctx.User.Username,
                ServerName = ctx.Guild.Name,
                ServerId = ctx.Guild.Id,
                DefaultLanguage = language
            };

            var isStored = await DBEngine.SaveUserAsync(databaseUser);

            if (isStored)
            {
                await ctx.Channel.SendMessageAsync("Success");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Wrong");
            }
        }

        [Command("profile")]
        public async Task GetProfileCommand(CommandContext ctx)
        {
            var DBEngine = new DBEngineService();

            var userToFind = await DBEngine.GetUserAsync(ctx.User.Username, ctx.Guild.Id);

            if (userToFind.Item1 == true)
            {
                var profileEmbed = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Azure,
                    Title = $"{userToFind.Item2.UserName}'s Profile",
                    Description = $"Server Name: {userToFind.Item2.ServerName} \n" +
                                  $"Default language: {userToFind.Item2.DefaultLanguage}"
                };

                await ctx.Channel.SendMessageAsync(embed: profileEmbed);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Wrong");
            }
        }

        [Command("language")]
        public async Task ChangeLanguageCommand(CommandContext ctx, string language)
        {
            var DBEngine = new DBEngineService();

            var databaseUser = new DatabaseUser
            {
                UserName = ctx.User.Username,
                ServerName = ctx.Guild.Name,
                ServerId = ctx.Guild.Id,
                DefaultLanguage = language
            };

            var isStored = await DBEngine.UpdateUserAsync(databaseUser);

            if (isStored)
            {
                await ctx.Channel.SendMessageAsync("Success");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("Wrong");
            }
        }
    }
}

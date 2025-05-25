using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ocr_bot_crc.Database.Services;
using ocr_bot_crc.Database.Models;

namespace ocr_bot.commands
{
    public class DatabaseCommands : BaseCommandModule
    {
        [Command("save-profile")]
        [Description("Saves the user profile in database, a default language can be specified in the passed argument.")]
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
                var message = new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"User: {databaseUser.UserName} successfully saved in the database.",
                    Color = DiscordColor.Azure
                };

                await ctx.Channel.SendMessageAsync(embed: message);
            }
            else
            {
                var message = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = $"An error occurred while saving user: {databaseUser.UserName} in the database.",
                    Color = DiscordColor.DarkRed
                };

                await ctx.Channel.SendMessageAsync(embed: message);
            }
        }

        [Command("my-profile")]
        [Description("Displays saved user information.")]
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
                var message = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = $"User: {ctx.User.Username} profile cannot be found in the database.",
                    Color = DiscordColor.DarkRed
                };

                await ctx.Channel.SendMessageAsync(embed: message);
            }
        }

        [Command("user-profile")]
        [Description("Displays saved user information, the user name must be entered in the passed argument.")]
        public async Task GetProfileCommand(CommandContext ctx, string userName)
        {
            var DBEngine = new DBEngineService();

            var userToFind = await DBEngine.GetUserAsync(userName, ctx.Guild.Id);

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
                var message = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = $"User: {userName} profile cannot be found in the database.",
                    Color = DiscordColor.DarkRed
                };

                await ctx.Channel.SendMessageAsync(embed: message);
            }
        }

        [Command("set-language")]
        [Description("Setting a default language for the user, the language must be entered in the passed argument.")]
        public async Task SetLanguageCommand(CommandContext ctx, string language)
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
                var message = new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Default language changed to {language} for user {databaseUser.UserName}",
                    Color = DiscordColor.Azure
                };

                await ctx.Channel.SendMessageAsync(embed: message);
            }
            else
            {
                var message = new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = $"An error occurred when changing the default language for user {databaseUser.UserName}",
                    Color = DiscordColor.DarkRed
                };

                await ctx.Channel.SendMessageAsync(embed: message);
            }
        }
    }
}

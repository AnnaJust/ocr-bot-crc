using DSharpPlus;
using DSharpPlus.CommandsNext;
using ocr_bot.commands;

namespace ocr_bot
{
    public class Program
    {
        private static DiscordClient? Client { get; set; }
        private static CommandsNextExtension? Commands { get; set; }

        static async Task Main(string[] args)
        {
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { Environment.GetEnvironmentVariable("PREFIX") },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Client = new DiscordClient(discordConfig);
            Client.Ready += ClientReady;
            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<OcrCommands>();
            Commands.RegisterCommands<DatabaseCommands>();
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task ClientReady(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
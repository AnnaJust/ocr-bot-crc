using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ocr_bot_crc.Database.Models;
using ocr_bot_crc.Database.Services;
using System.Text.Json;

namespace ocr_bot.commands
{
    public class OcrCommands : BaseCommandModule
    {
        [Command("hello")]
        public async Task HelloCommand(CommandContext ctx)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = $"Hello, {ctx.User.Username}!",
                Description = "I am an OCR bot that can read text from the images you send me.",
                Color = DiscordColor.Azure
            };

            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("ocr")]
        public async Task OcrCommand(CommandContext ctx, string language = "")
        {
            if (ctx.Message.Attachments.Count == 0)
            {
                var message = new DiscordEmbedBuilder
                {
                    Title = "No image attached",
                    Description = "Make sure you include the image file in the chat.",
                    Color = DiscordColor.DarkRed
                };

                await ctx.Channel.SendMessageAsync(embed: message);
                return;
            }

            if (string.IsNullOrEmpty(language))
            {
                var DBEngine = new DBEngineService();

                var databaseUser = new DatabaseUser
                {
                    UserName = ctx.User.Username,
                    ServerName = ctx.Guild.Name,
                    ServerId = ctx.Guild.Id
                };

                language = await DBEngine.GetUserLanguage(databaseUser);
            }

            var attachment = ctx.Message.Attachments[0];

            try
            {
                using var httpClient = new HttpClient();
                var imageStream = await httpClient.GetStreamAsync(attachment.Url);
                var imageBytes = await new StreamContent(imageStream).ReadAsByteArrayAsync();

                using var form = new MultipartFormDataContent();
                form.Headers.ContentType.MediaType = "multipart/form-data";
                form.Add(new ByteArrayContent(imageBytes), "file", attachment.FileName);
                form.Add(new StringContent(Environment.GetEnvironmentVariable("API_KEY")), "apikey");
                form.Add(new StringContent(language), "language");
                form.Add(new StringContent("true"), "isOverlayRequired");

                var response = await httpClient.PostAsync("https://api.ocr.space/parse/image", form);
                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var text = root
                    .GetProperty("ParsedResults")[0]
                    .GetProperty("ParsedText")
                    .GetString();

                if (string.IsNullOrWhiteSpace(text))
                {
                    var message = new DiscordEmbedBuilder
                    {
                        Title = "Text not recognised",
                        Color = DiscordColor.DarkRed
                    };

                    await ctx.Channel.SendMessageAsync(embed: message);
                }
                else
                {
                    var message = new DiscordEmbedBuilder
                    {
                        Title = "Recognised text:",
                        Description = text,
                        Color = DiscordColor.Azure
                    };

                    await ctx.Channel.SendMessageAsync(embed: message);
                }
            }
            catch (Exception ex)
            {
                var message = new DiscordEmbedBuilder
                {
                    Title = "Text not recognised",
                    Color = DiscordColor.DarkRed
                };

                Console.WriteLine(ex);
                await ctx.Channel.SendMessageAsync(embed: message);
            }
        }
    }
}

using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Moq;
using ocr_bot.commands;
using ocr_bot_crc.Database.Services;
using System.Reflection;
using System.Runtime.Serialization;

namespace ocr_bot_crc.tests
{
    public class OcrCommandsTests
    {
        [Theory]
        [InlineData("Alice", "Hello, Alice!")]
        [InlineData("Bob", "Hello, Bob!")]
        public void BuildHelloEmbed_ReturnsCorrectTitle(string username, string expectedTitle)
        {
            // Act
            var embed = OcrCommands(username);

            // Assert
            Assert.Equal(expectedTitle, embed.Title);
            Assert.Contains("OCR bot", embed.Description);
            Assert.Equal(DiscordColor.Azure, embed.Color);
        }
    }
}

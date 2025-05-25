namespace ocr_bot_crc.Database.Models
{
    public class DatabaseUser
    {
        public string? UserName { get; set; }
        public string? ServerName { get; set; }
        public ulong ServerId { get; set; }
        public string? DefaultLanguage { get; set; }
    }
}

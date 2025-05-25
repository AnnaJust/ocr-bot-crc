using ocr_bot_crc.Database.Models;

namespace ocr_bot_crc.Database.Services
{
    public interface IDBEngineService
    {
        public Task<bool> SaveUserAsync(DatabaseUser user);
        public Task<(bool, DatabaseUser)> GetUserAsync(string userName, ulong serverId);
        public Task<bool> UpdateUserAsync(DatabaseUser user);
        public Task<string> GetUserLanguage(DatabaseUser databaseUser);
    }
}

using Npgsql;
using ocr_bot_crc.Database.Models;

namespace ocr_bot_crc.Database.Services
{
    public class DBEngineService
    {
        public async Task<bool> SaveUserAsync(DatabaseUser databaseUser)
        {
            var userNumber = await GetTotalUserAsync() + 1;
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "INSERT INTO data.userinfo (user_no, user_name, server_name, server_id, language) " +
                        $"VALUES ('{userNumber}', '{databaseUser.UserName}', '{databaseUser.ServerName}', '{databaseUser.ServerId}', '{databaseUser.DefaultLanguage}');";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<(bool, DatabaseUser)> GetUserAsync(string username, ulong serverId)
        {
            DatabaseUser databaseUser;
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT u.user_no, u.user_name, u.server_name, u.server_id, u.language " +
                                "FROM data.userinfo u " +
                                $"WHERE user_name = '{username}' AND server_id = {serverId}";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        var reader = await command.ExecuteReaderAsync();
                        await reader.ReadAsync();

                        databaseUser = new DatabaseUser
                        {
                            UserName = reader.GetString(1),
                            ServerName = reader.GetString(2),
                            ServerId = (ulong)reader.GetInt64(3),
                            DefaultLanguage = reader.GetString(4)
                        };
                    }
                }
                return (true, databaseUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (false, null);
            }
        }

        public async Task<bool> UpdateUserAsync(DatabaseUser databaseUser)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "UPDATE data.userinfo " +
                        $"SET language = '{databaseUser.DefaultLanguage}' " +
                        $"WHERE user_name = '{databaseUser.UserName}' AND server_id = {databaseUser.ServerId}";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<string> GetUserLanguage(DatabaseUser databaseUser)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT language FROM data.userinfo " +
                        $"WHERE user_name = '{databaseUser.UserName}' AND server_id = {databaseUser.ServerId}";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        var userLanguage = await command.ExecuteScalarAsync();
                        return userLanguage.ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        private async Task<long> GetTotalUserAsync()
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT COUNT(*) FROM data.userinfo;";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        var userCount = await command.ExecuteScalarAsync();
                        return Convert.ToInt64(userCount);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}

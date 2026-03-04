using Microsoft.Data.SqlClient;

namespace AdoDb
{
    internal class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository()
        {
            _connectionString = Configuration.GetConnectionString();
        }

        public async Task CreateAsync(User user)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = "INSERT INTO Users (Name, Email, Age) VALUES (@Name, @Email, @Age)";
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Age", user.Age);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();
            await using var connection = new SqlConnection(_connectionString);
            const string query = "SELECT Id, Name, Email, Age FROM Users";
            await using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Age = reader.GetInt32(reader.GetOrdinal("Age"))
                });
            }

            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = "SELECT Id, Name, Email, Age FROM Users WHERE Id = @Id";
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Age = reader.GetInt32(reader.GetOrdinal("Age"))
                };
            }

            return null;
        }

        public async Task UpdateAsync(User user)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = "UPDATE Users SET Name = @Name, Email = @Email, Age = @Age WHERE Id = @Id";
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Age", user.Age);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = "DELETE FROM Users WHERE Id = @Id";
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = "SELECT COUNT(1) FROM Users WHERE Id = @Id";
            await using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync();
            return (int)await command.ExecuteScalarAsync() > 0;
        }
    }
}
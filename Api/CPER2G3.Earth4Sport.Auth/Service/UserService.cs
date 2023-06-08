using CPER2G3.Earth4Sport.Auth.Models;
using Npgsql;

using Dapper;

namespace CPER2G3.Earth4Sport.Auth.Service {
    public class UserService : IUserService {
        private readonly string _connectionString;
        public UserService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("authdb");
        }
        public void Login(string username, string password) {
            throw new NotImplementedException();
        }

        public void Register(User user) {
            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString)) {
                conn.Open();
                conn.Execute("INSERT INTO users (username, password) VALUES (@Username, @Password);", new { Username = user.Username, Password = user.Password});
            }
            return;
        }
    }
}

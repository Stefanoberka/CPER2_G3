using CPER2G3.Earth4Sport.AzureFunction.Models;

using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace CPER2G3.Earth4Sport.AzureFunction.Service {
    public class UserService : IUserService {
        private readonly string _connectionString;
        private SHA256 _mySHA256 = SHA256.Create();
        public UserService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("authdb");
        }
        public async Task<int> Login(string username, string password) {
            var a = _mySHA256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var encodedPwd = BitConverter.ToString(a).Replace("-", String.Empty);

            var query = @"USE [authdb]
                SELECT [username]
                      ,[password]
                  FROM [dbo].[users]
                WHERE username = @username AND password = @encodedPwd 
            "; var conn = new SqlConnection(_connectionString);
            conn.Open();
            var ciao = await conn.QueryFirstOrDefaultAsync<User>(query, new { username, encodedPwd });
            if(ciao == null) {
                return 404;  
            }
            else {
                return 200;
            }
           
        }

        public async Task<string> Register(User user) {
            var a = _mySHA256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
            var encodedPwd = BitConverter.ToString(a).Replace("-", String.Empty);
            string username = user.Username;
            
            var query = @"USE [authdb]
                            INSERT INTO[dbo].[users]
                                        ([username],[password])
                        VALUES
                            (@username, @encodedPwd)
            "; var conn = new SqlConnection(_connectionString);
            conn.Open();
            try {
                await conn.ExecuteAsync(query, new { username, encodedPwd });
                return "ok";
            }
            catch(Exception e) {
                return "Errore";
            }
        }
    }
}

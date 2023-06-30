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
            var hash = _mySHA256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var encodedPwd = BitConverter.ToString(hash).Replace("-", String.Empty);
            var query = @"USE [authdb]
                SELECT [username]
                      ,[password]
                  FROM [dbo].[users]
                WHERE username = @username AND password = @encodedPwd 
            "; var conn = new SqlConnection(_connectionString);
            conn.Open();
            var res = await conn.QueryFirstOrDefaultAsync<User>(query, new { username, encodedPwd });
            if(res == null) {
                return 404;  
            }
            else {
                return 200;
            }
           
        }

        public async Task<string> Register(User user) {
            var hash = _mySHA256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
            var encodedPwd = BitConverter.ToString(hash).Replace("-", String.Empty);
            string username = user.Username;
            string uuid = Guid.NewGuid().ToString();
            string clock_uuid = user.ClockUuid;
            var query1 = @"USE [authdb]
                            INSERT INTO[dbo].[users]
                                        ([uuid],[username],[password])
                        VALUES
                            (@uuid, @username, @encodedPwd)";
            var query2 = @"USE [authdb]
                            INSERT INTO[dbo].[users_clocks]
                                        ([user_uuid],[clock_uuid])
                        VALUES
                            (@uuid, @clock_uuid)";
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            try {
                await conn.ExecuteAsync(query1, new {uuid, username, encodedPwd });
                await conn.ExecuteAsync(query2, new { uuid, clock_uuid });
                return "ok";
            }
            catch(Exception e) {
                throw e;
            }
        }
    }
}

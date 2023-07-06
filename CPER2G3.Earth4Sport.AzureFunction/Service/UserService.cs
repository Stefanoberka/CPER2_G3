using CPER2G3.Earth4Sport.AzureFunction.Models;

using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPER2G3.Earth4Sport.AzureFunction.Service {
    public class UserService : IUserService {
        private readonly string _connectionString;
        private SHA256 _mySHA256 = SHA256.Create();
        public UserService(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("authdb");
        }
        public async Task<LoginResponse> Login(string username, string password) {
            var hash = _mySHA256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var encodedPwd = BitConverter.ToString(hash).Replace("-", String.Empty);
            var query = @"USE [authdb]
                SELECT [uuid],
                        [username]
                      ,[password]
                  FROM [dbo].[users]
                WHERE username = @username AND password = @encodedPwd 
            "; 
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            var res = await conn.QueryFirstOrDefaultAsync<User>(query, new { username, encodedPwd });

            return new LoginResponse() {
                Authorized = (res != null),
                Uuid = res != null ? res.Uuid : ""
            };  

           
        }

        public async Task<string> Register(User user, string clock_Uuid) {
            var hash = _mySHA256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
            var encodedPwd = BitConverter.ToString(hash).Replace("-", String.Empty);
            string username = user.Username;
            user.Uuid = Guid.NewGuid().ToString();
            string uuid = user.Uuid;
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
                await conn.ExecuteAsync(query2, new { uuid, clock_Uuid });
                return "ok";
            }
            catch(Exception e) {
                throw e;
            }
        }

        public async Task<List<string>> UserClocks(string userId) {
            var query = @"USE [authdb]
                SELECT [clock_uuid]
                  FROM [dbo].[users_clocks]
                WHERE [user_uuid] = @userId
            ";
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            var res = await conn.QueryAsync<string>(query, new { userId });
            return res.ToList();
        }
    }
}

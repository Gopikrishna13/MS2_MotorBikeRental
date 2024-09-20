using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using Dapper;

namespace MotorBikeRental.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"Insert into Users (FirstName, LastName, UserName, Password, NIC, Email, LicenseNumber) 
                              Values (@FirstName, @LastName, @UserName, @Password, @NIC, @Email, @LicenseNumber)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@NIC", user.NIC);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@LicenseNumber", user.LicenseNumber);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();//Does not return output
                }
            }

            return user;
        }

        public async Task<bool> CheckUnique(string UserName, string Email, string LicenseNumber)
        {
            var query = "SELECT COUNT(1) FROM Users WHERE Email = @Email OR LicenseNumber = @LicenseNumber OR UserName = @UserName";
//Count will return 0 if no record match 1 or 2 if record match
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@LicenseNumber", LicenseNumber);
                command.Parameters.AddWithValue("@UserName", UserName);

                await connection.OpenAsync();
                var result = (int)await command.ExecuteScalarAsync();//An asynchronous version of ExecuteScalar(), which executes the command and returns the first column of the first row in the first returned result set.
                return result == 0;//if result=0 true else false
            }
        }

       public async  Task <List<User>> GetAllUsers()
        {
             var query=@"select * from Users";

             using(var connection=new SqlConnection(_connectionString))
             {
                var users = await connection.QueryAsync<User>(query); 
                return users.ToList();
             }
        }
    }
}

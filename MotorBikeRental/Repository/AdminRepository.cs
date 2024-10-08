using System;
using System.Data;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using MotorBikeRental.DTOs.RequestDTO;

namespace MotorBikeRental.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Admin> AddAdmin(Admin admin)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO Admin (FirstName, LastName, UserName, Password, NIC, Email) 
                    OUTPUT INSERTED.AdminId  
                    VALUES (@FirstName, @LastName, @UserName, @Password, @NIC, @Email);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", admin.FirstName);
                    command.Parameters.AddWithValue("@LastName", admin.LastName);
                    command.Parameters.AddWithValue("@UserName", admin.UserName);
                    command.Parameters.AddWithValue("@Password", admin.Password);
                    command.Parameters.AddWithValue("@NIC", admin.NIC);
                    command.Parameters.AddWithValue("@Email", admin.Email);

                    await connection.OpenAsync();
                    admin.AdminId = (int)await command.ExecuteScalarAsync(); // ExecuteScalar returns the inserted AdminId
                }
            }

            return admin;
        }
      public async Task<bool> Login(AdminLoginRequestDTO adminloginRequestDTO)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        var username = adminloginRequestDTO.UserName;
        var query = @"SELECT Password FROM Admin WHERE UserName = @username";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@username", username);
            await connection.OpenAsync();

            var password = await command.ExecuteScalarAsync();
            if (password != null)
            {
              //  var enc_password = encrypt_password(adminloginRequestDTO.Password); 
                return adminloginRequestDTO.Password == password.ToString(); 
              
        }
         return false; 
    }
}
}

    //    public string encrypt_password(string password)
    //    {
    //     var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(password);
    //     return Convert.ToBase64String(plainTextBytes);
    //    }

        public async Task<bool> CheckUnique(string UserName, string Email, string NIC)
        {
            var query = @"SELECT COUNT(1) FROM Admin WHERE Email = @Email OR UserName = @UserName OR NIC = @NIC";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@NIC", NIC);

                    await connection.OpenAsync();
                    var result = (int)await command.ExecuteScalarAsync();
                    return result == 0; // return true if no record is found, meaning it's unique
                }
            }
        }


//         public async Task <bool> checkUpdate(int Id,string UserName,string Email,string NIC)
//         {
//  var query = @"SELECT COUNT(1) 
// FROM Admin 
// WHERE (Email = @Email OR UserName = @UserName OR NIC = @NIC)
// AND AdminId != @Id
// ";

//             using (var connection = new SqlConnection(_connectionString))
//             {
//                 using (var command = new SqlCommand(query, connection))
//                 {
                   
//             command.Parameters.AddWithValue("@Email", Email);
//             command.Parameters.AddWithValue("@UserName", UserName);
//             command.Parameters.AddWithValue("@NIC", NIC);
//             command.Parameters.AddWithValue("@Id", Id);

//             await connection.OpenAsync();
//             var count = (int)await command.ExecuteScalarAsync();
//             return count == 0; // Return true if unique
//                 }
//             }
//         }

        public async Task<List<Admin>> GetAllAdmins()
        {
            var query = "SELECT * FROM Admin";
            var admins = new List<Admin>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var admin = new Admin
                            {
                                AdminId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                UserName = reader.GetString(3),
                                Password = reader.GetString(4),
                                NIC = reader.GetString(5),
                                Email = reader.GetString(6)
                            };
                            admins.Add(admin);
                        }
                    }
                }
            }

            return admins;
        }

        public async Task<Admin> GetAdminByusername(string username)
        {
            var query = "SELECT * FROM Admin WHERE UserName = @username";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Admin
                            {
                                AdminId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                UserName = reader.GetString(3),
                                Password = reader.GetString(4),
                                NIC = reader.GetString(5),
                                Email = reader.GetString(6)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task <Admin> UpdateAdmin (int Id, Admin admin)
        {
            var query = @"
                UPDATE Admin 
                SET 
                    FirstName = @FirstName,
                    LastName = @LastName,
                    UserName = @UserName,
                    Password = @Password,
                    NIC = @NIC,
                    Email = @Email
                WHERE AdminId = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", admin.FirstName);
                    command.Parameters.AddWithValue("@LastName", admin.LastName);
                    command.Parameters.AddWithValue("@UserName", admin.UserName);
                    command.Parameters.AddWithValue("@Password", admin.Password);
                    command.Parameters.AddWithValue("@NIC", admin.NIC);
                    command.Parameters.AddWithValue("@Email", admin.Email);
                    command.Parameters.AddWithValue("@Id", Id);

                    await connection.OpenAsync();
                    var affectedRows = await command.ExecuteNonQueryAsync();

                    return affectedRows > 0 ? admin : null;
                }
            }
        }

        public async Task<bool> DeleteAdmin(int Id)
        {
            var query = "DELETE FROM Admin WHERE AdminId = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error occurred: {ex.Message}");
                        return false;
                    }
                }
            }
        }
    }
}

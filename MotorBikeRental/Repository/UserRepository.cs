using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using MotorBikeRental.DTOs.RequestDTO;

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
       
        var query = @"
            INSERT INTO Users (FirstName, LastName, UserName, Password, NIC, Email, LicenseNumber) 
            OUTPUT INSERTED.UserId  
            VALUES (@FirstName, @LastName, @UserName, @Password, @NIC, @Email, @LicenseNumber);";
      
        var userId = await connection.ExecuteScalarAsync<int>(query, new
        {
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Password,
            user.NIC,
            user.Email,
            user.LicenseNumber
        });

       
        user.UserId = userId;
    }

    return user;
}


public async   Task<User> GetByusername(string username)
{
     var query = "SELECT * FROM Users WHERE UserName = @username";

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
                            return new User
                            {
                                UserId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                UserName = reader.GetString(3),
                                Password = reader.GetString(4),
                                NIC = reader.GetString(5),
                                Email = reader.GetString(6),
                                LicenseNumber=reader.GetString(7)
                            };
                        }
                        return null;
                    }
                }
            }

}
        public async Task<bool> CheckUnique(string UserName, string Email, string LicenseNumber)
        {
            var query = "SELECT COUNT(1) FROM Users WHERE Email = @Email OR LicenseNumber = @LicenseNumber OR UserName = @UserName";
//COUNT(1) returns the number of rows that match the condition. 
//If no rows match, COUNT(1) returns 0. If one or more rows match, it returns a number greater than 1
            using (var connection = new SqlConnection(_connectionString))
            {
               

              
                var result = (int)await connection.ExecuteScalarAsync(query,new{
                    UserName,
                    Email,
                    LicenseNumber
                });//asynchronous method that executes the query and returns a single value from the database.
                return result == 0;//if result=0 true else false
            }
        }
public async Task <bool> Login(UserLoginRequestDTO userloginRequestDTO)
{
    using (var connection = new SqlConnection(_connectionString))
    {
        var username = userloginRequestDTO.UserName;
        var query = @"SELECT Password FROM Users WHERE UserName = @username";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@username", username);
            await connection.OpenAsync();

            var password = await command.ExecuteScalarAsync();
            if (password != null)
            {
            
                return userloginRequestDTO.Password == password.ToString(); 
              
        }
         return false; 
    }
}

}
       public async  Task <List<User>> GetAllUsers()
        {
             var query=@"select * from Users";

             using(var connection=new SqlConnection(_connectionString))
             {
                var users = await connection.QueryAsync<User>(query); //Dapper
                return users.ToList();
             }
        }

      public async  Task<User> GetUserById(int Id)
      {
        var query=@"select * from Users where UserId=@Id";
        using(var connection=new SqlConnection(_connectionString))
        {
            var users=await connection.QueryFirstOrDefaultAsync<User>(query, new { Id });
            return users;
        }
      }


    public async Task<User> UpdateUser(int Id, User user)
{
    var query = @"
        UPDATE Users 
        SET 
            FirstName = @FirstName,
            LastName = @LastName,
            UserName = @UserName,
            Password = @Password,
            NIC = @NIC,
            Email = @Email,
            LicenseNumber = @LicenseNumber
        WHERE UserId = @Id";

    using (var connection = new SqlConnection(_connectionString))
    {
       
        var affectedRows = await connection.ExecuteAsync(query, new
        {
            Id,
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Password,
            user.NIC,
            user.Email,
            user.LicenseNumber
        });

   
        if (affectedRows == 0)
        {
            return null; 
        }

      
   

        return user;
    }
}



public async Task<bool> DeleteUser(int Id)
{

    var checkUser="Select Count(1) from RentalRequest where RentalRequest.UserId=@Id";


    var query=@"delete  from Users where UserId=@Id";

    using(var connection=new SqlConnection(_connectionString))
    {

       try
{
    var count_user = await connection.ExecuteScalarAsync<int>(checkUser, new { Id });
    if (count_user > 0)
    {
        return false; 
    }

    await connection.ExecuteAsync(query, new { Id });
    return true; 
}
catch (Exception ex)
{
    Console.WriteLine($"Error occurred: {ex.Message}");
    return false;
}

 

    }

    
      
}

public async Task <int> UserCount()
{
    var query=@"Select count(Users.UserId) from Users";

    using(var connection=new SqlConnection(_connectionString))
    {
        using(var command=new SqlCommand(query,connection))
        {
            await connection.OpenAsync();
            var result=await command.ExecuteScalarAsync();
            return (int)result;

        }
    }
}

    }
}



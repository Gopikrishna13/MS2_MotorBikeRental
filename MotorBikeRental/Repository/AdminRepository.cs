using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using Dapper;

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
      
        var userId = await connection.ExecuteScalarAsync<int>(query, new
        {
            admin.FirstName,
            admin.LastName,
            admin.UserName,
            admin.Password,
            admin.NIC,
            admin.Email,
           
        });

       
        admin.AdminId = userId;
    }

    return admin;
}
        public async Task<bool> CheckUnique(string UserName, string Email)
        {
            var query = "SELECT COUNT(1) FROM Admin WHERE Email = @Email  OR UserName = @UserName";
//COUNT(1) returns the number of rows that match the condition. 
//If no rows match, COUNT(1) returns 0. If one or more rows match, it returns a number greater than 1
            using (var connection = new SqlConnection(_connectionString))
            {
               

              
                var result = (int)await connection.ExecuteScalarAsync(query,new{
                    UserName,
                    Email
                });//asynchronous method that executes the query and returns a single value from the database.
                return result == 0;//if result=0 true else false
            }
        }

       public async  Task <List<Admin>> GetAllAdmins()
        {
             var query=@"select * from Admin";

             using(var connection=new SqlConnection(_connectionString))
             {
                var users = await connection.QueryAsync<Admin>(query); //Dapper
                return users.ToList();
             }
        }

      public async  Task<Admin> GetAdminById(int Id)
      {
        var query=@"select * from Admin where AdminId=@Id";
        using(var connection=new SqlConnection(_connectionString))
        {
            var users=await connection.QueryFirstOrDefaultAsync<Admin>(query, new { Id });
            return users;
        }
      }


    public async Task<Admin> UpdateAdmin(int Id,Admin admin)
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
       
        var affectedRows = await connection.ExecuteAsync(query, new
        {
            Id,
            admin.FirstName,
            admin.LastName,
            admin.UserName,
            admin.Password,
            admin.NIC,
            admin.Email
        });

   
        if (affectedRows == 0)
        {
            return null; 
        }

      
   

        return admin;
    }
}



public async Task<bool> DeleteAdmin(int Id)
{




    var query=@"delete  from Admin where AdminId=@Id";

    using(var connection=new SqlConnection(_connectionString))
    {

       try
{
   
    await connection.ExecuteAsync(query, new { Id });
    return true; 
}
catch (Exception ex)
{
    Console.WriteLine($"Error occurred: {ex.Message}");
    return false; // Indicate failure
}

 

    }

    
      
}

    }
}

using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using Dapper;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;


namespace MotorBikeRental.Repository
{
    public class RentalRepository:IRentalRepository
{
 private readonly string _connectionString;

        public RentalRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

public async Task <bool> RentalRequest(RentalRequest request)
{
    var query=@"Insert into RentalRequest (UserId,BikeId,RentedDate,ReturnDate,Status)
    values(@userId,@bikeId,@renteddate,@returndate,@status)";

    using(var connection=new SqlConnection(_connectionString))
    {
        using(var command=new SqlCommand(query,connection))
        {
            await connection.OpenAsync();

            command.Parameters.AddWithValue("@userId",request.UserId);
            command.Parameters.AddWithValue("@bikeId",request.BikeId);
            command.Parameters.AddWithValue("@renteddate",request.RentedDate);
            command.Parameters.AddWithValue("@returndate",request.ReturnDate);
            command.Parameters.AddWithValue("@status",request.Status);

         var result=await command.ExecuteNonQueryAsync();

         if(result < 0)
         {
            return false;
         }
        }
    }
    return true;
}

public async Task <List<RentalRequest>> AllRequest()
{
    var query=@"Select * from RentalRequest";
    var request=new List <RentalRequest>();

    using(var connection=new SqlConnection(_connectionString))
    {
        using(var command=new SqlCommand(query,connection))
        {
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    var data=new RentalRequest
                    {
                        RequestId=reader.GetInt32(0),
                        BikeId=reader.GetInt32(1),
                        UserId=reader.GetInt32(2),
                        RentedDate=reader.GetDateTime(3),
                        ReturnDate=reader.GetDateTime(4),
                        Status=reader.GetString(5)

                    };
                    request.Add(data);
                }
            }

           
        }
    }
    return request;
  
}

public async Task<bool> UpdateRequest(int code, int Id)
{
    string status = code == 1 ? "Accepted" : "Rejected";

   
    var updateQuery = @"Update RentalRequest
                        SET Status = @status 
                        WHERE RequestId = @Id";


    var selectQuery = @"Select BikeId, UserId, RentedDate, ReturnDate, Status 
                        from RentalRequest 
                        WHERE RequestId = @Id";


    var insertQuery = @"Insert into ReturnedBikes (BikeId, UserId, RentedDate, [To], Due, Status) 
                        VALUES (@BikeId, @UserId, @RentedDate, @To, @Due, @Status)";

    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();

       
        using (var updateCommand = new SqlCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@status", status);
            updateCommand.Parameters.AddWithValue("@Id", Id);
            await updateCommand.ExecuteNonQueryAsync();
        }

        if (code == 1) 
        {

            using (var selectCommand = new SqlCommand(selectQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@Id", Id);
                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    if (reader.Read()) 
                    {
                       
                        int bikeId = reader.GetInt32(0);
                        int userId = reader.GetInt32(1);
                        DateTime rentedDate = reader.GetDateTime(2);
                        DateTime returnDate = reader.GetDateTime(3); 
                        string rentalStatus = reader.GetString(4);

                       reader.Close();
                        using (var insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            DateTime today = DateTime.Today;
                            TimeSpan dueDifference = returnDate - today;


                           int dueDays = (int)dueDifference.TotalDays;
                            insertCommand.Parameters.AddWithValue("@BikeId", bikeId);
                            insertCommand.Parameters.AddWithValue("@UserId", userId);
                            insertCommand.Parameters.AddWithValue("@RentedDate", rentedDate);
                            insertCommand.Parameters.AddWithValue("@To", returnDate); 
                            insertCommand.Parameters.AddWithValue("@Due", dueDays); 
                            insertCommand.Parameters.AddWithValue("@Status", "Pending"); 

                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }
    }
    return true; 
}


}

}



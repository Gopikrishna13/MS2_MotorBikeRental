using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using Dapper;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;

namespace MotorBikeRental.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly string _connectionString;

        public RentalRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> RentalRequest(RentalRequest request)
        {
            var query = @"Insert into RentalRequest (UserId, BikeId, RegistrationNumber, RentedDate, ReturnDate, Status) 
                          values(@userId, @bikeId, @regno, @renteddate, @returndate, @status)";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@userId", request.UserId);
                    command.Parameters.AddWithValue("@bikeId", request.BikeId);
                    command.Parameters.AddWithValue("@regno", request.RegistrationNumber);
                    command.Parameters.AddWithValue("@renteddate", request.RentedDate);
                    command.Parameters.AddWithValue("@returndate", request.ReturnDate);
                    command.Parameters.AddWithValue("@status", request.Status);

                    var result = await command.ExecuteNonQueryAsync();
                    if (result < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<List<RentalRequest>> AllRequest()
        {
            var query = @"Select * from RentalRequest";
            var request = new List<RentalRequest>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var data = new RentalRequest
                            {
                                RequestId = reader.GetInt32(0),
                                BikeId = reader.GetInt32(1),
                                UserId = reader.GetInt32(2),
                                RegistrationNumber = reader.GetString(3),
                                RentedDate = reader.GetDateTime(4),
                                ReturnDate = reader.GetDateTime(5),
                                Status = reader.GetString(6)
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
            var updateQuery = @"Update RentalRequest SET Status = @status WHERE RequestId = @Id";
            var selectQuery = @"Select BikeId, UserId, RegistrationNumber, RentedDate, ReturnDate, Status from RentalRequest WHERE RequestId = @Id";
            var insertQuery = @"Insert into ReturnedBikes (BikeId, UserId, RegistrationNumber, RentedDate, [To], Due, Status) VALUES (@BikeId, @UserId, @regNo, @RentedDate, @To, @Due, @Status)";

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
                                string RegistrationNumber = reader.GetString(2);
                                DateTime rentedDate = reader.GetDateTime(3);
                                DateTime returnDate = reader.GetDateTime(4);
                                string rentalStatus = reader.GetString(5);
                                reader.Close();

                                using (var insertCommand = new SqlCommand(insertQuery, connection))
                                {
                                    DateTime today = DateTime.Today;
                                    TimeSpan dueDifference = returnDate - today;
                                    int dueDays = (int)dueDifference.TotalDays;

                                    insertCommand.Parameters.AddWithValue("@BikeId", bikeId);
                                    insertCommand.Parameters.AddWithValue("@UserId", userId);
                                    insertCommand.Parameters.AddWithValue("@regNo", RegistrationNumber);
                                    insertCommand.Parameters.AddWithValue("@RentedDate", rentedDate);
                                    insertCommand.Parameters.AddWithValue("@To", returnDate);
                                    insertCommand.Parameters.AddWithValue("@Due", dueDays);
                                    insertCommand.Parameters.AddWithValue("@Status", "Pending");

                                   
                                        int rowsInserted = await insertCommand.ExecuteNonQueryAsync();
                                        if (rowsInserted < 1)
                                        {
                                           
                                            return false;
                                        }
                                   
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
public async Task<List<ReturnedBikes>> AllReturnBike()
{
    var query = @"select * from ReturnedBikes";
    var request = new List<ReturnedBikes>();
    using (var connection = new SqlConnection(_connectionString))
    {
        using (var command = new SqlCommand(query, connection))
        {
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var data = new ReturnedBikes
                    {
                        ReturnId = reader.GetInt32(reader.GetOrdinal("ReturnId")),
                        BikeId = reader.GetInt32(reader.GetOrdinal("BikeId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        RegistrationNumber = reader.GetString(reader.GetOrdinal("RegistrationNumber")),
                        RentedDate = reader.GetDateTime(reader.GetOrdinal("RentedDate")) ,
                        To = reader.GetDateTime(reader.GetOrdinal("To")) ,
                        Due = reader.GetDateTime(reader.GetOrdinal("Due")) ,
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                    };
                    request.Add(data);
                }
            }
        }
    }
    return request;
}


        public async Task<bool> UpdateReturn(int Id)
        {
            var query = @"Update ReturnedBikes set Status=@status where ReturnId=@id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@status", "Returned");
                    command.Parameters.AddWithValue("@id", Id);
                    var result = await command.ExecuteNonQueryAsync();
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public async Task<List<ReturnedBikes>> PendingByUser(int Id)
{
    var query = @"
    SELECT 
        Bikes.BikeId, 
        Bikes.Model, 
        Bikes.Brand, 
        ReturnedBikes.RegistrationNumber, 
        ReturnedBikes.RentedDate, 
        ReturnedBikes.[To], 
        ReturnedBikes.Due 
    FROM 
        Bikes 
    JOIN 
        ReturnedBikes ON Bikes.BikeId = ReturnedBikes.BikeId 
    WHERE 
        ReturnedBikes.UserId = @Id AND 
        ReturnedBikes.Status = 'Pending'";

    var request = new List<ReturnedBikes>();

    using (var connection = new SqlConnection(_connectionString))
    {
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", Id);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var data = new ReturnedBikes
                    {
                        BikeId = reader.GetInt32(reader.GetOrdinal("BikeId")),
                        RegistrationNumber = reader.GetString(reader.GetOrdinal("RegistrationNumber")),
                        RentedDate = reader.GetDateTime(reader.GetOrdinal("RentedDate")) ,
                        To = reader.GetDateTime(reader.GetOrdinal("To")) ,
                        Due = reader.GetDateTime(reader.GetOrdinal("Due")) 
                   
                    };
                    request.Add(data);
                }
            }
        }
    }
    return request;
}

public async Task <List<ReturnedBikes>> ReturnByUser(int Id)
{
        var query = @"
    SELECT 
        Bikes.BikeId, 
        Bikes.Model, 
        Bikes.Brand, 
        ReturnedBikes.RegistrationNumber, 
        ReturnedBikes.RentedDate, 
        ReturnedBikes.[To], 
        ReturnedBikes.Due 
    FROM 
        Bikes 
    JOIN 
        ReturnedBikes ON Bikes.BikeId = ReturnedBikes.BikeId 
    WHERE 
        ReturnedBikes.UserId = @Id AND 
        ReturnedBikes.Status = 'Returned'";

    var request = new List<ReturnedBikes>();

    using (var connection = new SqlConnection(_connectionString))
    {
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", Id);

            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var data = new ReturnedBikes
                    {
                        BikeId = reader.GetInt32(reader.GetOrdinal("BikeId")),
                        RegistrationNumber = reader.GetString(reader.GetOrdinal("RegistrationNumber")),
                        RentedDate = reader.GetDateTime(reader.GetOrdinal("RentedDate")) ,
                        To = reader.GetDateTime(reader.GetOrdinal("To")) ,
                        Due = reader.GetDateTime(reader.GetOrdinal("Due")) 
                   
                    };
                    request.Add(data);
                }
            }
        }
    }
    return request;

}

public async Task<decimal> Revenue()
{
    var query = @"SELECT SUM(Bikes.Rent)
                  FROM Bikes
                  JOIN ReturnedBikes ON Bikes.BikeId = ReturnedBikes.BikeId
                  WHERE ReturnedBikes.Status = 'Returned'";
    using (var connection = new SqlConnection(_connectionString))
    {
        using (var command = new SqlCommand(query, connection))
        {
            await connection.OpenAsync();
            var count = await command.ExecuteScalarAsync();
            return  (decimal)count;
        }
    }
}

public async Task<bool> CheckAvailability(string registrationNumber, DateTime reqdate, DateTime retdate)
{
    var query = @"
    SELECT COUNT(1)
    FROM ReturnedBikes WHERE
        ReturnedBikes.RegistrationNumber = @registrationNumber
        AND (
            (ReturnedBikes.RentedDate <= @retdate AND ReturnedBikes.[To] >= @reqdate)
          AND  ReturnedBikes.Status='Pending'
        )";

    using (var connection = new SqlConnection(_connectionString))
    {
        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@registrationNumber", registrationNumber);
            command.Parameters.AddWithValue("@reqdate", reqdate);
            command.Parameters.AddWithValue("@retdate", retdate);

            await connection.OpenAsync();
            var count = await command.ExecuteScalarAsync();
            return (int)count == 0;
        }
    }
}


    }
}

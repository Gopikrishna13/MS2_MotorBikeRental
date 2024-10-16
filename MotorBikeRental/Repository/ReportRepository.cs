using System;
using MotorBikeRental.IRepository;
using MotorBikeRental.Database.Entities;
using Microsoft.Data.SqlClient;
namespace MotorBikeRental.Repository
{
public class ReportRepository:IReportRepository
{

    private readonly string _connectionString;

        public ReportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
public async Task<List<CustomerRentalReport>> GetcustomerReport()
{
    var query = @"
    SELECT 
        Users.UserId,
        Users.UserName,
        ReturnedBikes.BikeId,
        ReturnedBikes.RentedDate,
        ReturnedBikes.[To],
        ReturnedBikes.Due,
        BikeUnits.RegistrationNumber
    FROM ReturnedBikes
    JOIN Users ON Users.UserId = ReturnedBikes.UserId
    JOIN BikeUnits ON BikeUnits.RegistrationNumber = ReturnedBikes.RegistrationNumber"; 

    var rentalReport = new List<CustomerRentalReport>();

    using (var connection = new SqlConnection(_connectionString))
    {
        using (var command = new SqlCommand(query, connection))
        {
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                    string userName = reader.GetString(reader.GetOrdinal("UserName"));
                    int bikeId = reader.GetInt32(reader.GetOrdinal("BikeId"));
                    string registrationNumber = reader.GetString(reader.GetOrdinal("RegistrationNumber"));
                    DateTime rentedDate = reader.GetDateTime(reader.GetOrdinal("RentedDate"));
                    DateTime? to = reader.GetDateTime(reader.GetOrdinal("To"));
                    DateTime due = reader.GetDateTime(reader.GetOrdinal("Due"));

                    var customerReport = new CustomerRentalReport
                    {
                        UserId = userId,
                        UserName = userName,
                        RentalHistories = new List<RentalHistory>()
                    };

                    customerReport.RentalHistories.Add(new RentalHistory
                    {
                        BikeId = bikeId,
                        RegistrationNumber = registrationNumber,
                        RentedDate = rentedDate,
                        To = to,
                        Due = due
                    });

                    rentalReport.Add(customerReport);
                }
            }
        }
    }

    return rentalReport;
}


public async  Task <List<BikeInventory>> GetInventoryReport()
{
    var report=new List<BikeInventory>();
    var query=@"Select Bikes.BikeId,Bikes.Model,Bikes.Brand,
    BikeUnits.RegistrationNumber,ReturnedBikes.Status
    from Bikes 
    join BikeUnits on Bikes.BikeId=BikeUnits.BikeId
    join ReturnedBikes on  BikeUnits.RegistrationNumber=ReturnedBikes.RegistrationNumber";

    using(var connection=new SqlConnection(_connectionString))
    {
        using(var command=new SqlCommand(query,connection))
        {
            await connection.OpenAsync();
            using(var reader=await command.ExecuteReaderAsync())
            {
                while(await reader.ReadAsync())
                {
                    
                        int BikeId=reader.GetInt32(reader.GetOrdinal("BikeId"));
                        string Brand=reader.GetString(reader.GetOrdinal("Brand"));
                        string Model=reader.GetString(reader.GetOrdinal("Model"));
                        string RegistrationNumber=reader.GetString(reader.GetOrdinal("RegistrationNumber"));
                        string Status=reader.GetString(reader.GetOrdinal("Status"));

                    var data=new BikeInventory
                    {
                        BikeId=BikeId,
                        Brand=Brand,
                        Model=Model,
                        RegistrationNumber=RegistrationNumber,
                        Status=Status

                    };

                    report.Add(data);

                }
            }
        }
    }
    return report;
}

public async  Task  <List<AllBikes>> FrequentRent()
{

    var report=new List <AllBikes>();
    var query = @"
    SELECT 
        Bikes.BikeId, Bikes.Model, Bikes.Brand, BikeUnits.RegistrationNumber,
        COUNT(ReturnedBikes.BikeId) AS RentCount FROM 
        Bikes JOIN 
        ReturnedBikes  ON Bikes.BikeId = ReturnedBikes.BikeId
    JOIN
        BikeUnits ON BikeUnits.RegistrationNumber=ReturnedBikes.RegistrationNumber
    GROUP BY Bikes.BikeId, Bikes.Model, Bikes.Brand,BikeUnits.RegistrationNumber
    ORDER BY RentCount DESC";

using(var connection=new SqlConnection(_connectionString))
{
    using(var command=new SqlCommand(query,connection))
    {
        await connection.OpenAsync();

        using(var reader=await command.ExecuteReaderAsync())
        {
            while(await reader.ReadAsync())
            {
                        int BikeId=reader.GetInt32(reader.GetOrdinal("BikeId"));
                        string Brand=reader.GetString(reader.GetOrdinal("Brand"));
                        string Model=reader.GetString(reader.GetOrdinal("Model"));
                        string RegistrationNumber=reader.GetString(reader.GetOrdinal("RegistrationNumber"));

                          var data=new AllBikes
                    {
                        BikeId=BikeId,
                        Brand=Brand,
                        Model=Model,
                        RegistrationNumber=RegistrationNumber
                       

                    };
                    report.Add(data);
            }
        }
    }
}
return report;
}

}



}



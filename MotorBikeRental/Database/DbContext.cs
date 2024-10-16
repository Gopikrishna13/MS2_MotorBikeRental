using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;

namespace MotorBikeRental.Database
{
    public class DbContext
    {
        private readonly string _connectionString;

        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTables()
        {
            CreateUser();
            CreateAdmin();
             CreateBike();
           RentalRequest();
           ReturnedBikes();
            BikeUnits();
            BikeImages();
            RentalHistory();
            RentalSample();
        }

public void RentalSample()
{
    var tablequery=@"IF NOT EXISTS(SELECT * FROM sys.tables WHERE name='RentalSample')
    BEGIN 
         CREATE TABLE RentalSample(
         SampleId int primary key identity(1,1),
         BikeId int,
         Foreign Key(BikeId ) references Bikes (BikeId)
         );
         END";
         ExecuteCommand(tablequery);
}
        public void CreateUser()
        {
            var tableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
            BEGIN
                CREATE TABLE Users(
                    UserId INT PRIMARY KEY IDENTITY(1,1),
                    FirstName NVARCHAR(50) NOT NULL,
                    LastName NVARCHAR(50) NOT NULL,
                    UserName NVARCHAR(50) NOT NULL UNIQUE,
                    Password NVARCHAR(20) NOT NULL,
                    NIC NVARCHAR(20) UNIQUE NOT NULL,
                    Email NVARCHAR(50) UNIQUE NOT NULL,
                    LicenseNumber NVARCHAR(50) UNIQUE NOT NULL
                );
                PRINT 'Table Users created successfully.';
            END";

            ExecuteCommand(tableQuery);
        }

    public void RentalHistory()
{
    var tablequery = @"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RentalHistory')
        BEGIN 
            CREATE TABLE RentalHistory (
                RentalHistoryId INT PRIMARY KEY IDENTITY(1,1), 
                CustomerId INT NOT NULL,                       
                BikeId INT NOT NULL,                            
                FromDate DATETIME NOT NULL,                    
                ToDate DATETIME,                               
                Status INT NOT NULL,                            
                FOREIGN KEY (CustomerId) REFERENCES Users(UserId),
                FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId)
            );
            PRINT 'Table RentalHistory created successfully.';
        END";

    ExecuteCommand(tablequery);
}

        public void CreateAdmin()
        {
            var tableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Admin')
            BEGIN
                CREATE TABLE Admin(
                    AdminId INT PRIMARY KEY IDENTITY(1,1),
                    FirstName NVARCHAR(50) NOT NULL,
                    LastName NVARCHAR(50) NOT NULL,
                    UserName NVARCHAR(50) NOT NULL UNIQUE,
                    Password NVARCHAR(20) NOT NULL,
                    NIC NVARCHAR(20) UNIQUE NOT NULL,
                    Email NVARCHAR(50) UNIQUE NOT NULL
                );
                PRINT 'Table Admin created successfully.';
            END";

            ExecuteCommand(tableQuery);
        }

        public void CreateBike()
        {
            var tableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Bikes')
            BEGIN
                CREATE TABLE Bikes(
                  BikeId INT PRIMARY KEY IDENTITY(1,1),
                  Model NVARCHAR(100) NOT NULL,         
                  Brand NVARCHAR(100) NOT NULL,         
                  Rent DECIMAL(10, 2) NOT NULL 
                );
                PRINT 'Table Bikes created successfully.';
            END";

            ExecuteCommand(tableQuery);
        }

        public void RentalRequest()
        {
            var tableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RentalRequest')
            BEGIN
                CREATE TABLE RentalRequest(
                    RequestId INT PRIMARY KEY IDENTITY(1,1),
                    BikeId INT NOT NULL,
                    UserId INT NOT NULL,
                    RegistrationNumber nvarchar(50),
                    RentedDate DATETIME NOT NULL,
                    ReturnDate DATETIME NOT NULL,
                    Status nvarchar(50),
                    FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId),
                    FOREIGN KEY (UserId) REFERENCES Users(UserId)
                );
                PRINT 'Table RentalRequest created successfully.';
            END";

            ExecuteCommand(tableQuery);
        }

        public void ReturnedBikes()
        {
            var tableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ReturnedBikes')
            BEGIN
                CREATE TABLE ReturnedBikes(
                    ReturnId INT PRIMARY KEY IDENTITY(1,1),
                    BikeId INT NOT NULL,
                    UserId INT NOT NULL,
                     RegistrationNumber nvarchar(50),
                    RentedDate DATETIME NOT NULL,
                    [To] DATETIME ,
                    Due DATETIME ,
                    Status nvarchar(50),

                 
                    FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId),
                    FOREIGN KEY (UserId) REFERENCES Users(UserId)
                );
                PRINT 'Table ReturnedBikes created successfully.';
            END";

            ExecuteCommand(tableQuery);
        }

      public void BikeUnits()
{
    var tableQuery = @"
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BikeUnits')
    BEGIN
        CREATE TABLE BikeUnits(
            UnitId INT PRIMARY KEY IDENTITY(1,1),
            BikeId INT NOT NULL,
            RegistrationNumber NVARCHAR(255),
            Year INT NOT NULL,
            Status INT NOT NULL,
            FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId)
        );
        PRINT 'Table BikeUnits created successfully.';
    END";
    
    ExecuteCommand(tableQuery);
}


public void BikeImages()
{
    var tableQuery = @"
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BikeImages')
    BEGIN
        CREATE TABLE BikeImages(
            ImageId INT PRIMARY KEY IDENTITY(1,1),
            BikeId INT NOT NULL,
            UnitId INT NOT NULL,
            ImagePath NVARCHAR(MAX),
            FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId),
            FOREIGN KEY (UnitId) REFERENCES BikeUnits(UnitId)
        );
        PRINT 'Table BikeImages created successfully.';
    END";
    
    ExecuteCommand(tableQuery);
}

        private void ExecuteCommand(string command)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand(command, connection))
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

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
            BikeImages();
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
                    BikeName NVARCHAR(50) NOT NULL,
                    Rent INT NOT NULL,
                    RegNo NVARCHAR(20) UNIQUE NOT NULL,
                    Status INT DEFAULT 0
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
                    RentedDate DATETIME NOT NULL,
                    ReturnDate DATETIME NOT NULL,
                    Due DATETIME,
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
                    RentedDate DATETIME NOT NULL,
                    ReturnDate DATETIME NOT NULL,
                    Due DATETIME,
                    FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId),
                    FOREIGN KEY (UserId) REFERENCES Users(UserId)
                );
                PRINT 'Table ReturnedBikes created successfully.';
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
                    ImagePath NVARCHAR(255),
                    FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId)
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

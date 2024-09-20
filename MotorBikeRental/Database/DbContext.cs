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
            CREATE TABLE Users(
                UserId INT PRIMARY KEY IDENTITY(1,1),
                FirstName NVARCHAR(50) NOT NULL,
                LastName NVARCHAR(50) NOT NULL,
                UserName NVARCHAR(50) NOT NULL UNIQUE,
                Password NVARCHAR(20) NOT NULL,
                NIC NVARCHAR(20) UNIQUE NOT NULL,
                Email NVARCHAR(50) UNIQUE NOT NULL,
                LicenseNumber NVARCHAR(50) UNIQUE NOT NULL
            );";

            ExecuteCommand(tableQuery);
            Console.WriteLine("Table 'Users' created successfully.");
        }

        public void CreateAdmin()
        {
            var tableQuery = @"
            CREATE TABLE Admin(
                AdminId INT PRIMARY KEY IDENTITY(1,1),
                FirstName NVARCHAR(50) NOT NULL,
                LastName NVARCHAR(50) NOT NULL,
                UserName NVARCHAR(50) NOT NULL UNIQUE,
                Password NVARCHAR(20) NOT NULL,
                NIC NVARCHAR(20) UNIQUE NOT NULL,
                Email NVARCHAR(50) UNIQUE NOT NULL
            );";

            ExecuteCommand(tableQuery);
            Console.WriteLine("Table 'Admin' created successfully.");
        }

        public void CreateBike()
        {
            var tableQuery = @"
            CREATE TABLE Bikes(
                BikeId INT PRIMARY KEY IDENTITY(1,1),
                BikeName NVARCHAR(50) NOT NULL,
                Rent int NOT NULL,
                
                RegNo varchar(20) UNIQUE NOT NULL,
                Status int DEFAULT 0
            );";

            ExecuteCommand(tableQuery);
            Console.WriteLine("Table 'Bikes' created successfully.");
        }

        public void RentalRequest()
        {
            var tableQuery = @"
            CREATE TABLE RentalRequest(
                RequestId INT PRIMARY KEY IDENTITY(1,1),
                BikeId INT NOT NULL,
                UserId INT NOT NULL,
                RentedDate DATETIME NOT NULL,
                ReturnDate DATETIME NOT NULL,
                Due DATETIME,
                FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId),
                FOREIGN KEY (UserId) REFERENCES Users(UserId)
            );";

            ExecuteCommand(tableQuery);
            Console.WriteLine("Table 'RentalRequest' created successfully.");
        }

        public void ReturnedBikes()
        {
            var tableQuery = @"
            CREATE TABLE ReturnedBikes(
                ReturnId INT PRIMARY KEY IDENTITY(1,1),
                BikeId INT NOT NULL,
                UserId INT NOT NULL,
                RentedDate DATETIME NOT NULL,
                ReturnDate DATETIME NOT NULL,
                Due DATETIME,
                FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId),
                FOREIGN KEY (UserId) REFERENCES Users(UserId)
            );";

            ExecuteCommand(tableQuery);
            Console.WriteLine("Table 'ReturnedBikes' created successfully.");
        }


        public void BikeImages()
        {
            var tableQuery=@"  CREATE TABLE BikeImages(
                ImageId INT PRIMARY KEY IDENTITY(1,1),
                BikeId INT NOT NULL,
                ImagePath NVARCHAR(255) ,
             
                FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId),
                
            );";
             ExecuteCommand(tableQuery);
            Console.WriteLine("Table 'BikeImages' created successfully.");
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

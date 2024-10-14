using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using Dapper;
using MotorBikeRental.DTOs.ResponseDTO;
namespace MotorBikeRental.Repository
{
    public class BikeRepository:IBikeRepository
{

    private readonly string _connectionString;

    public BikeRepository(string connectionString )
    {
        _connectionString=connectionString;
    }

    public async Task <bool> CheckUnique(string regNo)
    {

        var query=@"select count(1) from BikeUnits where RegistrationNumber=@RegNo";

        using(var connection=new SqlConnection(_connectionString))
        {

            var result=(int) await connection.ExecuteScalarAsync(query,new{

                RegNo=regNo

            });

            return result==0;

        }
    
    }


 public async Task<bool> AddBike(Bike bike)
{
    try
    {
        var query = @"INSERT INTO Bikes (Model, Brand, Rent) 
                      OUTPUT INSERTED.BikeId  
                      VALUES (@model, @brand, @rent)";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Insert the bike
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@model", bike.Model);
                command.Parameters.AddWithValue("@brand", bike.Brand);
                command.Parameters.AddWithValue("@rent", bike.Rent);

                bike.BikeId = (int)await command.ExecuteScalarAsync();

                // Loop through each unit
                foreach (var units in bike.Units)
                {
                    var bikeUnitQuery = @"
                        INSERT INTO BikeUnits (BikeId, RegistrationNumber, Year, Status)
                        OUTPUT INSERTED.UnitId
                        VALUES (@BikeId, @RegistrationNumber, @Year, @Status)";
                    
                    int bikeUnitId;
                    using (var command_1 = new SqlCommand(bikeUnitQuery, connection))
                    {
                        command_1.Parameters.AddWithValue("@BikeId", bike.BikeId);
                        command_1.Parameters.AddWithValue("@RegistrationNumber", units.RegistrationNumber);
                        command_1.Parameters.AddWithValue("@Year", units.Year);
                        command_1.Parameters.AddWithValue("@Status", units.Status);

                        bikeUnitId = (int)await command_1.ExecuteScalarAsync();
                    }

                
                    foreach (var image in units.Images)
                    {
                        var bikeImageQuery = @"
                            INSERT INTO BikeImages (BikeId, UnitId, ImagePath) 
                            VALUES (@BikeId, @UnitId, @Path)";
                        
                        using (var img_command = new SqlCommand(bikeImageQuery, connection))
                        {
                            img_command.Parameters.AddWithValue("@BikeId", bike.BikeId);
                            img_command.Parameters.AddWithValue("@UnitId", bikeUnitId); 
                            img_command.Parameters.AddWithValue("@Path", image.ImagePath);
                            
                         
                            await img_command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        return true;
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}


public async Task<List<BikeResponseDTO>> GetAllBikes()
{
    var query = @"
        SELECT 
            Bikes.BikeId, 
            Bikes.Model, 
            Bikes.Brand, 
            Bikes.Rent, 
            BikeUnits.RegistrationNumber, 
            STRING_AGG(BikeImages.ImagePath, ',') AS ImagePaths
        FROM Bikes
        JOIN BikeUnits ON Bikes.BikeId = BikeUnits.BikeId
        LEFT JOIN BikeImages ON BikeUnits.UnitId = BikeImages.UnitId
        GROUP BY 
            Bikes.BikeId, 
            Bikes.Model, 
            Bikes.Brand, 
            Bikes.Rent, 
            BikeUnits.RegistrationNumber";

    using (var connection = new SqlConnection(_connectionString))
    {
        // Query the database and manually map the results
        var result = await connection.QueryAsync(query);

        var bikes = new List<BikeResponseDTO>();

        foreach (var row in result)
        {
            var bike = new BikeResponseDTO
            {
                BikeId = row.BikeId,
                Model = row.Model,
                Brand = row.Brand,
                Rent = row.Rent,
                RegistrationNumber = row.RegistrationNumber,
                Images = row.ImagePaths != null
                    ? ((string)row.ImagePaths).Split(',').Select(path => new BikeImageResponseDTO
                    {
                        ImagePath = path
                    }).ToList()
                    : new List<BikeImageResponseDTO>()
            };

            bikes.Add(bike);
        }

        return bikes;
    }
}




//   public async Task<bool> DeleteBike(int id)
// {
//     var chkBikeQuery = @"SELECT COUNT(1) FROM RentalRequest WHERE BikeId = @Id";
//     var deleteQuery = @"DELETE FROM Bikes WHERE BikeId = @Id";

//     using (var connection = new SqlConnection(_connectionString))
//     {
       
//         var rentalCount = await connection.ExecuteScalarAsync<int>(chkBikeQuery, new { Id=id });
        
//         if (rentalCount > 0)
//         {
         
//             return false;
//         }

      
//         var deletedRows = await connection.ExecuteAsync(deleteQuery, new { Id=id  });
        
      
//         return deletedRows > 0;
//     }
// }


// public async Task <BikeImages> AddImages(BikeImages imageRequest)
// {

//     var query=@"insert into BikeImages (BikeId,ImagePath) 
//     output inserted.ImageId
//     values(@BikeId,@ImagePath)";

//     using(var connection=new SqlConnection(_connectionString))
//     {
//         var result=await connection.ExecuteScalarAsync<int>(query,new{
//            BikeId= imageRequest.BikeId,
//            ImagePath=imageRequest.ImagePath

//         });

//         imageRequest.ImageId=result;
//         Console.WriteLine("Image Path in Response: " + imageRequest.ImagePath);

//     }
//     return imageRequest;

// }


// public async Task <bool> checkBike(int BikeId)
// {
//     var query=@"select count(1) from Bikes where BikeId=@BikeId";

//     using(var connection=new SqlConnection(_connectionString))
//     {
//         var result=await connection.ExecuteScalarAsync<int>(query,new{BikeId});
//         if(result == 0)
//         {
//             return false;
//         }else{
//             return true;
//         }
//     }
// }

// public async Task <bool> checkImgId(int ImageId)
// {
//     var query=@"select count(1) from BikeImages where ImageId=@ImageId";

//     using(var connection=new SqlConnection(_connectionString))
//     {
//         var result=await connection.ExecuteScalarAsync<int>(query,new{ImageId});

//         if(result ==0)
//         {
//             return false;
//         }else{
//             return true;
//         }
//     }
// }

// public async   Task <bool> UpdateImages(int ImageId,BikeImages imageRequest)
// {
//     var query=@"update BikeImages set ImagePath=@ImagePath where ImageId=@ImageId AND BikeId=@BikeId";

//     using(var connection = new SqlConnection(_connectionString))
//     {
//         var result=await connection.ExecuteAsync(query,new{
//             ImagePath=imageRequest.ImagePath,
//             ImageId=ImageId,
//             BikeId=imageRequest.BikeId

//         });
//     }
//     return true;
// }


// public async Task <bool> DeleteImage(int ImageId)
// {
//     var query=@"delete from BikeImages where ImageId=@ImageId";

//     using(var connection=new SqlConnection(_connectionString))
//     {
//         var result=await connection.ExecuteAsync(query,new{ImageId});

//     }
//     return true;
// }

// public async Task <List<AllBikeImages>> AllBikeImages()
// {
//     var query=@"select Bikes.BikeId,Bikes.BikeName , Bikes.Rent ,BikeImages.ImagePath,Bikes.RegNo
//     From Bikes join BikeImages on Bikes.BikeId=BikeImages.BikeId";

//     using(var connection=new SqlConnection(_connectionString))
//     {
//         var result=await connection.QueryAsync<AllBikeImages>(query);
//         return result.ToList();
//     }
               
// }



// public async Task <List<AllBikeImages>> SearchBikes(string BikeName,int Rent)
// {
//     var query=@"select Bikes.BikeId,Bikes.BikeName,Bikes.Rent,Bikes.RegNo,BikeImages.ImagePath
//     from Bikes
//     join BikeImages on Bikes.BikeId=BikeImages.BikeId where Bikes.BikeName=BikeName OR Bikes.Rent<=Rent";

//     using(var connection=new SqlConnection(_connectionString))
//     {
//         var result=await connection.QueryAsync<AllBikeImages>(query);
//         return result.ToList();
//     }
// }

}
}
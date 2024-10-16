using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using Dapper;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;
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
    var bikeInsertQuery = @"INSERT INTO Bikes (Model, Brand, Rent) 
                            OUTPUT INSERTED.BikeId  
                            VALUES (@model, @brand, @rent)";

    var bikeUnitInsertQuery = @"
        INSERT INTO BikeUnits (BikeId, RegistrationNumber, Year, Status)
        OUTPUT INSERTED.UnitId
        VALUES (@BikeId, @RegistrationNumber, @Year, @Status)";

    var bikeImageInsertQuery = @"
        INSERT INTO BikeImages (BikeId, UnitId, ImagePath) 
        VALUES (@BikeId, @UnitId, @Path)";

    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        try
        {
            
            using (var command = new SqlCommand(bikeInsertQuery, connection))
            {
                command.Parameters.AddWithValue("@model", bike.Model);
                command.Parameters.AddWithValue("@brand", bike.Brand);
                command.Parameters.AddWithValue("@rent", bike.Rent);

              
                bike.BikeId = (int)await command.ExecuteScalarAsync();
                Console.WriteLine($"Inserted Bike ID: {bike.BikeId}"); 
            }

            foreach (var unit in bike.Units)
            {
                
                Console.WriteLine($"Inserting Unit: RegistrationNumber={unit.RegistrationNumber}, Year={unit.Year}, Status={unit.Status}");
                
                int bikeUnitId;
                using (var unitCommand = new SqlCommand(bikeUnitInsertQuery, connection))
                {
                    unitCommand.Parameters.AddWithValue("@BikeId", bike.BikeId);
                    unitCommand.Parameters.AddWithValue("@RegistrationNumber", unit.RegistrationNumber);
                    unitCommand.Parameters.AddWithValue("@Year", unit.Year);
                    unitCommand.Parameters.AddWithValue("@Status", unit.Status);

                    bikeUnitId = (int)await unitCommand.ExecuteScalarAsync();
                    Console.WriteLine($"Inserted Unit ID: {bikeUnitId}"); 
                }

                foreach (var image in unit.Images)
                {
                    
                    Console.WriteLine($"Inserting Image: Path={image.ImagePath}");

                    using (var imgCommand = new SqlCommand(bikeImageInsertQuery, connection))
                    {
                        imgCommand.Parameters.AddWithValue("@BikeId", bike.BikeId);
                        imgCommand.Parameters.AddWithValue("@UnitId", bikeUnitId);
                        imgCommand.Parameters.AddWithValue("@Path", image.ImagePath);

                        await imgCommand.ExecuteNonQueryAsync();
                        Console.WriteLine($"Inserted Image Path: {image.ImagePath}"); 
                    }
                }
            }

            return true;  
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error while adding bike: {ex.Message}");
            throw; 
        }
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

    var bikes = new List<BikeResponseDTO>();

    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        using (var command = new SqlCommand(query, connection))
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var bike = new BikeResponseDTO
                    {
                        BikeId = reader.GetInt32(0), 
                        Model = reader.GetString(1), 
                        Brand = reader.GetString(2), 
                        Rent = reader.GetDecimal(3), 
                        RegistrationNumber = reader.GetString(4), 
                        Images = reader.IsDBNull(5) //check empty value
                            ? new List<BikeImageResponseDTO>() 
                            : reader.GetString(5).Split(',')
                                .Select(path => new BikeImageResponseDTO
                                {
                                    ImagePath = path
                                }).ToList() 
                    };

                    bikes.Add(bike);
                }
            }
        }
    }

    return bikes;
}

public async  Task <bool> UpdateBike(int BikeId,BikeRequestDTO bikeRequest)
{
    try{
           var query=@"Update Bikes SET Brand=@brand,Model=@model,Rent=@rent 
    where BikeId=@Id ";

    using(var connection=new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();
        using(var command=new SqlCommand(query,connection))
        {
            command.Parameters.AddWithValue("@brand",bikeRequest.Brand);
            command.Parameters.AddWithValue("@model",bikeRequest.Model);
            command.Parameters.AddWithValue("@rent",bikeRequest.Rent);
            command.Parameters.AddWithValue("@Id",BikeId);

            await command.ExecuteNonQueryAsync();


           foreach (var unit in bikeRequest.Units)
           {
               var unit_query=@"Update BikeUnits 
               set  
               RegistrationNumber=  @RegistrationNumber,
               Year= @Year,
               Status=@Status
                        
           
             where BikeId=@Id";

               using(var command_unit=new SqlCommand(unit_query,connection))
               {
                command_unit.Parameters.AddWithValue("@RegistrationNumber",unit.RegistrationNumber);
                command_unit.Parameters.AddWithValue("@Year",unit.Year);
                command_unit.Parameters.AddWithValue("@Status",unit.Status);
                command_unit.Parameters.AddWithValue("@Id",BikeId);

                await command_unit.ExecuteNonQueryAsync();
               }

               foreach(var img in unit.Images)
               {
                  var img_query=@"Update BikeImages
                  SET ImagePath=@path where BikeId=@Id";

                  using(var img_command=new SqlCommand(img_query,connection))
                  {
                    img_command.Parameters.AddWithValue("@path",img.ImagePath);
                    img_command.Parameters.AddWithValue("@Id",BikeId);

                    await img_command.ExecuteNonQueryAsync();

                    
                  }
               }
            
           }

        }

        
    }
   return true;

    }catch(Exception ex)
    {
        throw new Exception(ex.Message);
    }
 
}


  public async Task<bool> DeleteBike(int id)
{
    var chkBikeQuery = @"SELECT COUNT(1) FROM ReturnedBikes WHERE BikeId = @Id AND Status=@status" ;
    var deleteBikeQuery = @"DELETE FROM Bikes WHERE BikeId = @Id";
    var deleteBikeUnitQuery=@"Delete from BikeUnits where BikeId=@Id";
    var deleteBikeImageQuery=@"Delete from BikeImages where BikeId=@Id";

    using (var connection = new SqlConnection(_connectionString))
    {
       
        var rentalCount = await connection.ExecuteScalarAsync<int>(chkBikeQuery, new { Id=id,status="Pending" });
        
        if (rentalCount > 0)
        {
         
            return false;
        }

      

         await connection.ExecuteAsync(deleteBikeImageQuery,new{Id=id});
         await connection.ExecuteAsync(deleteBikeUnitQuery,new{Id=id});
        var deletedRows=await connection.ExecuteAsync(deleteBikeQuery, new { Id=id  });
    
        
      
        return deletedRows > 0;
    }
}


public async  Task <int> BikesCount()
{
    var query=@"Select count(BikeUnits.RegistrationNumber) from BikeUnits";

    using(var connection=new SqlConnection(_connectionString))
    {
        using(var command=new SqlCommand(query,connection))
        {
            await connection.OpenAsync();
            var count = await command.ExecuteScalarAsync();
            return (int)count;


        }
    }
}

public async Task <int> PendingCount()
{
    var query = @"Select count(ReturnedBikes.RegistrationNumber)
     from ReturnedBikes where ReturnedBikes.Status='Pending'";
  using(var connection=new SqlConnection(_connectionString))
    {
        using(var command=new SqlCommand(query,connection))
        {
            await connection.OpenAsync();
            var count = await command.ExecuteScalarAsync();
            return (int)count;


        }
    }
}



public async Task<List<BikeResponseDTO>> SearchBikes(decimal rent, string brand, string model)
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
        WHERE (Bikes.Rent <= @Rent )
        OR (Bikes.Model = @Model )
        OR (Bikes.Brand = @Brand)
        GROUP BY 
            Bikes.BikeId, 
            Bikes.Model, 
            Bikes.Brand, 
            Bikes.Rent, 
            BikeUnits.RegistrationNumber";

    var bikes = new List<BikeResponseDTO>();
    
    using (var connection = new SqlConnection(_connectionString))
    {
        await connection.OpenAsync();

        using (var command = new SqlCommand(query, connection))
        {
           
            command.Parameters.AddWithValue("@Rent", rent);
            command.Parameters.AddWithValue("@Model",model); 
            command.Parameters.AddWithValue("@Brand",brand);
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var bike = new BikeResponseDTO
                    {
                        BikeId = reader.GetInt32(0),
                        Model = reader.GetString(1),
                        Brand = reader.GetString(2),
                        Rent = reader.GetDecimal(3),
                        RegistrationNumber = reader.GetString(4),
                        Images = reader.IsDBNull(5) 
                            ? new List<BikeImageResponseDTO>() 
                            : reader.GetString(5).Split(',')
                                .Select(path => new BikeImageResponseDTO
                                {
                                    ImagePath = path
                                }).ToList()
                    };

                    bikes.Add(bike);
                }
            }
        }
    }

    return bikes;
}

public async Task<BikeResponseDTO> GetById(int id)
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
        WHERE BikeUnits.BikeId=@id
        GROUP BY 
            Bikes.BikeId, 
            Bikes.Model, 
            Bikes.Brand, 
            Bikes.Rent, 
            BikeUnits.RegistrationNumber";

            using(var connection=new SqlConnection(_connectionString))
            {
                using(var command=new SqlCommand(query,connection))
                {
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@id",id);
                   
                   using(var reader=await command.ExecuteReaderAsync())
                   {
                    if(await reader.ReadAsync())
                    {
                            return new BikeResponseDTO
                            {
                        BikeId = reader.GetInt32(0),
                        Model = reader.GetString(1),
                        Brand = reader.GetString(2),
                        Rent = reader.GetDecimal(3),
                        RegistrationNumber = reader.GetString(4),
                        Images = reader.IsDBNull(5) 
                            ? new List<BikeImageResponseDTO>() 
                            : reader.GetString(5).Split(',')
                                .Select(path => new BikeImageResponseDTO
                                {
                                    ImagePath = path
                                }).ToList()

                            };
                    }
                    return null;
                   }
                   
                        
                }
            }
        
}

}
}
//string_AGG gets a list of values as comma seperated
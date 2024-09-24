using System;
using Microsoft.Data.SqlClient;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using Dapper;

namespace MotorBikeRental.Repository
{
    public class BikeRepository:IBikeRepository
{

    private readonly string _connectionString;

    public BikeRepository(string connectionString )
    {
        _connectionString=connectionString;
    }

    public async Task <bool> CheckUnique(string RegNo)
    {

        var query=@"select count(1) from Bikes where RegNo=@RegNo";

        using(var connection=new SqlConnection(_connectionString))
        {

            var result=(int) await connection.ExecuteScalarAsync(query,new{

                RegNo=RegNo

            });

            return result==0;

        }
    
    }


    public async Task <Bike>AddBike(Bike bike)
    {
        var query=@"insert into Bikes (BikeName,Rent,RegNo,Status)
         OUTPUT INSERTED.bikeId  
        values(@BikeName,@Rent,@RegNo,@Status)";

    using(var connection=new SqlConnection(_connectionString))
    {
        var result=await connection.ExecuteScalarAsync<int>(query,new{

            bike.BikeName,
            bike.Rent,
            bike.RegNo,
            bike.Status

        });
       bike.BikeId=result ;
    }

    return bike;


    }

  public async  Task <List<AllBikes>> GetAllBikes()
  {
    var query=@"select * from Bikes";

    using(var connection=new SqlConnection(_connectionString))
    {
        var result=await connection.QueryAsync<AllBikes>(query);
        return result.ToList();
    }
  }

  public async Task<bool> DeleteBike(int id)
{
    var chkBikeQuery = @"SELECT COUNT(1) FROM RentalRequest WHERE BikeId = @Id";
    var deleteQuery = @"DELETE FROM Bikes WHERE BikeId = @Id";

    using (var connection = new SqlConnection(_connectionString))
    {
       
        var rentalCount = await connection.ExecuteScalarAsync<int>(chkBikeQuery, new { Id=id });
        
        if (rentalCount > 0)
        {
         
            return false;
        }

      
        var deletedRows = await connection.ExecuteAsync(deleteQuery, new { Id=id  });
        
      
        return deletedRows > 0;
    }
}


public async Task <BikeImages> AddImages(BikeImages imageRequest)
{

    var query=@"insert into BikeImages (BikeId,ImagePath) 
    output inserted.ImageId
    values(@BikeId,@ImagePath)";

    using(var connection=new SqlConnection(_connectionString))
    {
        var result=await connection.ExecuteScalarAsync<int>(query,new{
           BikeId= imageRequest.BikeId,
           ImagePath=imageRequest.ImagePath

        });

        imageRequest.ImageId=result;
        Console.WriteLine("Image Path in Response: " + imageRequest.ImagePath);

    }
    return imageRequest;

}


public async Task <bool> checkBike(int BikeId)
{
    var query=@"select count(1) from Bikes where BikeId=@BikeId";

    using(var connection=new SqlConnection(_connectionString))
    {
        var result=await connection.ExecuteScalarAsync<int>(query,new{BikeId});
        if(result == 0)
        {
            return false;
        }else{
            return true;
        }
    }
}

public async Task <bool> checkImgId(int ImageId)
{
    var query=@"select count(1) from BikeImages where ImageId=@ImageId";

    using(var connection=new SqlConnection(_connectionString))
    {
        var result=await connection.ExecuteScalarAsync<int>(query,new{ImageId});

        if(result ==0)
        {
            return false;
        }else{
            return true;
        }
    }
}

public async   Task <bool> UpdateImages(int ImageId,BikeImages imageRequest)
{
    var query=@"update BikeImages set ImagePath=@ImagePath where ImageId=@ImageId AND BikeId=@BikeId";

    using(var connection = new SqlConnection(_connectionString))
    {
        var result=await connection.ExecuteAsync(query,new{
            ImagePath=imageRequest.ImagePath,
            ImageId=ImageId,
            BikeId=imageRequest.BikeId

        });
    }
    return true;
}


public async Task <bool> DeleteImage(int ImageId)
{
    var query=@"delete from BikeImages where ImageId=@ImageId";

    using(var connection=new SqlConnection(_connectionString))
    {
        var result=await connection.ExecuteAsync(query,new{ImageId});

    }
    return true;
}

public async Task <List<AllBikeImages>> AllBikeImages()
{
    var query=@"select Bikes.BikeId,Bikes.BikeName , Bikes.Rent ,BikeImages.ImagePath,Bikes.RegNo
    From Bikes join BikeImages on Bikes.BikeId=BikeImages.BikeId";

    using(var connection=new SqlConnection(_connectionString))
    {
        var result=await connection.QueryAsync<AllBikeImages>(query);
        return result.ToList();
    }
               
}

}
}
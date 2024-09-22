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

}

}



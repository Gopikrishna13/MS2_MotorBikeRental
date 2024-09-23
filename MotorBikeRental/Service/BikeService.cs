using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;

namespace MotorBikeRental.Service
{

    public class BikeService:IBikeService
{
    private readonly IBikeRepository _bikeRepository;

    public BikeService(IBikeRepository bikeRepository)
    {
        _bikeRepository=bikeRepository;
    }


   public async Task<List<BikeResponseDTO>> AddBike(BikeRequestDTO bikeRequestDTO)
{
    var responseList = new List<BikeResponseDTO>();

    foreach (var regNo in bikeRequestDTO.RegNo)
    {
        var isUnique = await _bikeRepository.CheckUnique(regNo);
        if (!isUnique)
        {
            throw new Exception("Registration number already exists");
        }

        var data = new Bike
        {
            BikeName = bikeRequestDTO.BikeName,
            Rent = bikeRequestDTO.Rent,
            RegNo = new List<string> { regNo }, 
            Status = bikeRequestDTO.Status
        };

        var addBike = await _bikeRepository.AddBike(data);

        var response = new BikeResponseDTO
        {
            BikeId = addBike.BikeId,
            BikeName = addBike.BikeName,
            Rent = addBike.Rent,
            RegNo = regNo,  
            Status = addBike.Status
        };
        responseList.Add(response);
    }

    return responseList;
}


public async Task <List<BikeResponseDTO>> GetAllBikes()
{

    var data=await _bikeRepository.GetAllBikes();

    if(data==null)
    {
        throw new Exception("No Bikes found");
    }

   var response=data.Select(x=>new BikeResponseDTO{
            BikeId = x.BikeId,
            BikeName = x.BikeName,
            Rent = x.Rent,
            RegNo = x.RegNo,  
            Status = x.Status
   }).ToList();

   return response;

}

public async Task <bool> DeleteBike(int Id)
{
    var data=await _bikeRepository.DeleteBike(Id);

    if(data==null)
    {
        return false;
    }

    return true;
}


public async Task <List<BikeImageResponseDTO>> AddImages(BikeImageRequestDTO imageRequestDTO)
{
    var responseList = new List<BikeImageResponseDTO>();

foreach(var img in imageRequestDTO.ImagePath)
{
    var bike_data=new BikeImages{
        BikeId=imageRequestDTO.BikeId,
        ImagePath=new List <string> {img}

    };

    var data=await _bikeRepository.AddImages(bike_data);

    var response=new BikeImageResponseDTO{
        ImageId=data.ImageId,
        BikeId=data.BikeId,
        ImagePath=img

    };

    responseList.Add(response);


}
return responseList;

   
}

}

}



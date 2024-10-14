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


   public async Task<bool> AddBike(BikeRequestDTO bikeRequestDTO)
{
   // var responseList = new List<BikeResponseDTO>();

    foreach (var unit in bikeRequestDTO.Units)
    {
        var isUnique = await _bikeRepository.CheckUnique(unit.RegistrationNumber);
        if (!isUnique)
        {
            throw new Exception("Registration number already exists");
        }

var bikeImages=unit.Images.Select(image=>new BikeImages{
ImagePath=image.ImagePath
}).ToList();


        var data = new Bike
        {
            Model = bikeRequestDTO.Model,
            Brand=bikeRequestDTO.Brand,
            Rent = bikeRequestDTO.Rent,
            Units = new List<BikeUnit> { new BikeUnit {
           
            RegistrationNumber=unit.RegistrationNumber,
            Year=unit.Year,
            Images=bikeImages,
            Status=unit.Status
           } }
           
        };

        var addBike = await _bikeRepository.AddBike(data);

        if(addBike)
        {
            return true;
        }

      
    }
    return false;

}


public async Task <List<BikeResponseDTO>> GetAllBikes()
{

    var data=await _bikeRepository.GetAllBikes();

    if(data==null)
    {
        throw new Exception("No Bikes found");
    }

  

   return data.ToList();

}

// public async Task <bool> DeleteBike(int Id)
// {
//     var data=await _bikeRepository.DeleteBike(Id);

//     if(data==null)
//     {
//         return false;
//     }

//     return true;
// }


// public async Task <List<BikeImageResponseDTO>> AddImages(BikeImageRequestDTO imageRequestDTO)
// {
//     var responseList = new List<BikeImageResponseDTO>();

//     var chkBike=await _bikeRepository.checkBike(imageRequestDTO.BikeId);

//     if(!chkBike)
//     {
//        throw new Exception("No such Bike!");

//     }

// foreach(var img in imageRequestDTO.ImagePath)
// {
//     var bike_data=new BikeImages{
//         BikeId=imageRequestDTO.BikeId,
//         ImagePath=new List <string> {img}

//     };

//     var data=await _bikeRepository.AddImages(bike_data);

//     var response=new BikeImageResponseDTO{
//         ImageId=data.ImageId,
//         BikeId=data.BikeId,
//         ImagePath=img

//     };

//     responseList.Add(response);


// }
// return responseList;

   
// }


// public async Task <bool> UpdateImages(int ImageId,BikeImageRequestDTO imageRequestDTO)
// {
//     var chkBike=await _bikeRepository.checkBike(imageRequestDTO.BikeId);
//     var chkImgId=await _bikeRepository.checkImgId(ImageId);

//     if(!chkBike || ! chkImgId)
//     {
//         throw new Exception("Invalid input");
//     }

//     if(!chkBike)
//     {
//         throw new Exception("No such Bike!");
//     }


//     var bike=new BikeImages
//     {
//         BikeId=imageRequestDTO.BikeId,
//         ImagePath=imageRequestDTO.ImagePath

//     };

//     var data=await _bikeRepository.UpdateImages(ImageId,bike);
//     return true;
// }


// public async Task <bool> DeleteImage(int ImageId)
// {
//     var checkImgId=await _bikeRepository.checkImgId(ImageId);

//     if(!checkImgId)
//     {
//         throw new Exception("No such Image");
//     }

//     var data=await _bikeRepository.DeleteImage(ImageId);
//     return true;
// }

// public async Task <List<AllBikeImages>> AllBikeImages()
// {
//     var data=await _bikeRepository.AllBikeImages();

//     if(data == null)
//     {
//         throw new Exception("No data found");

//     }

//     var response=data.Select(x=>new AllBikeImages{
//             BikeId = x.BikeId,
//             BikeName = x.BikeName,
//             Rent = x.Rent,
//             RegNo = x.RegNo,  
//             ImagePath=x.ImagePath
//    }).ToList();

//    return response;
// }

// public async  Task <List<AllBikeImages>> SearchBikes(string BikeName,int Rent)
// {
//     var data=await _bikeRepository.SearchBikes(BikeName,Rent);

//     if(data == null)
//     {
//         throw new Exception ("Data could not be Found!");
//     }

//     var response=data.Select(x=>new AllBikeImages{

//         BikeId=x.BikeId,
//         BikeName=x.BikeName,
//         Rent=x.Rent,
//         RegNo=x.RegNo,
//         ImagePath=x.ImagePath

//     }).ToList();

//     return response;

// }

}

}



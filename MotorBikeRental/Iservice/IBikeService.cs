using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.Database.Entities;

namespace MotorBikeRental.Iservice
{

    public interface IBikeService
{
    Task<bool> AddBike(BikeRequestDTO bikeRequestDTO);
    Task <List<BikeResponseDTO>> GetAllBikes();
     Task <bool> DeleteBike(int Id);
   // Task <List<BikeImageResponseDTO>> AddImages(BikeImageRequestDTO imageRequestDTO);
   // Task <bool> UpdateImages(int ImageId,BikeImageRequestDTO imageRequestDTO);
   // Task <bool> DeleteImage(int ImageId);
   // Task <List<AllBikeImages>> AllBikeImages();
  Task <List<BikeResponseDTO>> SearchBikes(decimal Rent,string Brand,string Model);
  Task <BikeResponseDTO> GetByRegistration(string RegNo);
  Task <bool> UpdateBike(int BikeId,BikeRequestDTO bikeRequest);
    

}

}



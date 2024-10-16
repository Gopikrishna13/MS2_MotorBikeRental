using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;
using System.Collections.Generic;
using MotorBikeRental.DTOs.RequestDTO;


namespace MotorBikeRental.IRepository
{
    public interface IBikeRepository
{
    Task<bool> AddBike(Bike bike);
   
    Task <bool> CheckUnique(string RegNo);
     Task <List<BikeResponseDTO>> GetAllBikes();
     Task <bool> DeleteBike(int Id);
  //  Task <BikeImages> AddImages(BikeImages imageRequest);
   // Task <bool> checkBike(int BikeId);
   // Task <bool> UpdateImages(int ImageId,BikeImages imageRequest);
    //Task <bool> checkImgId(int ImageId);
   // Task <bool> DeleteImage(int ImageId);
    //Task <List<AllBikeImages>> AllBikeImages();
    Task <List<BikeResponseDTO>> SearchBikes(decimal Rent,string Brand,string Model);
    Task<BikeResponseDTO> GetById(int id);
    Task <bool> UpdateBike(int BikeId,BikeRequestDTO bikeRequest);
    Task <int> BikesCount();
    Task <int> PendingCount();
    }

}
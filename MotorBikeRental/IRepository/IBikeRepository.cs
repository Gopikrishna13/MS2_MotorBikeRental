using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;
using System.Collections.Generic;


namespace MotorBikeRental.IRepository
{
    public interface IBikeRepository
{
    Task<bool> AddBike(Bike bike);
   
    Task <bool> CheckUnique(string RegNo);
     Task <List<BikeResponseDTO>> GetAllBikes();
   // Task <bool> DeleteBike(int Id);
  //  Task <BikeImages> AddImages(BikeImages imageRequest);
   // Task <bool> checkBike(int BikeId);
   // Task <bool> UpdateImages(int ImageId,BikeImages imageRequest);
    //Task <bool> checkImgId(int ImageId);
   // Task <bool> DeleteImage(int ImageId);
    //Task <List<AllBikeImages>> AllBikeImages();
    Task <List<BikeResponseDTO>> SearchBikes(decimal Rent,string Brand,string Model);
    Task<BikeResponseDTO> GetByRegistartion(string regNo);
    }

}
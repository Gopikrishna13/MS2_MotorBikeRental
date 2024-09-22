using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;


namespace MotorBikeRental.IRepository
{
    public interface IBikeRepository
{
    Task <Bike> AddBike (Bike bike);
     Task <bool> CheckUnique(string RegNo);

}


}


using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;
namespace MotorBikeRental.IRepository
{
public interface IUserRepository
{
   Task<User > AddUser(User user);
   Task <bool> CheckUnique(string UserName,string Email,string LicenseNumber);
   Task <List<User>> GetAllUsers();

}
}



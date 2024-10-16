using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;

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
   Task<User> GetUserById(int Id);
   Task<User> UpdateUser(int Id,User user);
   Task<bool> DeleteUser(int Id);
   Task <bool> Login(UserLoginRequestDTO userloginRequestDTO);
   Task<User> GetByusername(string username);
       Task <int> UserCount();

}
}



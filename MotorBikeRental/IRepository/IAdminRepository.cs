using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.DTOs.RequestDTO;



namespace MotorBikeRental.IRepository
{
public interface IAdminRepository
{
   Task<Admin > AddAdmin(Admin admin);
   Task <bool> CheckUnique(string UserName,string Email,string NIC);
   Task <List<Admin>> GetAllAdmins();
   Task<Admin> GetAdminByusername(string username);
   Task<Admin> UpdateAdmin(int Id,Admin admin);
   Task<bool> DeleteAdmin(int Id);
   Task <bool> Login(AdminLoginRequestDTO adminloginRequestDTO);
 //  Task <bool> checkUpdate(int Id,string UserName,string Email,string NIC);

}
}



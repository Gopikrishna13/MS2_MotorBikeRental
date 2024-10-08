using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;
namespace MotorBikeRental.IRepository
{
public interface IAdminRepository
{
   Task<Admin > AddAdmin(Admin admin);
   Task <bool> CheckUnique(string UserName,string Email,string NIC);
   Task <List<Admin>> GetAllAdmins();
   Task<Admin> GetAdminById(int Id);
   Task<Admin> UpdateAdmin(int Id,Admin admin);
   Task<bool> DeleteAdmin(int Id);

}
}



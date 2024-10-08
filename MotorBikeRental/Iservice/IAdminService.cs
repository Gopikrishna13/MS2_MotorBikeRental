using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;

namespace MotorBikeRental.Iservice
{
public interface IAdminService
{
    Task <AdminResponseDTO> AddAdmin(AdminRequestDTO adminRequestDTO);
    Task <List<AdminResponseDTO>> GetAllAdmins();
     Task<AdminResponseDTO> GetAdminByusername(string username);
    Task<AdminResponseDTO> UpdateAdmin(int Id,AdminRequestDTO adminRequestDTO);
    Task<bool> DeleteAdmin(int Id);
    Task <bool> Login(AdminLoginRequestDTO adminloginRequestDTO);

}

}



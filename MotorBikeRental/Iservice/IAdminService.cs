using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;

namespace MotorBikeRental.Iservice
{
public interface IAdminService
{
    Task <AdminResponseDTO> AddAdmin(AdminRequestDTO adminRequestDTO);
    Task <List<AdminResponseDTO>> GetAllAdmins();
    Task<AdminResponseDTO> GetAdminById(int Id);
    Task<AdminResponseDTO> UpdateAdmin(int Id,AdminRequestDTO adminRequestDTO);
    Task<bool> DeleteAdmin(int Id);
}

}



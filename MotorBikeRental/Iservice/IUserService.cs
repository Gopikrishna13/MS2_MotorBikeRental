using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;

namespace MotorBikeRental.Iservice
{
public interface IUserService
{
    Task <UserResponseDTO> AddUser(UserRequestDTO userRequestDTO);
    Task <List<UserResponseDTO>> GetAllUsers();
}

}



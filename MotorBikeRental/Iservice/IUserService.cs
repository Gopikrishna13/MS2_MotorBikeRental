using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;

namespace MotorBikeRental.Iservice
{
public interface IUserService
{
    Task <UserResponseDTO> AddUser(UserRequestDTO userRequestDTO);
    Task <List<UserResponseDTO>> GetAllUsers();
    Task<UserResponseDTO> GetUserById(int Id);
    Task<UserResponseDTO> UpdateUser(int Id,UserRequestDTO userRequestDTO);
    Task<bool> DeleteUser(int Id);
    Task <bool> Login(UserLoginRequestDTO userloginRequestDTO);
    Task<UserResponseDTO> GetByusername(string username);
    Task <int> UserCount();
}

}



using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;

namespace MotorBikeRental.Service
{

public class UserService:IUserService
{
private readonly  IUserRepository _userRepository;

public UserService(IUserRepository userRepository)
{
    _userRepository=userRepository;
}


public async Task <UserResponseDTO> AddUser(UserRequestDTO userRequestDTO)
{

    var isUnique=await _userRepository.CheckUnique(userRequestDTO.UserName,userRequestDTO.Email,userRequestDTO.LicenseNumber
);

if(!isUnique)
{
    throw new Exception("Data already Exists");
}

    var user=new User{

        FirstName =userRequestDTO.FirstName,
        LastName  =userRequestDTO.LastName,
        UserName  =userRequestDTO.UserName,
        Password  =userRequestDTO.Password,
        NIC  =userRequestDTO.NIC,
        Email  =userRequestDTO.Email,
        LicenseNumber=userRequestDTO.LicenseNumber

    };

    var data=await _userRepository.AddUser(user);

    var userResponse=new UserResponseDTO{
            
            FirstName = data.FirstName,
            LastName = data.LastName,
            UserName = data.UserName,
            Password=data.Password,
            NIC=data.NIC,
            Email = data.Email,
            LicenseNumber=data.LicenseNumber

    };
    return userResponse;

}


}

}



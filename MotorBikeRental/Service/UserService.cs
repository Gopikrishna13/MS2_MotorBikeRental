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
            UserId=data.UserId,
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


public async Task <bool> Login(UserLoginRequestDTO userloginRequestDTO)
{
    var chkLogin=await _userRepository.Login(userloginRequestDTO);
    if(chkLogin)
    {
        return true;
    }else{
        return false;
    }
}

public async Task<UserResponseDTO> GetByusername(string username)
{
     var getByusername=await _userRepository.GetByusername(username);
    if(getByusername==null)
    {
        return null;
    }

    var response=new UserResponseDTO{
            UserId=getByusername.UserId,
            FirstName = getByusername.FirstName,
            LastName = getByusername.LastName,
            UserName = getByusername.UserName,
            Password=getByusername.Password,
            NIC=getByusername.NIC,
            Email = getByusername.Email,
            LicenseNumber=getByusername.LicenseNumber

    };
    return response;
}

public async Task<List<UserResponseDTO>> GetAllUsers()
{
    var getAll = await _userRepository.GetAllUsers(); 
    
    if (getAll == null ) 
    {
        return new List<UserResponseDTO>(); 
    }

    var responseDTO = getAll.Select(X => new UserResponseDTO
    {
        UserId=X.UserId,
        FirstName = X.FirstName,
        LastName = X.LastName,
        UserName = X.UserName,
        Password = X.Password,
        NIC = X.NIC,
        Email = X.Email,
        LicenseNumber = X.LicenseNumber
    }).ToList();

    return responseDTO;
}

public async Task<UserResponseDTO> GetUserById(int Id)
{
    var getById=await _userRepository.GetUserById(Id);
    if(getById==null)
    {
        return null;
    }

    var response=new UserResponseDTO{
        
            FirstName = getById.FirstName,
            LastName = getById.LastName,
            UserName = getById.UserName,
            Password=getById.Password,
            NIC=getById.NIC,
            Email = getById.Email,
            LicenseNumber=getById.LicenseNumber

    };
    return response;
}


   public async Task<UserResponseDTO> UpdateUser(int Id,UserRequestDTO userRequestDTO)
   {
// var isUnique=await _userRepository.CheckUnique(userRequestDTO.UserName,userRequestDTO.Email,userRequestDTO.LicenseNumber
// );

// if(!isUnique)
// {
//     throw new Exception("Data already Exists");
// }
    var user=new User{
        FirstName =userRequestDTO.FirstName,
        LastName  =userRequestDTO.LastName,
        UserName  =userRequestDTO.UserName,
        Password  =userRequestDTO.Password,
        NIC  =userRequestDTO.NIC,
        Email  =userRequestDTO.Email,
        LicenseNumber=userRequestDTO.LicenseNumber
    };
     var update=await _userRepository.UpdateUser(Id,user);
     if(update==null)
     {
        return null;
     }


     var response=new UserResponseDTO{
            UserId=Id,
            FirstName = update.FirstName,
            LastName = update.LastName,
            UserName = update.UserName,
            Password=update.Password,
            NIC=update.NIC,
            Email = update.Email,
            LicenseNumber=update.LicenseNumber

     };
     return response;
   }


  public async Task<bool> DeleteUser(int Id)
  {
    var data=await _userRepository.DeleteUser(Id);
    if(!data)
    {
        return false;
    }

    return true;
  }

 public async Task <int> UserCount()
 {
    var data=await _userRepository.UserCount();
    return data;
 }
}

}



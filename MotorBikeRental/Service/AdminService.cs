using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;

namespace MotorBikeRental.Service
{

public class AdminService:IAdminService
{
private readonly  IAdminRepository _adminRepository;

public AdminService(IAdminRepository adminRepository)
{
    _adminRepository=adminRepository;
}


public async Task <AdminResponseDTO> AddAdmin(AdminRequestDTO adminRequestDTO)
{

    var isUnique=await _adminRepository.CheckUnique(adminRequestDTO.UserName,adminRequestDTO.Email,adminRequestDTO.NIC);

if(!isUnique)
{
    throw new Exception("Data already Exists");
}

    var user=new Admin{

        FirstName =adminRequestDTO.FirstName,
        LastName  =adminRequestDTO.LastName,
        UserName  =adminRequestDTO.UserName,
        Password  =adminRequestDTO.Password,
        NIC  =adminRequestDTO.NIC,
        Email  =adminRequestDTO.Email
    };

    var data=await _adminRepository.AddAdmin(user);

    var adminResponse=new AdminResponseDTO{
            AdminId=data.AdminId,
            FirstName = data.FirstName,
            LastName = data.LastName,
            UserName = data.UserName,
            Password=data.Password,
            NIC=data.NIC,
            Email = data.Email

    };
    return adminResponse;

}

public async Task <bool> Login(AdminLoginRequestDTO adminloginRequestDTO)
{
    var checkLogin=await _adminRepository.Login(adminloginRequestDTO);
    if(checkLogin)
    {
        return true;
    }else{
        return false;
    }
}
public async Task<List<AdminResponseDTO>> GetAllAdmins()
{
    var getAll = await _adminRepository.GetAllAdmins(); 
    
    if (getAll == null ) 
    {
        return new List<AdminResponseDTO>(); 
    }

    var responseDTO = getAll.Select(X => new AdminResponseDTO
    {
        AdminId=X.AdminId,
        FirstName = X.FirstName,
        LastName = X.LastName,
        UserName = X.UserName,
        Password = X.Password,
        NIC = X.NIC,
        Email = X.Email
    }).ToList();

    return responseDTO;
}

public async Task<AdminResponseDTO> GetAdminByusername(string username)
{
    var getByusername=await _adminRepository.GetAdminByusername(username);
    if(getByusername==null)
    {
        return null;
    }

    var response=new AdminResponseDTO{
            AdminId=getByusername.AdminId,
            FirstName = getByusername.FirstName,
            LastName = getByusername.LastName,
            UserName = getByusername.UserName,
            Password=getByusername.Password,
            NIC=getByusername.NIC,
            Email = getByusername.Email

    };
    return response;
}


   public async Task<AdminResponseDTO> UpdateAdmin(int Id,AdminRequestDTO adminRequestDTO)
{
// var isUnique=await _adminRepository.checkUpdate(Id,adminRequestDTO.UserName,adminRequestDTO.Email,adminRequestDTO.NIC);

// if(!isUnique)
// {
//     throw new Exception("Data already Exists");
// }
    var user=new Admin{
        FirstName =adminRequestDTO.FirstName,
        LastName  =adminRequestDTO.LastName,
        UserName  =adminRequestDTO.UserName,
        Password  =adminRequestDTO.Password,
        NIC  =adminRequestDTO.NIC,
        Email  =adminRequestDTO.Email
    };
     var update=await _adminRepository.UpdateAdmin(Id,user);
     if(update==null)
     {
        return null;
     }


     var response=new AdminResponseDTO{
            AdminId=Id,
            FirstName = update.FirstName,
            LastName = update.LastName,
            UserName = update.UserName,
            Password=update.Password,
            NIC=update.NIC,
            Email = update.Email

     };
     return response;
   }


  public async Task<bool> DeleteAdmin (int Id)
  {
    var data=await _adminRepository.DeleteAdmin(Id);
    if(!data)
    {
        return false;
    }

    return true;
  }


}
}




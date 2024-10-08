using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.Iservice;

namespace MotorBikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin(AdminRequestDTO adminRequestDTO)
        {
            try
            {
                var userData = await _adminService.AddAdmin(adminRequestDTO);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        } 
 [HttpPost("Login")]
 public async Task <IActionResult>Login(AdminLoginRequestDTO adminloginRequestDTO)
 {
    try{
        var data=await _adminService.Login(adminloginRequestDTO);
        return Ok(data);

    }catch(Exception ex)
    {
        return BadRequest(ex.Message);
    }

 }

        [HttpGet("AllAdmin")]
        public async Task <IActionResult> GetAllAdmins()
        {
            try{

                var getUser=await _adminService.GetAllAdmins();
                return Ok(getUser);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpGet("GetByUserName")]
        public async Task<IActionResult> GetAdminByusername(string username)
        {
            try{

                var getByusername=await _adminService.GetAdminByusername(username);
                return Ok(getByusername);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin(int Id,AdminRequestDTO adminRequestDTO)
        {
             try{

                var updateuser=await _adminService.UpdateAdmin(Id,adminRequestDTO);
                return Ok(updateuser);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 

        }

        [HttpDelete("DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin(int Id)
        {
            try{
                var deleteData=await _adminService.DeleteAdmin(Id);
                return Ok("Admin Deleted Successfully");
                
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    } 
}

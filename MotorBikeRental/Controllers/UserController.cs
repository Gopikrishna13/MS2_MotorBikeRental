using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.Iservice;

namespace MotorBikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(UserRequestDTO userRequestDTO)
        {
            try
            {
                var userData = await _userService.AddUser(userRequestDTO);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        } 

 [HttpPost("Login")]
 public async Task <IActionResult>Login(UserLoginRequestDTO userloginRequestDTO)
 {
    try{
        var data=await _userService.Login(userloginRequestDTO);
        return Ok(data);

    }catch(Exception ex)
    {
        return BadRequest(ex.Message);
    }

 }

     [HttpGet("GetByUserName")]
        public async Task<IActionResult> GetByusername(string username)
        {
            try{

                var getByusername=await _userService.GetByusername(username);
                return Ok(getByusername);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpGet("AllUsers")]
        public async Task <IActionResult> GetAllUsers()
        {
            try{

                var getUser=await _userService.GetAllUsers();
                return Ok(getUser);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetUserById(int Id)
        {
            try{

                var getById=await _userService.GetUserById(Id);
                return Ok(getById);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int Id,UserRequestDTO userRequestDTO)
        {
             try{

                var updateuser=await _userService.UpdateUser(Id,userRequestDTO);
                return Ok(updateuser);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 

        }

        [HttpDelete("DeletUser")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try{
                var deleteData=await _userService.DeleteUser(Id);
                return Ok("User Deleted Successfully");
                
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UserCount")]
        public async Task <IActionResult> UserCount()
        {
            try{
                var data=await _userService.UserCount();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    } 
}

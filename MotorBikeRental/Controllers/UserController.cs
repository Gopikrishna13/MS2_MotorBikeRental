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
    } 
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.Iservice;

namespace MotorBikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly IBikeService _bikeService;

        public  BikeController(IBikeService bikeService)
        {
            _bikeService=bikeService;
        }

        [HttpPost("AddBike")]
        public async Task <IActionResult> AddBike(BikeRequestDTO bikeRequestDTO)
        {
            try{
                var bike=await _bikeService.AddBike(bikeRequestDTO);
                return Ok(bike);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

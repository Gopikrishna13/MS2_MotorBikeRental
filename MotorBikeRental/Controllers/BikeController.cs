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

        [HttpGet("AllBikes")]

        public async Task <IActionResult> GetAllBikes()
        {
            try{

                var getbike=await _bikeService.GetAllBikes();
                return Ok(getbike);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task <IActionResult> GetById(int id)
        {
            try{
                var getbike=await _bikeService.GetById(id);
                return Ok(getbike);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteBike")]
        public async Task <IActionResult> DeleteBike(int Id)
        {
            try{

                var data=await _bikeService.DeleteBike(Id);
                return Ok("Bike Deleted Successfully");

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


          [HttpGet("SearchBikes")]
        public async Task <IActionResult> SearchBikes(decimal Rent,string Brand,string Model)
        {
            try{

                var data=await _bikeService.SearchBikes(Rent,Brand,Model);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


[HttpPut("UpdateBike")]
public async Task <IActionResult> UpdateBike(int BikeId,BikeRequestDTO bikeRequest)
{
    try{

        var data=await _bikeService.UpdateBike(BikeId,bikeRequest);
        return Ok(data);

    }catch(Exception ex)
    {
        return BadRequest(ex.Message);
    }
}

      [HttpGet("BikesCount")]
      public async Task <IActionResult> BikesCount()
      {
        try{
             var data=await _bikeService.BikesCount();
        return Ok(data);

        }catch(Exception ex)
        {
              return BadRequest(ex.Message);
        }
      }

      [HttpGet("PendingCount")]
      public async Task <IActionResult> PendingCount()
      {
        try{
              var data=await _bikeService.PendingCount();
        return Ok(data);

        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
      }

      
    }
}

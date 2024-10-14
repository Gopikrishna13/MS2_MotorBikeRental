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

        [HttpGet("GetByRegistratioNumber")]
        public async Task <IActionResult> GetByRegistration(string RegNo)
        {
            try{
                var getbike=await _bikeService.GetByRegistration(RegNo);
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

        // [HttpPost("AddImages")]
        // public async Task <IActionResult> AddImages(BikeImageRequestDTO imageRequestDTO)
        // {

        //     try{

        //         var data=await _bikeService.AddImages(imageRequestDTO);
        //         return Ok(data);

        //     }catch(Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }

        // }

        // [HttpPut("UpdateImages")]
        // public async Task <IActionResult> UpdateImages(int ImageId,BikeImageRequestDTO imageRequestDTO)
        // {
        //     try{
        //         var data=await _bikeService.UpdateImages(ImageId,imageRequestDTO);
        //         return Ok(data);

        //     }catch(Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }


        // [HttpDelete("DeleteImage")]
        // public async Task <IActionResult> DeleteImage(int ImageId)
        // {
        //     try{

        //         var data=await _bikeService.DeleteImage(ImageId);
        //         return Ok(data);

        //     }catch(Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }

        // [HttpGet("AllBikeImages")]
        // public async Task <IActionResult> AllBikeImages()
        // {
        //     try{

        //         var data=await _bikeService.AllBikeImages();
        //         return Ok(data);

        //     }catch(Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }

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
    }
}

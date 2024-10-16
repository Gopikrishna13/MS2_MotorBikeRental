using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.Iservice;


namespace MotorBikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalservice;

        public RentalController(IRentalService rentalService)
        {
            _rentalservice=rentalService;
        }

        [HttpPost("RentalRequest")]
        public async Task <IActionResult> RentalRequest(RentalRequestDTO requestDTO)
        {
            try{

                var data=await  _rentalservice.RentalRequest(requestDTO);
                return Ok(data);
                
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("AllRequest")]
        public async Task <IActionResult> AllRequest()
        {
            try{
                var data=await _rentalservice.AllRequest();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateRequest")]
        public async Task <IActionResult> UpdateRequest(int code,int Id)
        {
            try{
                var data=await _rentalservice.UpdateRequest(code,Id);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

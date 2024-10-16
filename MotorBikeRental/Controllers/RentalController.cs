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

        [HttpGet("AllReturnBike")]
        public async Task <IActionResult> AllReturnBike()
        {
            try{

                var data=await _rentalservice.AllReturnBike();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateReturn")]
        public async Task<IActionResult> UpdateReturn(int Id)
        {
                try{

                    var data=await _rentalservice.UpdateReturn(Id);
                    return Ok(data);

                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
        }

        [HttpGet("PendingByUser")]
        public async Task <IActionResult> PendingByUser(int Id)
        {
            try{

                var data=await _rentalservice.PendingByUser(Id);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

         [HttpGet("ReturnByUser")]
        public async Task <IActionResult> ReturnByUser(int Id)
        {
            try{

                var data=await _rentalservice.ReturnByUser(Id);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

          [HttpGet("Revenue")]
        public async Task <IActionResult> Revenue()
        {
            try{

                var data=await _rentalservice.Revenue();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CheckAvailability")]
        public async Task <IActionResult> CheckAvailability(string registrationNumber,DateTime reqdate,DateTime retdate)
        {
            try{
                var data=await _rentalservice.CheckAvailability(registrationNumber,reqdate,retdate);
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}

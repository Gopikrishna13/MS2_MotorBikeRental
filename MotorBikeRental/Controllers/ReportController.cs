using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotorBikeRental.Iservice;
namespace MotorBikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService  _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService=reportService;
        }


        [HttpGet("GetCustomerReport")]
        public async Task <IActionResult> GetcustomerReport()
        {
            try{

                var data=await _reportService.GetcustomerReport();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("InventoryReport")]
        public async Task <IActionResult> GetInventoryReport()
        {
            try{
                var data=await _reportService.GetInventoryReport();
                return Ok(data);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("FrequentRent")]
        
            public async Task <IActionResult> FrequentRent()
            {
                try{
                    var data=await _reportService.FrequentRent();
                    return Ok(data);

                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        
    }
}

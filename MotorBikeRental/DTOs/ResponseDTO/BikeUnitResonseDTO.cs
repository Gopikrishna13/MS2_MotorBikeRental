using System;

namespace MotorBikeRental.DTOs.ResponseDTO
{
    public class BikeUnitResonseDTO
{
    public string RegistrationNumber { get; set; }  
    public int Year { get; set; }  
   public List <BikeImageResponseDTO> Images {get;set;}
    public int Status { get; set; } =0; 
}

}



using System;
using System.Collections.Generic;
using MotorBikeRental.DTOs.RequestDTO;

namespace MotorBikeRental.DTOs.RequestDTO
{
    public class BikeRequestDTO
{
     
   // public int BikeId { get; set; }  
    public string Model { get; set; }  
    public string Brand { get; set; }  
    public decimal Rent { get; set; }  
    public List<BikeUnitRequestDTO> Units { get; set; }

}


}


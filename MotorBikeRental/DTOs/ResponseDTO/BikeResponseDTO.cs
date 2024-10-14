using System;

namespace MotorBikeRental.DTOs.ResponseDTO
{
public class BikeResponseDTO
{
    public int BikeId { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public decimal Rent { get; set; }
    public string RegistrationNumber {get;set;}

    public List <BikeImageResponseDTO> Images{get;set;}

}

}



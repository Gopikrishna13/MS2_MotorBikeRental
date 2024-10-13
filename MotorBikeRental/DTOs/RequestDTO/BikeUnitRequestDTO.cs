using System;
using MotorBikeRental.DTOs.RequestDTO;
namespace MotorBikeRental.DTOs.RequestDTO
{

public class BikeUnitRequestDTO
{
   // public int BikeId{get;set;}

    public string RegistrationNumber { get; set; }  // Registration number of the bike unit
    public int Year { get; set; }  // Year of manufacture for the bike unit
    public List <BikeImageRequestDTO> Images { get; set; }  // List of image URLs for the bike unit
    public int Status { get; set; }=0; 
}

}
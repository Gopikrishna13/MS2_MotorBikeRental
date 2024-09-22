using System;

namespace MotorBikeRental.DTOs.ResponseDTO
{
public class BikeResponseDTO
{
    public int BikeId { get; set; }
    public string BikeName { get; set; }
    public int Rent { get; set; }
    public string RegNo { get; set; }
    public int Status { get; set; }

}
}



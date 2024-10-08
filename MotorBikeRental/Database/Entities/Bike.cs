using System;
using System.Collections.Generic;
namespace MotorBikeRental.Database.Entities
{

    
public class Bike
{
    public int BikeId { get; set; }  // Unique identifier for the bike model
    public string Model { get; set; }  // Model name of the bike
    public string Brand { get; set; }  // Brand of the bike (e.g., Yamaha, Kawasaki)
    public decimal Rent { get; set; }  // Rental price per day for the bike
    public List<BikeUnit> Units { get; set; }  // List of units for this bike model



}

}


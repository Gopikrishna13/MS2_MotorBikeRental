using System;

namespace MotorBikeRental.Database.Entities
{
    public class BikeUnit
{
 public string RegistrationNumber { get; set; }  
    public int Year { get; set; }  
    public List<string> Images { get; set; }  
    public int Status { get; set; } =0; 
}


}


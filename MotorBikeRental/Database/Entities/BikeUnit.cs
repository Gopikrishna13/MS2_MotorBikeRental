using System;

namespace MotorBikeRental.Database.Entities
{
    public class BikeUnit
{
  //  public int BikeId{get;set;}
    public string RegistrationNumber { get; set; }  
    public int Year { get; set; }  
   public List <BikeImages> Images {get;set;}
    public int Status { get; set; } =0; 
}


}


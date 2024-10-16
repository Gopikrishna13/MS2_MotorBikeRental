using System;

namespace MotorBikeRental.Database.Entities
{
    public class RentalHistory
{
    
    public int BikeId{get;set;}
    public string RegistrationNumber{get;set;}
    public DateTime RentedDate { get; set; }  
    public DateTime? To { get; set; } 
    public DateTime Due {get;set;} 
   // public decimal RentalAmount { get; set; } 
}


}



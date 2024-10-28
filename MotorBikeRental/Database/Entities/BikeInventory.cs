using System;

namespace MotorBikeRental.Database.Entities
{


public class BikeInventory
{
   public int BikeId{get;set;}
   public string Brand {get;set;}
   public string Model{get;set;}
   public string RegistrationNumber{get;set;}
   public DateTime RentedDate{get;set;}
   public DateTime ReturnDate{get;set;}
   public string Status{get;set;}
}


}



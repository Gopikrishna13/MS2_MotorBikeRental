using System;

namespace MotorBikeRental.Database.Entities
{
public class ReturnedBikes
{
   public int ReturnId{get;set;}
    public int UserId{get;set;}
    public int BikeId{get;set;}
    public string RegistrationNumber{get;set;}
    public DateTime RentedDate {get;set;}
    public DateTime To{get;set;}
    public DateTime Due{get;set;}
    public string Status{get;set;}
}

}


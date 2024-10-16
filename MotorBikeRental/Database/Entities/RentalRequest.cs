using System;

namespace MotorBikeRental.Database.Entities
{
    public class RentalRequest
{
    public int RequestId{get;set;}
    public int UserId{get;set;}
    public int BikeId{get;set;}
    public DateTime RentedDate {get;set;}
    public DateTime ReturnDate{get;set;}
    public string Status{get;set;}="Waiting";
}

}



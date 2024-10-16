using System;

namespace MotorBikeRental.DTOs.RequestDTO
{


public class RentalRequestDTO
{
   
    public int UserId{get;set;}
    public int BikeId{get;set;}
    public string RegistrationNumber{get;set;}
    public DateTime RentedDate {get;set;}
    public DateTime ReturnDate{get;set;}
    public string Status{get;set;}="Waiting";

}


}


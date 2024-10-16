using System;

namespace MotorBikeRental.DTOs.RequestDTO
{
    public class ReturnedBikesDTO
{
    public int RequestId{get;set;}
    public int UserId{get;set;}
    public int BikeId{get;set;}
    public string RegistrationNumber{get;set;}
    public DateTime From {get;set;}
    public DateTime To{get;set;}
    public DateTime Due{get;set;}
    public string Status{get;set;}
}


}


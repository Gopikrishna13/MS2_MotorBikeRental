using System;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;
using System.Collections.Generic;
using MotorBikeRental.DTOs.RequestDTO;

namespace MotorBikeRental.IRepository

{
    public interface IRentalRepository
{
Task <bool> RentalRequest(RentalRequest request);
Task <List<RentalRequest>> AllRequest();
Task <bool> UpdateRequest(int code,int Id);
Task <List<ReturnedBikes>> AllReturnBike();
 Task<bool> UpdateReturn(int Id);
 Task <List<ReturnedBikes>> PendingByUser(int Id);
 Task <List<ReturnedBikes>> ReturnByUser(int Id);
 Task <decimal> Revenue();
 Task <bool> CheckAvailability(string registrationNumber,DateTime reqdate,DateTime retdate);
}


}

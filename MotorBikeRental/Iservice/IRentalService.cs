using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.Database.Entities;

namespace MotorBikeRental.Iservice
{
public interface IRentalService
{
Task <bool> RentalRequest(RentalRequestDTO requestDTO);
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



using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
using MotorBikeRental.Database.Entities;

namespace MotorBikeRental.Service
{
    public class RentalService:IRentalService
{

     private readonly IRentalRepository _rentalRepository;

    public RentalService(IRentalRepository rentalRepository)
    {
        _rentalRepository=rentalRepository;
    }

public async Task <bool> RentalRequest(RentalRequestDTO requestDTO)
{

var rental=new RentalRequest
{
    UserId=requestDTO.UserId,
    BikeId=requestDTO.BikeId,
    RegistrationNumber=requestDTO.RegistrationNumber,
    RentedDate=requestDTO.RentedDate,
    ReturnDate=requestDTO.ReturnDate,
    Status=requestDTO.Status

};

var data=await _rentalRepository.RentalRequest(rental);
return data;



}
 public async Task <List<RentalRequest>> AllRequest()
 {
    var data=await _rentalRepository.AllRequest();

    if(data == null)
    {
        throw new Exception("No Data Found!");
    }

return data.ToList();
 }

 public async Task <bool> UpdateRequest(int code,int Id)
 {
    var data=await _rentalRepository.UpdateRequest(code,Id);
   return data;

 }

 public async Task <List<ReturnedBikes>> AllReturnBike()
 {

    var data=await _rentalRepository.AllReturnBike();
    if(data == null)
    {
        throw new Exception("No Data Found!");
    }
    return data.ToList();

 }

 public async Task<bool> UpdateReturn(int Id)
 {
    var data=await _rentalRepository.UpdateReturn(Id);
    if(data == null)
    {
        throw new Exception("No Data Found!");
    }
    return data;
 }

 public async Task <List<ReturnedBikes>> PendingByUser(int Id)
 {
    var data=await _rentalRepository.PendingByUser(Id);
    return data.ToList();
 }

  public async Task <List<ReturnedBikes>> ReturnByUser(int Id)
 {
    var data=await _rentalRepository.ReturnByUser(Id);
    return data.ToList();
 }

 public async  Task <decimal> Revenue()
 {
     var data=await _rentalRepository.Revenue();
     return data;
 }

 public async Task <bool> CheckAvailability(string registrationNumber,DateTime reqdate,DateTime retdate)
 {
    var data=await _rentalRepository.CheckAvailability(registrationNumber,reqdate,retdate);
    return data;
 }

}

}



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

}

}



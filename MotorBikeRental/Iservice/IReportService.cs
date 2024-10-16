using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;
using MotorBikeRental.Database.Entities;


namespace MotorBikeRental.Iservice
{


public interface IReportService
{

    Task <List<CustomerRentalReport>> GetcustomerReport();
    Task <List<BikeInventory>> GetInventoryReport();
    Task  <List<AllBikes>> FrequentRent();

}


}



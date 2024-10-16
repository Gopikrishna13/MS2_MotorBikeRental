using System;
using MotorBikeRental.Database.Entities;
namespace MotorBikeRental.IRepository
{
public interface IReportRepository
{
Task <List<CustomerRentalReport>> GetcustomerReport();
 Task <List<BikeInventory>> GetInventoryReport();
  Task  <List<AllBikes>> FrequentRent();
}
}



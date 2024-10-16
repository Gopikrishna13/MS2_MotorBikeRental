using System;
using MotorBikeRental.Database.Entities;
using MotorBikeRental.IRepository;
using MotorBikeRental.Iservice;
namespace MotorBikeRental.Service
{
    public class ReportService:IReportService
{

    private readonly IReportRepository _reportrepository;

    public ReportService(IReportRepository reportrepository)
    {
        _reportrepository=reportrepository;
    }

    public async Task <List<CustomerRentalReport>> GetcustomerReport()
    {
   var data=await _reportrepository.GetcustomerReport();
   return data.ToList();
    }

    public async  Task <List<BikeInventory>> GetInventoryReport()
    {

        var data=await _reportrepository.GetInventoryReport();
        return data.ToList();
    }
 public async  Task  <List<AllBikes>> FrequentRent()
 {
    var data=await _reportrepository.FrequentRent();
    return data.ToList();
 }
}

}



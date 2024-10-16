using System;

namespace MotorBikeRental.Database.Entities
{

    public class CustomerRentalReport
{
    public int UserId{get;set;}
    public string UserName{get;set;}
    public List<RentalHistory> RentalHistories {get;set;}

}

}



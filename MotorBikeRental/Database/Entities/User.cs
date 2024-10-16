using System;
using System.Collections.Generic;

namespace MotorBikeRental.Database.Entities
{
public class User
{
    public int UserId { get; set; }  
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string NIC { get; set; }
    public string Email { get; set; }
    public string LicenseNumber { get; set; }
    public List <RentalHistory> RentalHistories {get;set;}
}

}


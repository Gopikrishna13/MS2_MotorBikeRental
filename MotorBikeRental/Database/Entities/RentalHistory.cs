using System;

namespace MotorBikeRental.Database.Entities
{
    public class RentalHistory
{
    public int RentalHistoryId { get; set; }  
    public int RentalId { get; set; }  
    public DateTime RentalDate { get; set; }  
    public DateTime? ReturnDate { get; set; }  
    public decimal RentalAmount { get; set; } 
}


}



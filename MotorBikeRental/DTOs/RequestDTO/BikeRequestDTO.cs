using System;
using System.Collections.Generic;

namespace MotorBikeRental.DTOs.RequestDTO
{
    public class BikeRequestDTO
{
     
                   public string BikeName {get;set;}
                  public int Rent {get;set;}
                 public List <string>  RegNo {get;set;}
                 public  int Status {get;set;}=0;

}


}


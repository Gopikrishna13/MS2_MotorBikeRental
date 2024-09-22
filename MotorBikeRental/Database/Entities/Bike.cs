using System;
using System.Collections.Generic;
namespace MotorBikeRental.Database.Entities;

public class Bike
{
                public  int  BikeId {get;set;}
                public   string BikeName {get;set;}
                public  int Rent {get;set;}
                public List <string> RegNo {get;set;}
                public int Status {get;set;}=0;

}

using ParkingLot2.Classes;

namespace ParkingLot2.Interfaces;


public interface IRateCalculationStratergy
{
    double CalculateRate(Ticket ticket);
}
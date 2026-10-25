using ParkingLot2.Interfaces;

namespace ParkingLot2.Classes.RateCalculationStratergy;

public class HourlyStratergy : IRateCalculationStratergy
{
    public double CalculateRate(Ticket ticket)
    {
        int hours = DateTime.Now.Hour - ticket.InTime.Hour;
        return hours * 100;
    }
}
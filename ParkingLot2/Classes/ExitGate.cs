using ParkingLot2.Interfaces;

namespace ParkingLot2.Classes;

public class ExitGate
{
    public double CalculateFair(IRateCalculationStratergy rateStrategy, Ticket ticket)
    {
        return rateStrategy.CalculateRate(ticket);
    }
    
    public bool ProcessPayment(IPaymentStrategy paymentStragey, double amount)
    {
        return paymentStragey.Pay(amount);
    }
}
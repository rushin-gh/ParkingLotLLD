namespace ParkingLot2.Interfaces;

public interface IPaymentStrategy
{
    bool Pay(double amount);
}
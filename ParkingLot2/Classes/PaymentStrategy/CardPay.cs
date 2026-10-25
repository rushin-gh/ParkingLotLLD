using ParkingLot2.Interfaces;

namespace ParkingLot2.Classes.PaymentStrategy;

public class CardPay : IPaymentStrategy
{
    public bool Pay(double amount)
    {
        bool paymentStatus = true;
        // if failure -> paymentstatus = false;
        return paymentStatus;
    }
}
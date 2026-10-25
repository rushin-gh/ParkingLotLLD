using ParkingLot2.Enums;
using ParkingLot2.Interfaces;

namespace ParkingLot2.Classes;

public sealed class ParkingManager
{
    private static readonly Lazy<ParkingManager> _instance = new(() => new ParkingManager());

    public static ParkingManager Instance => _instance.Value;

    private readonly ParkingLot parkingLot;
    private readonly IParkingSpotFindingStrategy _spotStrategy = new NearestParkingSpot();

    private ParkingManager()
    {
        parkingLot = new ParkingLot(CreateDemoSpots());
    }

    public IReadOnlyList<ParkingSpot> Spots => parkingLot.ParkingSpots;

    // No global lock: each spot is claimed with a compare-and-swap, so gates only
    // compete when they want the exact same spot, and the loser moves to the next one.
    public Ticket Park(Vehicle veh)
    {
        var spot = parkingLot.TryHoldSpot(_spotStrategy, veh)
                   ?? throw new InvalidOperationException($"No free {veh.VehicleType} spot");

        Ticket ticket;
        try
        {
            ticket = parkingLot.entryGate.IssueTicket(veh, spot);
        }
        catch
        {
            spot.TryCancelHold();   // give the spot back if the ticket could not be issued
            throw;
        }

        spot.TryConfirm();
        // ticket handed over to driver
        return ticket;
    }

    public bool Unpark(IRateCalculationStratergy rateStrat, IPaymentStrategy payStrat, Ticket ticket)
    {
        // Only one exit attempt per ticket at a time; a repeat or closed ticket is rejected.
        if (!ticket.TryBeginExit())
            return false;

        bool paid;
        try
        {
            var fair = parkingLot.exitGate.CalculateFair(rateStrat, ticket);
            paid = parkingLot.exitGate.ProcessPayment(payStrat, fair);
        }
        catch
        {
            ticket.TryCancelExit();
            throw;
        }

        if (!paid)
        {
            ticket.TryCancelExit();  // let the driver retry payment
            return false;
        }

        parkingLot.VacateSpot(ticket.Spot);
        ticket.TryClose();
        return true;
    }

    // Demo layout: 2 floors x 2 rows x 5 cols, half cars, half bikes.
    private static IEnumerable<ParkingSpot> CreateDemoSpots()
    {
        var id = 1;
        for (var floor = 0; floor < 2; floor++)
        for (var row = 0; row < 2; row++)
        for (var col = 0; col < 5; col++)
        {
            yield return new ParkingSpot
            {
                Id = id++,
                floor = floor,
                row = row,
                col = col,
                vehicleType = row == 0 ? VehicleType.Car : VehicleType.Bike
            };
        }
    }
}

namespace ParkingLot2.Classes;

public class EntryGate
{
    // Shared across all gates. Will come from the DB primary key later.
    private static int _lastTicketId;

    public Ticket IssueTicket(Vehicle veh, ParkingSpot spot)
    {
        var ticket = new Ticket()
        {
            Tid = Interlocked.Increment(ref _lastTicketId),
            InTime = DateTime.Now,
            Spot = spot,
            Veh = veh
        };
        return ticket;
    }
}

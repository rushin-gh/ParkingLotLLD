namespace ParkingLot2.Classes;

public class EntryGate
{
    // Shared across all gates. Will come from the DB primary key later.
    private static int _lastTicketId;
    private static readonly object _idLock = new();

    public Ticket IssueTicket(Vehicle veh, ParkingSpot spot)
    {
        return new Ticket()
        {
            Tid = NextTicketId(),
            InTime = DateTime.Now,
            Spot = spot,
            Veh = veh
        };
    }

    private static int NextTicketId()
    {
        lock (_idLock)
        {
            return ++_lastTicketId;
        }
    }
}

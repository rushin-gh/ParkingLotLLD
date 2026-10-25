using ParkingLot2.Enums;

namespace ParkingLot2.Classes;

public class Ticket
{
    public int Tid { get; init; }

    public DateTime InTime { get; init; }

    public required ParkingSpot Spot { get; init; }

    public required Vehicle Veh { get; init; }

    private int _status = (int)TicketStatus.Active;

    public TicketStatus Status => (TicketStatus)Volatile.Read(ref _status);

    // Active -> Exiting. Stops the same ticket being processed twice at once.
    public bool TryBeginExit() => TryMove(TicketStatus.Active, TicketStatus.Exiting);

    // Exiting -> Active, e.g. when payment fails so the driver can retry.
    public bool TryCancelExit() => TryMove(TicketStatus.Exiting, TicketStatus.Active);

    // Exiting -> Closed, after payment succeeded and the spot was freed.
    public bool TryClose() => TryMove(TicketStatus.Exiting, TicketStatus.Closed);

    private bool TryMove(TicketStatus from, TicketStatus to) =>
        Interlocked.CompareExchange(ref _status, (int)to, (int)from) == (int)from;
}

using ParkingLot2.Enums;

namespace ParkingLot2.Classes;

public class ParkingSpot
{
    public int Id { get; init; }

    public int floor { get; init; }

    public int row { get; init; }

    public int col { get; init; }

    public VehicleType vehicleType { get; init; }

    // Stored as int so it can be changed atomically with Interlocked.
    private int _status = (int)ParkingSpotStatus.Free;

    public ParkingSpotStatus Status => (ParkingSpotStatus)Volatile.Read(ref _status);

    // Free -> Hold. Only one thread can win this for a given spot.
    public bool TryHold() => TryMove(ParkingSpotStatus.Free, ParkingSpotStatus.Hold);

    // Hold -> Occupied, once the ticket has been issued.
    public bool TryConfirm() => TryMove(ParkingSpotStatus.Hold, ParkingSpotStatus.Occupied);

    // Hold -> Free, if issuing the ticket failed.
    public bool TryCancelHold() => TryMove(ParkingSpotStatus.Hold, ParkingSpotStatus.Free);

    // Occupied -> Free. Returns false if the spot was not occupied (e.g. a double release).
    public bool TryRelease() => TryMove(ParkingSpotStatus.Occupied, ParkingSpotStatus.Free);

    private bool TryMove(ParkingSpotStatus from, ParkingSpotStatus to) =>
        Interlocked.CompareExchange(ref _status, (int)to, (int)from) == (int)from;
}

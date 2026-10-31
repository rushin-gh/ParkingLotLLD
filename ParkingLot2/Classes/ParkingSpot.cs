using ParkingLot2.Enums;

namespace ParkingLot2.Classes;

public class ParkingSpot
{
    public int Id { get; init; }

    public int floor { get; init; }

    public int row { get; init; }

    public int col { get; init; }

    public VehicleType vehicleType { get; init; }

    // Each spot has its own lock, so threads only wait on each other
    // when they want the same spot.
    private readonly object _lock = new();

    private ParkingSpotStatus _status = ParkingSpotStatus.Free;

    public ParkingSpotStatus Status
    {
        get { lock (_lock) return _status; }
    }

    // Free -> Hold. Only one thread can win this for a given spot.
    public bool TryHold() => TryMove(ParkingSpotStatus.Free, ParkingSpotStatus.Hold);

    // Hold -> Occupied, once the ticket has been issued.
    public bool TryConfirm() => TryMove(ParkingSpotStatus.Hold, ParkingSpotStatus.Occupied);

    // Hold -> Free, if issuing the ticket failed.
    public bool TryCancelHold() => TryMove(ParkingSpotStatus.Hold, ParkingSpotStatus.Free);

    // Occupied -> Free. Returns false if the spot was not occupied (e.g. a double release).
    public bool TryRelease() => TryMove(ParkingSpotStatus.Occupied, ParkingSpotStatus.Free);

    // The check and the update happen inside one lock, so no other thread can change
    // the status in between.
    private bool TryMove(ParkingSpotStatus from, ParkingSpotStatus to)
    {
        lock (_lock)
        {
            if (_status != from)
                return false;

            _status = to;
            return true;
        }
    }
}

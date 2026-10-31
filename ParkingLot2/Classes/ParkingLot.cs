using ParkingLot2.Interfaces;

namespace ParkingLot2.Classes;

public class ParkingLot
{
    // The list itself never changes after construction, so reading it from many threads is safe.
    // Only each spot's status changes, and that is guarded by each spot's own lock.
    private readonly IReadOnlyList<ParkingSpot> _parkingSpots;

    public EntryGate entryGate { get; }

    public ExitGate exitGate { get; }

    public IReadOnlyList<ParkingSpot> ParkingSpots => _parkingSpots;

    public ParkingLot(IEnumerable<ParkingSpot> spots)
    {
        _parkingSpots = spots.ToList().AsReadOnly();
        entryGate = new EntryGate();
        exitGate = new ExitGate();
    }

    // Walks the ranked spots and holds the first one this thread wins.
    // Returns null when no matching spot is free.
    public ParkingSpot? TryHoldSpot(IParkingSpotFindingStrategy spotStrategy, Vehicle veh)
    {
        foreach (var spot in spotStrategy.RankSpots(_parkingSpots, veh))
        {
            if (spot.TryHold())
                return spot;
        }

        return null;
    }

    public bool VacateSpot(ParkingSpot spot) => spot.TryRelease();
}

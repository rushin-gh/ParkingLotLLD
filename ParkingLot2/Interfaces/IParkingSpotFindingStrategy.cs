using ParkingLot2.Classes;

namespace ParkingLot2.Interfaces;

public interface IParkingSpotFindingStrategy
{
    // Returns candidate spots, best first. The caller tries to hold each one in turn,
    // because another gate may grab a spot between ranking and holding.
    IEnumerable<ParkingSpot> RankSpots(IReadOnlyList<ParkingSpot> spots, Vehicle veh);
}

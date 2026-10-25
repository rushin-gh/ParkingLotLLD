using ParkingLot2.Enums;
using ParkingLot2.Interfaces;

namespace ParkingLot2.Classes;

public class FarthestParkingSpot : IParkingSpotFindingStrategy
{
    public IEnumerable<ParkingSpot> RankSpots(IReadOnlyList<ParkingSpot> spots, Vehicle veh)
    {
        return spots
            .Where(s => s.vehicleType == veh.VehicleType && s.Status == ParkingSpotStatus.Free)
            .OrderByDescending(s => s.floor).ThenByDescending(s => s.row).ThenByDescending(s => s.col);
    }
}

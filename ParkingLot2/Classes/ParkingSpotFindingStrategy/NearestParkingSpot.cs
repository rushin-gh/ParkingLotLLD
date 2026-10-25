using ParkingLot2.Enums;
using ParkingLot2.Interfaces;

namespace ParkingLot2.Classes;

public class NearestParkingSpot : IParkingSpotFindingStrategy
{
    public IEnumerable<ParkingSpot> RankSpots(IReadOnlyList<ParkingSpot> spots, Vehicle veh)
    {
        return spots
            .Where(s => s.vehicleType == veh.VehicleType && s.Status == ParkingSpotStatus.Free)
            .OrderBy(s => s.floor).ThenBy(s => s.row).ThenBy(s => s.col);
    }
}

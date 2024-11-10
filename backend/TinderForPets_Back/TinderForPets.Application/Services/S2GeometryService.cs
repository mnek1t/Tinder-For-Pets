using Google.Common.Geometry;

namespace TinderForPets.Application.Services
{
    public class S2GeometryService
    {
        // define hierarchy
        // Level 10 covers approximately 10 km², useful for city-level proximity.
        private const int DefaultS2Level = 10;

        public ulong GetS2CellId(double latitude, double longitude, int level = DefaultS2Level)
        {
            var latLng = S2LatLng.FromDegrees(latitude, longitude);
            var cellId = S2CellId.FromLatLng(latLng).ParentForLevel(level);
            return cellId.Id;
        }

        public List<ulong> GetNearbyCellIds(double latitude, double longitude, double radiusKm, int level = DefaultS2Level)
        {
            var latLng = S2LatLng.FromDegrees(latitude, longitude);
            var centerCell = S2CellId.FromLatLng(latLng).ParentForLevel(level);

            // Define a cap (circle) around the center cell to find nearby cells
            S2Cap cap = S2Cap.FromAxisAngle(centerCell.ToPoint(), S1Angle.FromRadians(radiusKm / 6371)); // 6371 is Earth radius in km
            S2RegionCoverer coverer = new S2RegionCoverer { MaxCells = 8 }; // Adjust MaxCells for more or fewer nearby cells

            var nearbyCells = new List<S2CellId>();
            coverer.GetCovering(cap, nearbyCells);

            return nearbyCells.Select(cell => cell.Id).ToList();
        }
    }
}

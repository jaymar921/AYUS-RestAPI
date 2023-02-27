namespace AYUS_RestAPI.ASP.Models.Request
{
    public class MapLocation
    {
        public string UUID { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? AdditionData { get; set; } = string.Empty;
    }
}

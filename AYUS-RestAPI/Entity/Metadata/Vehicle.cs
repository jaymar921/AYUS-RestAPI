using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata
{
    public class Vehicle
    {
        public string UUID { get; set; } = string.Empty;
        [Key]
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get ; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}

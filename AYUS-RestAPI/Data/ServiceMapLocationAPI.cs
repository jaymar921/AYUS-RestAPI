using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Data
{
    public class ServiceMapLocationAPI
    {
        [Key]
        public string SessionID { get; set; } = string.Empty;
        public double ClientLocLat { get; set; }
        public double ClientLocLon { get; set;}
        public double MechanicLocLat { get; set; }
        public double MechanicLocLon { get;set; }
    }
}

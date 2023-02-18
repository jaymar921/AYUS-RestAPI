using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYUS_RestAPI.Entity.Metadata
{
    public class Vehicle
    {
        [Key]
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get ; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}

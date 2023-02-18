using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata.Mechanic
{
    public class Service
    {
        [Key]
        public string ServiceID { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata
{
    public class PersonalInformation
    {
        [Key] public string UUID { get; set; } = Guid.NewGuid().ToString();
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; } = DateTime.UtcNow;
        public string Address { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime Expiry { get; set; } = DateTime.UtcNow;

    }
}

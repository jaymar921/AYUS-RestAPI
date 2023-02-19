using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class PersonalInformationModel
    {
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        public string Lastname { get; set; } = string.Empty;
        [Required]
        public string Contact { get; set; } = string.Empty;
        [Required]
        public DateTime Birthdate { get; set; } = DateTime.Now;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string LicenseNumber { get; set; } = string.Empty;
        [Required]
        public DateTime Expiry { get; set; } = DateTime.Now;

    }
}

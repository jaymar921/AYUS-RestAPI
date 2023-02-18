using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata
{
    public class Credential
    {
        [Key]
        public string UUID { get ; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

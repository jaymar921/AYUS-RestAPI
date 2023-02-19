using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class CredentialModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}

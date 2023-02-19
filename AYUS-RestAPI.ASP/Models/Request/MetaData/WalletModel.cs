using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class WalletModel
    {
        [Required]
        public double Balance { get; set; } = 0;
        
        public string Pincode { get; set; } = string.Empty;
    }
}

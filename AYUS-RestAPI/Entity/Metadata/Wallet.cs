using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata
{
    public class Wallet
    {
        [Key]
        public string UUID { get; set; } = string.Empty;
        public double Balance { get; set; } = 0;
        public string Pincode { get; set; } = "000000";
    }
}

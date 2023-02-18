using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata.Mechanic
{
    public class Billing
    {
        [Key]
        public string BillingID { get; set; } = Guid.NewGuid().ToString();
        public string ShopID { get; set; } = string.Empty;
        public DateTime BillingDate { get; set; } = DateTime.UtcNow;
        public double ServiceFee { get; set; } = 0;
        public string ServiceRemark { get; set; } = string.Empty;
    }
}

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class BillingModel
    {
        public string ShopID { get; set; } = string.Empty;
        public double ServiceFee { get; set; } = 0;
        public string ServiceRemark { get; set; } = string.Empty;
    }
}

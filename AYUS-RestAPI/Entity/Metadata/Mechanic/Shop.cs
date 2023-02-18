using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata.Mechanic
{
    public class Shop
    {
        [Key]
        public string ShopID { get; set; } = Guid.NewGuid().ToString();
        public string ShopName { get; set; } = string.Empty;
        public string ShopDescription { get; set; } = string.Empty;

        private List<Billing> billings= new List<Billing>();
        private List<ServiceOffer> offerings= new List<ServiceOffer>();

        public Shop() { }
        public Shop(List<Billing> bills, List<ServiceOffer> offerings)
        {
            billings = bills;
            this.offerings = offerings;
        }

        public List<Billing> Billings
        {
            get { return billings; }
        }

        public List<ServiceOffer> ServiceOffers { get { return offerings; } }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata.Mechanic
{
    public class ServiceOffer
    {
        [Key]
        public string ServiceID { get; set; } = Guid.NewGuid().ToString();
        public string ShopID { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public string ServiceExpertise { get; set; } = string.Empty;

        private Service Service = new Service();

        public ServiceOffer() { }
        public ServiceOffer(Service service)
        {
            this.Service = service;
        }

        public void setService(Service service)
        {
            this.Service = service;
        }

        public Service getService() { return Service; }
    }
}

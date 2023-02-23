using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata.Mechanic
{
    public class ServiceOffer
    {
        [Key]
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public string ShopID { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public string ServiceExpertise { get; set; } = string.Empty;
        public string ServiceID { get; set; } = string.Empty;

        private Service Service = new();

        public ServiceOffer() { }
        public ServiceOffer(Service service)
        {
            this.Service = service;
            ServiceID = service.ServiceID;
        }

        public void SetService(Service service)
        {
            this.Service = service;
            ServiceID = service.ServiceID;
        }

        public Service GetService() { return Service; }
    }
}

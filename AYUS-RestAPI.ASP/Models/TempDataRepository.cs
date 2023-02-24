using AYUS_RestAPI.Data;

namespace AYUS_RestAPI.ASP.Models
{
    public class TempDataRepository
    {
        List<ServiceRequest> serviceRequests = new List<ServiceRequest>();

        public List<ServiceRequest> GetServiceRequests()
        {
            return serviceRequests;
        }
    }
}

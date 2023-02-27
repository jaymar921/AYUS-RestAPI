using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Data;

namespace AYUS_RestAPI.ASP.Models
{
    public class TempDataRepository
    {
        List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
        List<MapLocation> mapLocations = new List<MapLocation>();

        public List<ServiceRequest> GetServiceRequests()
        {
            return serviceRequests;
        }

        public List<MapLocation> GetMapLocations()
        {
            return mapLocations;
        }
    }
}

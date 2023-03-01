using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Data;
using AYUS_RestAPI.Entity.Metadata;
using System.Collections;

namespace AYUS_RestAPI.ASP.Models
{
    public class TempDataRepository
    {
        List<ServiceRequest> serviceRequests = new List<ServiceRequest>();
        List<MapLocation> mapLocations = new List<MapLocation>();
        List<SessionFlag> SessionFlags = new List<SessionFlag>();

        public List<ServiceRequest> GetServiceRequests()
        {
            return serviceRequests;
        }

        public List<MapLocation> GetMapLocations()
        {
            return mapLocations;
        }

        public List<SessionFlag> GetSessionFlags() 
        { 
            return SessionFlags;
        }
    }
}

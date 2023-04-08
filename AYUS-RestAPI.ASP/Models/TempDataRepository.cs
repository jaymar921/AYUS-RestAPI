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
        List<ReportModel> reportModels = new List<ReportModel>();
        public int ServicePrice { get; set; } = 25;

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

        public List<ReportModel> GetReports()
        {
            return reportModels;
        }
    }
}

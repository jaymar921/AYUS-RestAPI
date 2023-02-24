using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using AYUS_RestAPI.Data;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : Controller
    {
        private JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        private readonly DataRepository dataRepository;
        private readonly TempDataRepository tempDataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public ServiceRequestController(DataRepository dataRepository, TempDataRepository temp)
        {
            this.dataRepository = dataRepository;
            tempDataRepository = temp;
        }

        [HttpGet]
        public JsonResult GetRequest()
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the MechanicUUID at the header of the request" }, options);
            }

            Request.Headers.TryGetValue("ServiceRequestUUID", out var serviceRequestUUID);
            User? user = dataRepository.GetUser(mechanicUUID.ToString());

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from MechanicUUID specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "User is found but the role is not a Mechanic" }, options);
            }
            List<ServiceRequest> requests = tempDataRepository.GetServiceRequests().Where(s => s.Recepient == mechanicUUID.ToString() || s.RequestID == serviceRequestUUID.ToString()).ToList();

            return Json(new { Status = 200, Message = $"Services found for mechanic '{user.Credential.Username}'", ServiceRequests = requests
            }, options);
        }


        [HttpPost]
        public JsonResult PostRequest(ServiceRequest serviceRequest)
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }



            User? user = dataRepository.GetUser(serviceRequest.Recepient);

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from Recepient specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "Recepient is found but the role is not a Mechanic" }, options);
            }

            if(dataRepository.GetUser(serviceRequest.Requestor) == null)
            {
                return Json(new { Status = 404, Message = "No data found from Requestor specified" }, options);
            }

            tempDataRepository.GetServiceRequests().Add(serviceRequest);
            

            return Json(new
            {
                Status = 201,
                Message = $"A request has been saved for mechanic '{user.Credential.Username}'"
            }, options);
        }

        [HttpDelete]
        public JsonResult DeleteRequest()
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("ServiceRequestUUID", out var serviceRequestUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the ServiceRequestUUID at the header of the request" }, options);
            }


            var data = tempDataRepository.GetServiceRequests().FirstOrDefault(s => s.RequestID == serviceRequestUUID.ToString());


            if(data != null)
            {
                tempDataRepository.GetServiceRequests().Remove(data);
            }


            return Json(new
            {
                Status = 200,
                Message = "A request has been removed"
            }, options);
        }
    }
}

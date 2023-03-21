using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using AYUS_RestAPI.Data;
using AYUS_RestAPI.ASP.Classes;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestController : Controller
    {
        private JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        private readonly DataRepository dataRepository;
        private readonly TempDataRepository tempDataRepository;
        public ServiceRequestController(DataRepository dataRepository, TempDataRepository temp)
        {
            this.dataRepository = dataRepository;
            tempDataRepository = temp;
        }

        [HttpGet]
        public JsonResult GetRequest()
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID);
           

            Request.Headers.TryGetValue("ClientUUID", out var clientUUID);
            

            Request.Headers.TryGetValue("ServiceRequestUUID", out var serviceRequestUUID);
            User? user = dataRepository.GetUser(mechanicUUID.ToString()) ?? dataRepository.GetUser(clientUUID.ToString());

            if (user != null)
            {
                if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC) && user.PersonalInformation.UUID == mechanicUUID.ToString())
                {
                    return Json(new { Status = 400, Message = "User[Mechanic] is found but the role is not a Mechanic" }, options);
                }
            }

            

            List<object> requests = new List<object>();
            tempDataRepository.GetServiceRequests().Where(s => (s.Recepient == mechanicUUID.ToString() || s.RequestID == serviceRequestUUID.ToString() || s.Requestor == clientUUID.ToString()) && s.Status.ToLower() != "declined").ToList().ForEach(
                req => {
                    requests.Add(new {
                        req.RequestID,
                        req.Requestor,
                        req.Recepient,
                        req.Contact,
                        FullName =  dataRepository.GetUser(req.Requestor)?.PersonalInformation.Firstname+ " " + dataRepository.GetUser(req.Requestor)?.PersonalInformation.Lastname,
                        req.Location,
                        req.Service,
                        req.Description,
                        req.Picture,
                        req.Vehicle,
                        dataRepository.GetService(req.Service)?.ServiceName,
                        req.Status,
                        req.NewStatus
                    });
            });
            

            return Json(new { Status = 200, Message = $"Services found for mechanic", ServiceRequests = requests
            }, options);
        }


        [HttpPost]
        public JsonResult PostRequest([FromBody] ServiceRequestModel model)
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);

            User? user = dataRepository.GetUser(model.Recepient);

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from Recepient specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "Recepient is found but the role is not a Mechanic" }, options);
            }

            if(dataRepository.GetUser(model.Requestor) == null)
            {
                return Json(new { Status = 404, Message = "No data found from Requestor specified" }, options);
            }
            ServiceRequest request = ServiceRequest.parse(model);
                
            tempDataRepository.GetServiceRequests().Add(request);
            dataRepository.AddLog(new Data.Logs { Info = $"A service request has been created for mechanic '{user.Credential.Username}'" });
            return Json(new
            {
                Status = 201,
                Message = $"A request has been saved for mechanic '{user.Credential.Username}'",
                Info = request
            }, options);
        }

        [HttpPut]
        public JsonResult PutRequest([FromBody] ServiceRequestModel model)
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);

            User? user = dataRepository.GetUser(model.Recepient);

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from Recepient specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "Recepient is found but the role is not a Mechanic" }, options);
            }

            if (dataRepository.GetUser(model.Requestor) == null)
            {
                return Json(new { Status = 404, Message = "No data found from Requestor specified" }, options);
            }
            ServiceRequest request = ServiceRequest.parse(model);

            ServiceRequest? requestFound = tempDataRepository.GetServiceRequests().FirstOrDefault(r => ((r.Recepient == request.Recepient && r.Requestor == request.Requestor) || r.RequestID == request.RequestID) && request.Status.ToLower() != "declined");

            if (requestFound == null)
            {
                return Json(new
                {
                    Status = 200,
                    Message = $"No data found'"
                }, options);
            }



            requestFound.Vehicle = model.Vehicle;
            requestFound.Status = model.NewStatus;
            requestFound.Contact = model.Contact;
            requestFound.Location = model.Location;
            requestFound.Service = model.Service;
            requestFound.Description = model.Description;
            requestFound.Picture = model.Picture;

            dataRepository.AddLog(new Data.Logs { Info = $"A service request has been updated for user '{user.Credential.Username}'" });
            return Json(new
            {
                Status = 200,
                Message = $"A request has been updated '{user.Credential.Username}'",
                Info = model
            }, options);
        }

        [HttpDelete]
        public JsonResult DeleteRequest()
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!Request.Headers.TryGetValue("ServiceRequestUUID", out var serviceRequestUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the ServiceRequestUUID at the header of the request" }, options);
            }


            var data = tempDataRepository.GetServiceRequests().FirstOrDefault(s => s.RequestID == serviceRequestUUID.ToString());


            if(data != null)
            {
                tempDataRepository.GetServiceRequests().Remove(data);
            }

            dataRepository.AddLog(new Data.Logs { Info = $"A Service request has been deleted, ID '{serviceRequestUUID}'" });
            return Json(new
            {
                Status = 200,
                Message = "A request has been removed"
            }, options);
        }
    }
}

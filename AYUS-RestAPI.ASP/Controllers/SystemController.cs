using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        private readonly DataRepository dataRepository;
        private readonly TempDataRepository tempDataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public SystemController(DataRepository data, TempDataRepository tempDataRepository)
        {
            dataRepository = data;
            this.tempDataRepository = tempDataRepository;
        }

        [Route("ResetDatabase")]
        [HttpDelete]
        public JsonResult ResetDatabase()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }
            dataRepository.ResetDatabase();
            return Json(new { Status = 200, Message = "Database Reset success" }, options);
        }

        [Route("Service")]
        [HttpPost]
        public JsonResult PostService(ServiceModel model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!TryValidateModel(model))
            {
                return Json(new { Status = 400, Message = "Invalid data format [Make sure that the body of the request is in correct format]" }, options);
            }

            Service service = new Service
            {
                ServiceDescription = model.ServiceDescription,
                ServiceName = model.ServiceName
            };

            dataRepository.AddService(service);

            return Json(new { Status = 201, Message = "Service was successfully added into the database", Info = service }, options);
        }

        [Route("Service")]
        [HttpPut]
        public JsonResult PutService(Service model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!TryValidateModel(model))
            {
                return Json(new { Status = 400, Message = "Invalid data format [Make sure that the body of the request is in correct format]" }, options);
            }

            if (dataRepository.GetService(model.ServiceID) == null)
            {
                return Json(new { Status = 404, Message = $"Service with ID '{model.ServiceID}' was not found in the database, use POST instead to register a new service." }, options);
            }

            Service service = new Service
            {
                ServiceID = model.ServiceID,
                ServiceDescription = model.ServiceDescription,
                ServiceName = model.ServiceName
            };

            dataRepository.UpdateService(service);

            return Json(new { Status = 201, Message = "Service was successfully updated from the database", Info = service }, options);
        }

        [Route("Service")]
        [HttpGet]
        public JsonResult GetService()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }


            List<Service> Services = dataRepository.GetAllServices();

            return Json(new { Status = 200, Message = "Provided the list of services found in the database", Services }, options);
        }


        [Route("Service")]
        [HttpDelete]
        public JsonResult DeleteService()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("ServiceID", out var serviceID))
            {
                return Json(new { Status = 401, Message = "Please specify the ServiceID at the header of the request" }, options);
            }

            Service? service = dataRepository.GetService(serviceID.ToString());

            if (service == null)
            {
                return Json(new { Status = 404, Message = "SerivceID provided does not seem to exist" }, options);
            }

            dataRepository.DeleteService(service);

            return Json(new { Status = 200, Message = "Service was successfully deleted from the database" }, options);
        }

        [Route("ServicePrice")]
        [HttpPut]
        public JsonResult PutServicePrice(int price)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            
            tempDataRepository.ServicePrice = price;

            return Json(new { Status = 201, Message = "ServicePrice was successfully updated from the server", NewPrice = price }, options);
        }

        [Route("ServicePrice")]
        [HttpGet]
        public JsonResult GetServicePrice()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            return Json(new { Status = 200, Message = "Service Price retrieved", tempDataRepository.ServicePrice }, options);
        }
    }
}

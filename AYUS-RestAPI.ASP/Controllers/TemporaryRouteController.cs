using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemporaryRouteController : Controller
    {
        private JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        private readonly DataRepository dataRepository;
        private readonly TempDataRepository tempDataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";

        public TemporaryRouteController(DataRepository dataRepository, TempDataRepository tempDataRepository)
        {
            this.dataRepository = dataRepository;
            this.tempDataRepository = tempDataRepository;
        }

        [HttpPut, HttpPost]
        [Route("MapLocation")]
        public JsonResult PostPutdata(MapLocation model)
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            MapLocation? mapLocation = tempDataRepository.GetMapLocations().FirstOrDefault(mapLoc => mapLoc.UUID == model.UUID);

            if (mapLocation == null)
            {
                tempDataRepository.GetMapLocations().Add(model);
                return Json(new { Status = 201, Message = "Registered MapLocation data" }, options);
            }
            else if(mapLocation.UUID != model.UUID)
            {
                tempDataRepository.GetMapLocations().Add(model);
                return Json(new { Status = 201, Message = "Registered MapLocation data" }, options);
            }
            else
            {
                mapLocation.Latitude = model.Latitude;
                mapLocation.Longitude = model.Longitude;
                mapLocation.AdditionData = model.AdditionData;
                
            }

            return Json(new { Status = 200, Message = "Updated MapLocation data" }, options);
        }

        [HttpGet]
        [Route("MapLocation")]
        public JsonResult Getdata()
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }
            if(!Request.Headers.TryGetValue("UUID", out var uuid))
            {
                return Json(new { Status = 401, Message = "Please specify the UUID at the header of the request" }, options);
            }

            MapLocation? mapLocation = tempDataRepository.GetMapLocations().FirstOrDefault(mapLoc => mapLoc.UUID == uuid.ToString());

            if (mapLocation == null)
            {
                return Json(new { Status = 404, Message = "No data found" }, options);
            }
            else if (mapLocation.UUID != uuid.ToString())
            {
                return Json(new { Status = 404, Message = "No data found" }, options);
            }

            return Json(new { Status = 200, Message = "Found data from the UUID specified", Data=mapLocation }, options);
        }
    }
}

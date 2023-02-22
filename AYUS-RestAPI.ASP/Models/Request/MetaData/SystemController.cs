using AYUS_RestAPI.Entity.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        private readonly DataRepository dataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public SystemController(DataRepository data)
        {
            dataRepository = data;
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
            return Json(new { Status = 200, Message = "Database Reset success"}, options);
        }
    }
}

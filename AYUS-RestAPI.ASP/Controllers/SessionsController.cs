using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Enumerations;
using AYUS_RestAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : Controller
    {
        private readonly DataRepository dataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public SessionsController(DataRepository data)
        {
            dataRepository = data;
        }
        [HttpGet]
        [Route("AvailableMechanics")]
        public JsonResult GetAvailableMechanics()
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


            List<AccountModel> users = new List<AccountModel>();
                
            dataRepository.GetAllUser().Where(u => u.AccountStatus.GetRole.Equals(Roles.MECHANIC)).ToList().ForEach(u => users.Add(u.ParseModel()));

            
            if (users.Count == 0)
            {
                return Json(new { Status = 404, Message = "No Available Mechanics" }, options);
            }
            return Json(users, options);
        }
    }
}

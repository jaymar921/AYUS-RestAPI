using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Utility;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly DataRepository dataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public AccountController(DataRepository data)
        {
            dataRepository = data;
        }

        
        [HttpGet]
        public JsonResult GetAccount()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if(apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }
            Request.Headers.TryGetValue("uuid", out var uuid);
            Request.Headers.TryGetValue("username", out var username);
            Request.Headers.TryGetValue("password", out var password);
;
            User? user = dataRepository.GetUserByUsernameAndPassword(username.ToString(), password.ToString().HashMD5()) ?? dataRepository.GetUser(uuid.ToString());
            if(user == null)
            {
                return Json(new { Status = 404, Message = "Not found" }, options);
            }
            return Json(user.ParseModel(), options);
        }

        [HttpPost]
        public JsonResult Post(AccountModel model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied!" }, options);
            }
            if (!TryValidateModel(model))
            {
                return Json(new { Status = 400, Message = "Invalid data format" }, options);
            }
            PersonalInformation personal = model.personalInformation.ParseModel();
            Wallet wallet = model.wallet.ParseModel(personal.UUID);
            Credential credential = model.credential.ParseModel(personal.UUID);
            AccountStatus accountStatus = model.accountStatus.ParseModel(personal.UUID);
            User user = new User(personal, credential, wallet, accountStatus, new List<Vehicle>());

            var _existing = dataRepository.GetUserByUsername(credential.Username) ?? dataRepository.GetUserByEmail(credential.Email);
            if (_existing != null)
            {
                return Json(new { Status = 409, Message = "There is a conflict of data. Data already exist." }, options);
            }
            dataRepository.AddUser(user);

            return Json(user.ParseModel(), options);
        }

        [HttpPut]
        public JsonResult Put(AccountModel model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied!" }, options);
            }
            if (!TryValidateModel(model))
            {
                return Json(new { Status = 400, Message = "Invalid data format" }, options);
            }
            PersonalInformation personal = model.personalInformation.ParseModel();
            Wallet wallet = model.wallet.ParseModel(personal.UUID);
            Credential credential = model.credential.ParseModel(personal.UUID);
            AccountStatus accountStatus = model.accountStatus.ParseModel(personal.UUID);
            User user = new User(personal, credential, wallet, accountStatus, new List<Vehicle>());

            var _existing = dataRepository.GetUserByUsername(credential.Username) ?? dataRepository.GetUserByEmail(credential.Email) ?? dataRepository.GetUser(personal.UUID);
            if (_existing == null)
            {
                return Json(new { Status = 404, Message = "No data found!" }, options);
            }
            dataRepository.UpdateUser(user);

            return Json(user.ParseModel(), options);
        }
    }
}

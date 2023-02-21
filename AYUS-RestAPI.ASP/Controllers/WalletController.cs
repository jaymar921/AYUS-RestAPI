using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : Controller
    {
        private readonly DataRepository dataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public WalletController(DataRepository data)
        {
            dataRepository = data;
        }

        [HttpGet]
        public JsonResult GetWallet(string? uuid)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            if (uuid == null)
            {
                return Json(new { Status = 401, Message = "Please specify the uuid at the query of the request" }, options);
            }
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            User? user =dataRepository.GetUser(uuid.ToString());
            

            if (user == null)
            {
                return Json(new { Status = 404, Message = "Not found" }, options);
            }

            return Json(new { Status = 200, Message = $"Wallet of {user.Credential.Username} was found", WalletData = user.ParseModel().wallet }, options);
        }

        [HttpPut]
        public JsonResult PutWallet(string? uuid)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (uuid == null)
            {
                return Json(new { Status = 401, Message = "Please specify the uuid at the query of the request" }, options);
            }
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if(!Request.Headers.TryGetValue("newbalance", out var newbalance))
            {
                return Json(new { Status = 401, Message = "Please specify the newbalance at the header of the request" }, options);
            }

            User? user = dataRepository.GetUser(uuid.ToString());
            if (user == null)
            {
                return Json(new { Status = 404, Message = "Not found" }, options);
            }

            if (!Double.TryParse(newbalance, out double result))
            {
                return Json(new { Status = 401, Message = "newbalance invalid datatype, must be a number" }, options);
            }

            if (Request.Headers.TryGetValue("pincode", out var pincode))
            {
                if(pincode.ToString() != string.Empty)
                    user.Wallet.Pincode = pincode;
            }

            user.Wallet.Balance = result;
            dataRepository.UpdateUser(user);

            return Json(new { Status = 204, Message = $"Wallet of {user.Credential.Username} was successfully updated" }, options);
        }
    }
}

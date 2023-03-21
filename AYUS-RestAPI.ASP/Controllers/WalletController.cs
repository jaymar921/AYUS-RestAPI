using AYUS_RestAPI.ASP.Classes;
using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : Controller
    {
        private readonly DataRepository dataRepository;
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public WalletController(DataRepository data)
        {
            dataRepository = data;
        }

        [HttpGet]
        public JsonResult GetWallet(string? uuid)
        {

            if (uuid == null)
            {
                return Json(new { Status = 401, Message = "Please specify the uuid at the query of the request" }, options);
            }
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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
            
            if (uuid == null)
            {
                return Json(new { Status = 401, Message = "Please specify the uuid at the query of the request" }, options);
            }

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!Request.Headers.TryGetValue("newbalance", out var newbalance))
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
            dataRepository.AddLog(new Data.Logs { Info = $"'{user.Credential.Username}' wallet was updated" });
            return Json(new { Status = 204, Message = $"Wallet of {user.Credential.Username} was successfully updated" }, options);
        }
    }
}

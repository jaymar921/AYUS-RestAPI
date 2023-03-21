using AYUS_RestAPI.ASP.Classes;
using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : Controller
    {
        private readonly DataRepository dataRepository;
        public JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public HistoryController(DataRepository data)
        {
            dataRepository = data;
        }
        [HttpGet]
        public JsonResult Get()
        {

            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);

            if(!Request.Headers.TryGetValue("UserID", out var userid) || !Request.Headers.TryGetValue("option", out var option) || !Request.Headers.TryGetValue("limit", out var lim))
            {
                return Json(new { Status = 401, Message = "Please specify the [UserID, option, and limit]  at the header of the request" }, options);
            }
            Int32.TryParse(lim.ToString(), out int limit);
            if (limit == 0)
                limit = 9999;
            if (option.ToString().ToLower() == "session" || option.ToString().ToLower() == "transaction")
            {
                var data = dataRepository.GetAllSessions().OrderByDescending(s => s.TimeEnd).Where(s => s.ClientUUID == userid.ToString() || s.MechanicUUID == userid.ToString()).Take(limit).ToList();
                if (option.ToString().ToLower() == "session")
                {
                    return Json(new { Message = "Retrieved sessions", data }, options);
                }else if(option.ToString().ToLower() == "transaction")
                {
                    List<Transaction> transactions = new List<Transaction>();
                    foreach(var session in data)
                    {
                        var transaction = dataRepository.GetTransaction(session.TransactionID);
                        if(transaction != null)
                            transactions.Add(transaction);
                    }
                    return Json(new { Message = "Retrieved transactions", data= transactions }, options);
                }
            }
            else if (option.ToString().ToLower() == "billing")
            {
                var mechanic = dataRepository.GetUser(userid.ToString());
                if(mechanic != null)
                {
                    if(mechanic.AccountStatus.ShopID != null)
                    {
                        var data = dataRepository.GetAllBillings().OrderByDescending(s => s.BillingDate).Where(b => b.ShopID == mechanic.AccountStatus.ShopID).Take(limit).ToList();
                        return Json(new { Message = "Retrieved billing", data }, options);
                    }
                }
                
            }


            return Json(new { Message = "No data to retrieve from history" }, options);
        }
    }
}

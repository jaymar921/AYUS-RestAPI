using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        private readonly DataRepository dataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";

        public TransactionController(DataRepository dataRepository) 
        {
            this.dataRepository = dataRepository;
        }

        [HttpPost]
        public JsonResult PostTransaction(TransactionModel model)
        {
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

            Transaction transaction = new Transaction
            {
                ServiceName = model.ServiceName,
                ServicePrice = model.ServicePrice,
                Remark = model.Remark
            };

            dataRepository.AddTransaction(transaction);

            return Json(new { Status=201, Message = "Transaction was successfully created", TransactionID = transaction.ID, Info=new { transaction.ServiceName, transaction.ServicePrice, transaction.Remark, transaction.DateOfTransaction} }, options);
        }


        [HttpGet]
        public JsonResult GetTransaction()
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("TransactionID", out var transactionID))
            {
                return Json(new { Status = 401, Message = "Please specify the TransactionID at the header of the request" }, options);
            }

            Transaction? transaction = dataRepository.GetTransaction(transactionID.ToString());

            if(transaction != null)
            {
                return Json(new { Status = 200, Message = "Transaction was successfully created", TransactionID = transaction.ID, Info = new { transaction.ServiceName, transaction.ServicePrice, transaction.Remark, transaction.DateOfTransaction } }, options);
            }

            return Json(new { Status=404, Message="Specified transaction ID was not found"}, options);
        }
    }
}

using AYUS_RestAPI.ASP.Classes;
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

        public TransactionController(DataRepository dataRepository) 
        {
            this.dataRepository = dataRepository;
        }

        [HttpPost]
        public JsonResult PostTransaction(TransactionModel model)
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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

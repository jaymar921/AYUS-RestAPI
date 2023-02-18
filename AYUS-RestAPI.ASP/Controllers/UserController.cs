using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly DataRepository dataRepository;
        public UserController(DataRepository data)
        {
            dataRepository = data;
        }

        public JsonResult Get()
        {
            
            //dataRepository.AddUser(user);
            //user = dataRepository.GetUser("e6c36be0-768b-4a31-b0de-faf02950146f");
            User user = dataRepository.GetUser("b58257e1-7509-4a7a-be91-0b7f3853c754");
            var options = new JsonSerializerOptions { WriteIndented = true };

            return Json(new { user, shop=user.AccountStatus.getShop() }, options);
        }
    }
}

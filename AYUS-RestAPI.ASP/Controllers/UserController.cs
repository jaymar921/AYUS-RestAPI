using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using Microsoft.AspNetCore.Mvc;
using System.Text;
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

        [Route("/api/account")]
        [HttpGet]
        public JsonResult GetAccount()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }
            Request.Headers.TryGetValue("uuid", out var uuid);
            Request.Headers.TryGetValue("username", out var username);
            Request.Headers.TryGetValue("password", out var password);

            /*
fetch('http://ipaddress/api/account', {
    method: 'GET',
    headers: {
        'AYUS-API-KEY': 'API_SECRET-42e016b219421dc83d180bdee27f81dd'
    },
    body: {
        username: "JayMar921",
        password: "password123"
    }
})
            */



            //dataRepository.AddUser(user);
            //user = dataRepository.GetUser("e6c36be0-768b-4a31-b0de-faf02950146f");
            //User user = dataRepository.GetUser("b58257e1-7509-4a7a-be91-0b7f3853c754");
            User user = dataRepository.GetUser(username.ToString(), password.ToString()) ??dataRepository.GetUser(uuid.ToString());

            return Json(new { user, shop=user.AccountStatus.getShop() }, options);
        }

        [Route("/api/account/reg")]
        [HttpGet]
        public JsonResult Get()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            // Setting up CLIENT

            // create the personal info
            PersonalInformation personal = new PersonalInformation
            {
                Firstname = "Jayharron Mar",
                Lastname = "Abejar",
                Contact = "09123456789",
                Birthdate = new DateTime(2000, 09, 21),
                Address = "Talisay City, Cebu",
                LicenseNumber = "G-1232321",
                Expiry = new DateTime(2023, 09, 21)
            };

            Wallet wallet = new Wallet
            {
                Balance = 0,
                UUID = personal.UUID,
                Pincode = string.Empty,
            };

            Credential credential = new Credential
            {
                UUID = personal.UUID,
                Password = "password123",
                Username = "JayMar921",
                Email = "jayharron@email.com"
            };

            AccountStatus accountStatus = new AccountStatus
            {
                UUID = personal.UUID,
                Role = "CLIENT"
            };

            User user = new User(personal, credential, wallet, accountStatus, new List<Vehicle>());
            dataRepository.AddUser(user);

            return Json(new { user, shop = user.AccountStatus.getShop() }, options);
        }
    }
}

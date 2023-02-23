using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request.MetaData;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using AYUS_RestAPI.Enumerations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MechanicController : Controller
    {
        private readonly DataRepository dataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public MechanicController(DataRepository data)
        {
            dataRepository = data;
        }

        [HttpGet]
        [Route("Shop")]
        public JsonResult GetShop()
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if(!Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the MechanicUUID at the header of the request" }, options);
            }

            User? user = dataRepository.GetUser(mechanicUUID.ToString());

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from MechanicUUID specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "User is found but the role is not a Mechanic" }, options);
            }

            Shop? shop = dataRepository.GetShop(user.AccountStatus.ShopID);

            return Json(new { Status = 200, Message = "Shop data was found", Shop=shop }, options);
        }

        [HttpPut]
        [Route("Shop")]
        public JsonResult PutShop(ShopModel model)
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the MechanicUUID at the header of the request" }, options);
            }

            User? user = dataRepository.GetUser(mechanicUUID.ToString());

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from MechanicUUID specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "User is found but the role is not a Mechanic" }, options);
            }

            Shop? shop = dataRepository.GetShop(user.AccountStatus.ShopID);

            if(shop !=null) 
            {
                shop.ShopName = model.ShopName;
                shop.ShopDescription = model.ShopDescription; 
                dataRepository.UpdateShop(shop);
                return Json(new { Status = 200, Message = "Shop data was updated", Shop = new { shop.ShopID, shop.ShopName, shop.ShopDescription } }, options);
            }


            return Json(new { Status = 404, Message = "Shop data was not found" }, options);
        }

        [HttpPost]
        [Route("ServiceOffer")]
        public JsonResult PostServiceOffer(ServiceOfferModel model)
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the MechanicUUID at the header of the request" }, options);
            }

            User? user = dataRepository.GetUser(mechanicUUID.ToString());

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from MechanicUUID specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "User is found but the role is not a Mechanic" }, options);
            }

            Shop? shop = dataRepository.GetShop(user.AccountStatus.ShopID);

            if(shop == null)
                return Json(new { Status = 404, Message = "Shop data was not found" }, options);

            Service? service = dataRepository.GetService(model.ServiceID);

            if(service == null)
                return Json(new { Status = 404, Message = "ServiceID specified was not found" }, options);

            ServiceOffer serviceOffer = new ServiceOffer
            {
                ServiceID = model.ServiceID,
                ServiceExpertise = model.ServiceExpertise,
                ShopID = shop.ShopID,
                Price = model.Price
            };

            dataRepository.AddServiceOffer(serviceOffer);


            return Json(new { Status = 201, Message = "Service was registered, see info for more details", Info=serviceOffer }, options);
        }

        [HttpGet]
        [Route("ServiceOffer")]
        public JsonResult GetServiceOffer()
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the MechanicUUID at the header of the request" }, options);
            }

            User? user = dataRepository.GetUser(mechanicUUID.ToString());

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from MechanicUUID specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "User is found but the role is not a Mechanic" }, options);
            }

            Shop? shop = dataRepository.GetShop(user.AccountStatus.ShopID);

            if (shop == null)
                return Json(new { Status = 404, Message = "Shop data was not found" }, options);

            


            return Json(new { Status = 200, Message = $"List of service offered from shop '{shop.ShopName}', see info for more details", Info = shop.ServiceOffers }, options);
        }

        [HttpDelete]
        [Route("ServiceOffer")]
        public JsonResult DeleteServiceOffer()
        {
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            if (!Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the MechanicUUID at the header of the request" }, options);
            }

            if (!Request.Headers.TryGetValue("ServiceOfferUUID", out var serviceOfferUUID))
            {
                return Json(new { Status = 401, Message = "Please specify the ServiceOfferUUID at the header of the request" }, options);
            }

            User? user = dataRepository.GetUser(mechanicUUID.ToString());

            if (user == null)
            {
                return Json(new { Status = 404, Message = "No data found from MechanicUUID specified" }, options);
            }

            if (!user.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 400, Message = "User is found but the role is not a Mechanic" }, options);
            }

            Shop? shop = dataRepository.GetShop(user.AccountStatus.ShopID);

            if (shop == null)
                return Json(new { Status = 404, Message = "Shop data was not found" }, options);

            ServiceOffer? offer = null;
            shop.ServiceOffers.ForEach(s =>
            {
                if(s.UUID == serviceOfferUUID.ToString())
                {
                    offer = s;
                }
            });
            if(offer != null)
                dataRepository.DeleteServiceOffer(offer);


            return Json(new { Status = 200, Message = $"Removed Service offer successfully", Info = shop.ServiceOffers }, options);
        }
    }
}

using AYUS_RestAPI.ASP.Classes;
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
        public JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public MechanicController(DataRepository data)
        {
            dataRepository = data;
        }

        [HttpGet]
        [Route("Shop")]
        public JsonResult GetShop()
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);

            if(Request.Headers.TryGetValue("option",out var opt))
            {
                if(opt.ToString().ToLower() == "all")
                {
                    return Json(new { Status = 200, Message = "Retrieved all shops", Shops = dataRepository.GetAllShop() }, options); 
                }
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
            if (shop != null)
            {
                shop.Billings.Clear();
                dataRepository.GetAllBillings(shop.ShopID).ForEach( bill => shop.Billings.Add(bill));
                shop.ServiceOffers.Clear();
                dataRepository.GetAllServiceOffers(shop.ShopID).ForEach(serviceOffer => shop.ServiceOffers.Add(serviceOffer));
            }

            return Json(new { Status = 200, Message = "Shop data was found", Shop=shop }, options);
        }

        [HttpPut]
        [Route("Shop")]
        public JsonResult PutShop(ShopModel model)
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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
                ServiceID = service.ServiceID,
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
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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

            List<Object> list = new List<Object>();

            shop.ServiceOffers.ForEach(serviceOffered =>
            {
                list.Add(new
                {
                    serviceOffered.UUID,
                    serviceOffered.ShopID,
                    serviceOffered.Price,
                    serviceOffered.ServiceExpertise,
                    serviceOffered.ServiceID,
                    serviceOffered.GetService().ServiceName
                });
            });


            return Json(new { Status = 200, Message = $"List of service offered from shop '{shop.ShopName}', see info for more details", Info = list }, options);
        }

        [HttpDelete]
        [Route("ServiceOffer")]
        public JsonResult DeleteServiceOffer()
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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

        [HttpPost]
        [Route("Billing")]
        public JsonResult PostBilling(BillingModel model)
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            Shop? shop = dataRepository.GetShop(model.ShopID);

            if(shop == null)
            {
                return Json(new { Status = 404, Message = "Shop data was not found!" }, options);
            }

            if (shop.ShopName == string.Empty)
            {
                return Json(new { Status = 404, Message = "Shop data was not found!" }, options);
            }

            Billing billing = new Billing
            {
                ShopID = model.ShopID,
                ServiceFee = model.ServiceFee,
                ServiceRemark = model.ServiceRemark,
            };
            dataRepository.AddBilling(billing);

            return Json(new { Status = 201, Message = "Billing was created, see info for details", Info=billing }, options);
        }

        [HttpGet]
        [Route("Billing")]
        public JsonResult GetBilling()
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!Request.Headers.TryGetValue("ShopID", out var shopID))
            {
                return Json(new { Status = 401, Message = "Please specify the ShopID at the header of the request" }, options);
            }

            Shop? shop = dataRepository.GetShop(shopID.ToString());

            if (shop == null)
            {
                return Json(new { Status = 404, Message = "Shop data was not found!" }, options);
            }

            if (shop.ShopName == string.Empty)
            {
                return Json(new { Status = 404, Message = "Shop data was not found!" }, options);
            }

            List<Billing> BillingData = dataRepository.GetAllBillings(shopID.ToString());

            return Json(new { Status = 200, Message = "Billing information found from shop provided", BillingData }, options);
        }


        [HttpDelete]
        [Route("Billing")]
        public JsonResult DeleteBilling()
        {
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!Request.Headers.TryGetValue("BillingID", out var billingID))
            {
                return Json(new { Status = 401, Message = "Please specify the BillingID at the header of the request" }, options);
            }

            Billing? billing = dataRepository.GetBilling(billingID.ToString());
            if(billing == null)
            {
                return Json(new { Status = 404, Message = "Billing information not found"}, options);
            }

            if(billing.ShopID == string.Empty)
                return Json(new { Status = 404, Message = "Billing information not found" }, options);

            dataRepository.DeleteBilling(billing);

            return Json(new { Status = 200, Message = "Deleted Billing information, see info for details", Info=billing }, options);
        }
    }
}

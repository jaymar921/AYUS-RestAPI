﻿using AYUS_RestAPI.ASP.Classes;
using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly DataRepository dataRepository;
        public AccountController(DataRepository data)
        {
            dataRepository = data;
        }

        
        [HttpGet]
        public JsonResult GetAccount()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1],options);

            Request.Headers.TryGetValue("uuid", out var uuid);
            Request.Headers.TryGetValue("username", out var username);
            Request.Headers.TryGetValue("password", out var password);

            if(Request.Headers.TryGetValue("option", out var opt))
            {
                if(opt.ToString().ToLower() == "all")
                {
                    List<AccountModel> users = new();
                    dataRepository.GetAllUser().ToList().ForEach(user => users.Add(user.ParseModel()));
                    return Json(new { Status = 200, Message = "Retrieved Account", Accounts = users}, options);
                }
            }
            
            User? user = dataRepository.GetUserByUsernameAndPassword(username.ToString(), password.ToString().HashSHA256()) ?? dataRepository.GetUser(uuid.ToString());
            if(user == null)
            {
                User? _check_user_username = dataRepository.GetUserByUsername(username.ToString());
                if(_check_user_username != null)
                    return Json(new { Status = 401, Message = "Invalid Password, Access Denied" }, options);
                return Json(new { Status = 404, Message = "Not found" }, options);
            }
            return Json(new { Status = 200, Message = "Retrieved Account", AccountData=user.ParseModel() }, options);
        }

        [HttpPost]
        public JsonResult Post(AccountModel model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


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

            return Json(new { Status = 201, Message = "Account was successfully registered" }, options);
        }

        [HttpPut]
        public JsonResult Put(AccountModel model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);

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

            return Json(new { Status = 204, Message = "Account was successfully updated" }, options);
        }

        [HttpDelete]
        public JsonResult DeleteAccount()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);

            Request.Headers.TryGetValue("uuid", out var uuid);


            User? user = dataRepository.GetUser(uuid.ToString());

            if(user == null)
            {
                return Json(new { Status = 404, Message = "Account not found" }, options);
            }

            dataRepository.DeleteUser(user.PersonalInformation.UUID);

            return Json(new { Status = 200, Message = "Deleted Account, see info for details", Info = user.ParseModel() }, options);
        }

        [HttpPut]
        [Route("Password")]
        public JsonResult UpdatePassword(string uuid)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            var _existing = dataRepository.GetUser(uuid);
            if (_existing == null)
            {
                return Json(new { Status = 404, Message = "No data found!" }, options);
            }

            if(!Request.Headers.TryGetValue("new-password", out var newpassword))
            {
                return Json(new { Status = 404, Message = "Please specify the new-password at the header of the request" }, options);
            }

            _existing.Credential.Password = newpassword.ToString().HashSHA256();

            dataRepository.UpdateUser(_existing);

            return Json(new { Status = 204, Message = "Account password was successfully updated" }, options);
        }

        [HttpPost]
        [Route("Vehicle")]
        public JsonResult RegisterVehicle(Vehicle model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!TryValidateModel(model))
            {
                return Json(new { Status = 400, Message = "Invalid data format" }, options);
            }

            if (dataRepository.GetUser(model.UUID) == null)
            {
                return Json(new { Status = 401, Message = $"The user with UUID '{model.UUID}' does not exist." }, options);
            }

            // check if the vehicle already exist
            if(dataRepository.GetVehicleByPlateNumber(model.PlateNumber) != null)
            {
                return Json(new { Status = 401, Message = $"Vehicle with PlateNumber '{model.PlateNumber}' already exist in the database. change the method to 'PUT' to update vehicle information." }, options);
            }

            dataRepository.AddVehicle(model);
            

            return Json(new { Status = 201, Message = "Vehicle was registered successfully", Info=model }, options);
        }

        [HttpPut]
        [Route("Vehicle")]
        public JsonResult UpdateVehicle(Vehicle model)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!TryValidateModel(model))
            {
                return Json(new { Status = 400, Message = "Invalid data format" }, options);
            }

            if (dataRepository.GetUser(model.UUID) == null)
            {
                return Json(new { Status = 401, Message = $"The user with UUID '{model.UUID}' does not exist." }, options);
            }

            // check if the vehicle already exist
            if (dataRepository.GetVehicleByPlateNumber(model.PlateNumber) == null)
            {
                return Json(new { Status = 401, Message = $"Vehicle with PlateNumber '{model.PlateNumber}' does not exist in the database. Use 'POST' method to register vehicle instead." }, options);
            }

            dataRepository.UpdateVehicle(model);


            return Json(new { Status = 204, Message = "Vehicle was updated successfully", Info=model }, options);
        }

        [HttpDelete]
        [Route("Vehicle")]
        public JsonResult DeleteVehicle()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!Request.Headers.TryGetValue("PlateNumber", out var plateNumber))
            {
                return Json(new { Status = 401, Message = "Please specify the PlateNumber at the header of the request" }, options);
            }

            Vehicle? vehicle = dataRepository.GetVehicleByPlateNumber(plateNumber.ToString());

            if(vehicle == null)
            {
                return Json(new { Status = 404, Message = $"Vehicle with PlateNumber '{plateNumber}' does not exist" }, options);
            }

            dataRepository.DeleteVehicle(vehicle);


            return Json(new { Status = 200, Message = $"Vehicle with PlateNumber '{plateNumber}' was deleted successfully" }, options);
        }

        [HttpGet]
        [Route("Vehicle")]
        public JsonResult GetVehicle()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            // header validation
            var _validation = HeaderValidation.Validate(Request);
            bool.TryParse((string?)_validation[0], out bool validated);
            if (!validated)
                return Json(_validation[1], options);


            if (!Request.Headers.TryGetValue("uuid", out var uuid))
            {
                return Json(new { Status = 401, Message = "Please specify the uuid at the header of the request" }, options);
            }

            User? user = dataRepository.GetUser(uuid.ToString());
            if (user == null)
            {
                return Json(new { Status = 404, Message = $"The user with UUID '{uuid}' does not exist." }, options);
            }

            List<Vehicle> vehicles = dataRepository.GetVehicle(uuid.ToString());



            return Json(new { Status = 200, Message = $"Retrieved list of vehicle from user {user.Credential.Username}", Vehicles=vehicles.ParseVehicles() }, options);
        }
    }
}

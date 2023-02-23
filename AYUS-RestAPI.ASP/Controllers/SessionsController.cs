using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Data;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Enumerations;
using AYUS_RestAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : Controller
    {
        private readonly DataRepository dataRepository;
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public SessionsController(DataRepository data)
        {
            dataRepository = data;
        }

        [HttpGet]
        [Route("AvailableMechanics")]
        public JsonResult GetAvailableMechanics()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }


            List<MechanicPartial> users = new List<MechanicPartial>();
            var sessionsList = dataRepository.GetAllSessions().ToList();
            dataRepository.GetAllUser()
                .Where(u => u.AccountStatus.GetRole.Equals(Roles.MECHANIC)).ToList()
                .ForEach(u => {
                    bool found = false;
                    sessionsList.ForEach(session =>
                    {
                        if (session.isActive)
                        {
                            if (session.MechanicUUID == u.PersonalInformation.UUID)
                                found = true;
                        }
                    });
                    if (!found)
                        users.Add(u.ParseMechanicModel());
                    });

            

            
            if (users.Count == 0)
            {
                return Json(new { Status = 404, Message = "No Available Mechanics" }, options);
            }
            return Json(users, options);
        }

        [HttpPost]
        [Route("RegisterSession")]
        public JsonResult PutSession()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            Request.Headers.TryGetValue("ClientUUID", out var clientUUID);
            Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID);
            Request.Headers.TryGetValue("SessionDetails", out var sessionDetails);

            if (clientUUID.ToString() == mechanicUUID.ToString())
            {
                return Json(new { Status = 401, Message = "ClientUUID and MechanicUUID should not be the same" }, options);
            }


            var client = dataRepository.GetUser(clientUUID.ToString());
            var mechanic = dataRepository.GetUser(mechanicUUID.ToString());

            if (client == null)
            {
                return Json(new { Status = 404, Message = "ClientUUID was not found" }, options);
            }

            if (mechanic == null)
            {
                return Json(new { Status = 404, Message = "MechanicUUID was not found" }, options);
            }else if (!mechanic.AccountStatus.GetRole.Equals(Roles.MECHANIC))
            {
                return Json(new { Status = 401, Message = "MechanicUUID doesn't have a 'MECHANIC' role, request denied" }, options);
            }

            var sessionsList = dataRepository.GetAllSessions().ToList();
            object? invalid = null;
            sessionsList.ForEach(session =>
            {
                if(session.isActive)
                {
                    if (session.MechanicUUID == mechanicUUID)
                    {
                        invalid = new { Status = 404, Message = "Mechanic is already in a session, denied" };
                    }
                    else if (session.ClientUUID == clientUUID)
                    {
                        invalid = new { Status = 404, Message = "Client is already in a session, denied" };
                    }
                }
            });

            if(invalid != null)
            {
                return Json(invalid, options);
            }

            Session session = new Session
            {
                ClientUUID = clientUUID.ToString(),
                MechanicUUID = mechanicUUID.ToString(),
                SessionDetails = sessionDetails.ToString()
            };

            dataRepository.AddSession(session);
            
            return Json(new { Status = 201, Message = "Session Registered", session.SessionID }, options);
        }

        [HttpGet]
        [Route("GetSession")]
        public JsonResult GetSession()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            Request.Headers.TryGetValue("ClientUUID", out var clientUUID);
            Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID);
            Request.Headers.TryGetValue("SessionID", out var sessionID);

            var sessionsList = dataRepository.GetAllSessions().ToList();
            object? foundData = null;
            sessionsList.ForEach(session =>
            {
                if (session.isActive)
                {
                    if (session.MechanicUUID == mechanicUUID || session.ClientUUID == clientUUID || session.SessionID == sessionID)
                    {
                        foundData = new { Status = 200, Message = "Sesion found", SessionData = session };
                    }
                }
            });

            if(foundData != null) return Json(foundData, options);

            return Json(new { Status = 404, Message = "No session found"}, options);
        }

        [HttpPut]
        [Route("EndSession")]
        public JsonResult EndSession()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!Request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return Json(new { Status = 401, Message = "Please specify the API KEY at the header of the request" }, options);
            }

            if (apiKey != API_KEY)
            {
                return Json(new { Status = 401, Message = "Invalid API Key, Access Denied" }, options);
            }

            Request.Headers.TryGetValue("ClientUUID", out var clientUUID);
            Request.Headers.TryGetValue("MechanicUUID", out var mechanicUUID);
            Request.Headers.TryGetValue("SessionID", out var sessionID);

            // check if transaction header was found in the header of the request
            if(!Request.Headers.TryGetValue("TransactionID", out var transactID))
            {
                return Json(new { Status = 401, Message = "TransactionID must be specified at the header of the request, make sure that the transaction has been done before ending the session" }, options);
            }

            // check if the transaction id is already exist
            if(transactID.ToString() == string.Empty)
            {
                return Json(new { Status = 401, Message = "TransactionID must not be empty, make sure that the transaction has been done before ending the session" }, options);
            }

            // Making sure that the transaction ID already exists in the database
            if(dataRepository.GetTransaction(transactID.ToString()) == null)
            {
                return Json(new { Status = 404, Message = "Specified transaction was not found, make sure that the transaction was already created before calling this request." }, options);
            }


            var sessionsList = dataRepository.GetAllSessions().ToList();
            Session? foundSession = null;
            sessionsList.ForEach(session =>
            {
                if (session.isActive)
                {
                    if (session.MechanicUUID == mechanicUUID || session.ClientUUID == clientUUID || session.SessionID == sessionID)
                    {
                        foundSession = session; 
                    }
                }
            });


            // return success if the session is already exist, because if not, no session can be ended if not created :)
            if(foundSession != null)
            {
                foundSession.TransactionID = transactID;
                foundSession.TimeEnd = DateTime.UtcNow;
                foundSession.isActive = false;
                return Json(new { Status = 200, Message = $"Session with ID {sessionID} has been ended successfully" }, options);
            }

            // return not found if no session was found 
            return Json(new { Status = 404, Message = "No session found" }, options);
        }
    }
}

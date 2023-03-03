using System.Text.Json;

namespace AYUS_RestAPI.ASP.Classes
{
    public class HeaderValidation
    {
        private static string API_KEY = "API_SECRET-42e016b219421dc83d180bdee27f81dd";
        public static List<object> Validate(HttpRequest request)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            if (!request.Headers.TryGetValue("AYUS-API-KEY", out var apiKey))
            {
                return new List<object> { "false", new { Status = 401, Message = "Please specify the API KEY at the header of the request" } };
            }

            if (apiKey != API_KEY)
            {
                return new List<object> { "false", new { Status = 401, Message = "Invalid API Key, Access Denied" } };
            }

            return new List<object> { "true", new { Status = 200 } };
        }
    }
}

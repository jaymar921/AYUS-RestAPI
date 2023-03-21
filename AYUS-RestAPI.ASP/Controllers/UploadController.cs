using AYUS_RestAPI.ASP.Models;
using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.Entity.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AYUS_RestAPI.ASP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly DataRepository dataRepository;
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        public static IWebHostEnvironment _environment;
        public UploadController(DataRepository data, IWebHostEnvironment environment)
        {
            dataRepository = data;
            _environment = environment;
        }

        [HttpPost]
        public async Task<JsonResult> UploadData([FromForm] FileUpload fileUpload)
        {

            if(!Request.Headers.TryGetValue("UserID", out var userID))
            {
                return Json(new { Status = 401, Message = "Please specify the UserID at the header of the request" }, options);
            }

            if (!Request.Headers.TryGetValue("Filename", out var filename))
            {
                return Json(new { Status = 401, Message = "Please specify the UserID at the header of the request" }, options);
            }

            foreach (var item in fileUpload.files)
            {

                if (item.FileName == null || item.FileName.Length == 0)
                {
                    return Json(new { Status = 201, Message = "No File" }, options);
                }

                string ext = item.FileName.Split(".")[1];
                var path = Path.Combine(_environment.WebRootPath, "Images/", $"{userID}_{filename}.{ext}");

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                    stream.Close();
                }
            }
            return Json(new { Status = 201, Message = "Uploaded File" }, options);
        }

        [HttpGet]
        public HttpResponseMessage GetData()
        {

            if (!Request.Headers.TryGetValue("UserID", out var userID))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (!Request.Headers.TryGetValue("Filename", out var filename))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            
            var path = Path.Combine(_environment.WebRootPath, "Images/", $"{userID}_{filename}.png");
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentLength = stream.Length;
                }

            }catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return result;
        }

        [HttpGet("files/{userid}/{filename}")]
        public async Task<ActionResult> DownloadData(string userid, string filename)
        {
            var path = Path.Combine(_environment.WebRootPath, "Images/", $"{userid}_{filename}.png");
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            return File(bytes, contentType, Path.GetFileName(path));
        }
    }
}

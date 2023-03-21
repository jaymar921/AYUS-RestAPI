namespace AYUS_RestAPI.ASP.Models.Request
{
    public class FileUpload
    {
        public List<IFormFile> files { get; set; } = new List<IFormFile>();
    }
}

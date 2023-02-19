using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class ShopModel
    {
        
        public string ShopID { get; set; } = Guid.NewGuid().ToString();
        public string ShopName { get; set; } = string.Empty;
        public string ShopDescription { get; set; } = string.Empty;
    }
}

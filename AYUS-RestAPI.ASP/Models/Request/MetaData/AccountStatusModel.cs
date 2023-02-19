using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class AccountStatusModel
    {
        public ShopModel? Shop { get; set; } = new ShopModel();
        [Required]
        public string Role { get; set; } = string.Empty;


    }
}

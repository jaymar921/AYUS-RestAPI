using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class AccountStatusModel
    {
        public ShopModel? Shop { get; set; } = new ShopModel();
        [Required]
        public string Role { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public bool IsOnline { get; set; } = false;

    }
}

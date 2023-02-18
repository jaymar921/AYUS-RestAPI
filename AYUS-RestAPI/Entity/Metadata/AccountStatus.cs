using AYUS_RestAPI.Entity.Metadata.Mechanic;
using AYUS_RestAPI.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Entity.Metadata
{
    public class AccountStatus
    {
        [Key]
        public string UUID { get; set; } = string.Empty;
        public string ShopID { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        private Shop Shop = new Shop();
        public AccountStatus() { }

        public AccountStatus(Shop shop)
        {
            Shop = shop;
        }

        public Shop getShop() { return Shop; }
        public void setShop(Shop shop) { Shop = shop; }

        public Roles GetRole { get { return (new Roles()).GetRoles(Role);  } }
    }
}

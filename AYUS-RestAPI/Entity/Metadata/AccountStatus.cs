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
        public double Rating { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public bool IsOnline { get; set; } = false;

        private Shop Shop = new Shop();
        public AccountStatus() { }

        public AccountStatus(Shop shop)
        {
            Shop = shop;
        }

        public Shop GetShop() { return Shop; }
        public void SetShop(Shop shop) { Shop = shop; }

        public Roles GetRole { get { return (new Roles()).GetRoles(Role);  } }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

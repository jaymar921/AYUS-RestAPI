using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Entity.Metadata.Mechanic;

namespace AYUS_RestAPI.ASP.Models
{
    public class DataRepository
    {
        private readonly AppDbContext _dbContext;
        public DataRepository(AppDbContext appDbContext) 
        {
            _dbContext = appDbContext;
        }

        public void AddUser(User user)
        {
            PersonalInformation personalInformation = user.PersonalInformation;
            Credential credential = user.Credential;
            Wallet wallet = user.Wallet;
            List<Vehicle> vehicles = user.Vehicles;
            AccountStatus accountStatus = user.AccountStatus;

            _dbContext.personalInformation.Add(personalInformation);
            _dbContext.credential.Add(credential);
            _dbContext.wallets.Add(wallet);
            _dbContext.accountStatus.Add(accountStatus);
            vehicles.ForEach( vehicle => { _dbContext.Add(vehicle); });

            if (accountStatus.GetRole == Enumerations.Roles.MECHANIC){
                Shop shop = accountStatus.getShop();
                _dbContext.shops.Add(shop);
                shop.ServiceOffers.ForEach(offer => {

                    Service service = offer.getService();
                    _dbContext.services.Add(service);
                    _dbContext.serviceOffers.Add(offer);

                });
                shop.Billings.ForEach(bill => {
                    _dbContext.billing.Add(bill);
                });
            }

            _dbContext.SaveChanges();
        }

        public User GetUser(string uuid)
        {
            PersonalInformation personalInformation = _dbContext.personalInformation.First(p => p.UUID == uuid);
            Credential credential = _dbContext.credential.First(c => c.UUID == uuid);
            Wallet wallet = _dbContext.wallets.First(w => w.UUID == uuid);
            AccountStatus accountStatus = _dbContext.accountStatus.First(a => a.UUID == uuid);

            if(accountStatus.GetRole == Enumerations.Roles.MECHANIC)
            {
                
                Shop shop = _dbContext.shops.First(s => s.ShopID == accountStatus.ShopID);
                List<ServiceOffer> serviceOffer = _dbContext.serviceOffers.Where( so => so.ShopID == shop.ShopID).ToList();
                serviceOffer.ForEach(offer =>
                {
                    offer.setService(_dbContext.services.First(service => service.ServiceID == offer.ServiceID));
                });
                _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                    bill => shop.Billings.Add(bill)
                );
                
                accountStatus.setShop(shop);
            }

            return new User(personalInformation, credential, wallet, accountStatus, new List<Vehicle>());
        }
    }
}

using AYUS_RestAPI.Data;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Entity.Metadata.Mechanic;
using System;

namespace AYUS_RestAPI.ASP.Models
{
    public class DataRepository
    {
        private readonly AppDbContext _dbContext;
        public DataRepository(AppDbContext appDbContext) 
        {
            _dbContext = appDbContext;
            //_dbContext.Database.EnsureDeleted();
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
            vehicles.ForEach( vehicle => { _dbContext.vehicles.Add(vehicle); });

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

        public void UpdateUser(User user)
        {
            PersonalInformation? personalInformation = _dbContext.personalInformation.FirstOrDefault(p => p.UUID == user.PersonalInformation.UUID);
            if (personalInformation != null)
            {
                personalInformation.Firstname = user.PersonalInformation.Firstname;
                personalInformation.Lastname = user.PersonalInformation.Lastname;
                personalInformation.Contact = user.PersonalInformation.Contact;
                personalInformation.Birthdate = user.PersonalInformation.Birthdate;
                personalInformation.Address = user.PersonalInformation.Address;
                personalInformation.LicenseNumber = user.PersonalInformation.LicenseNumber;
                personalInformation.Expiry = user.PersonalInformation.Expiry;
                _dbContext.SaveChanges();
            }
            Credential credential = _dbContext.credential.First(c => c.UUID == user.PersonalInformation.UUID);
            if(credential != null)
            {
                //credential.Username = user.Credential.Username;
                credential.Password = user.Credential.Password;
                //credential.Email = user.Credential.Email;
                _dbContext.SaveChanges();
            }
            Wallet wallet = _dbContext.wallets.First(w => w.UUID == user.PersonalInformation.UUID);
            if(wallet != null)
            {
                wallet.Balance = user.Wallet.Balance;
                wallet.Pincode = user.Wallet.Pincode;
                _dbContext.SaveChanges();
            }
            AccountStatus accountStatus = _dbContext.accountStatus.First(a => a.UUID == user.PersonalInformation.UUID);
            /*
            if(accountStatus != null)
            {
                accountStatus.ShopID = user.AccountStatus.getShop().ShopID;
                accountStatus.Role = user.AccountStatus.Role;
                _dbContext.SaveChanges();
                if (accountStatus.GetRole == Enumerations.Roles.MECHANIC)
                {

                    Shop shop = _dbContext.shops.First(s => s.ShopID == user.AccountStatus.getShop().ShopID);
                    if(shop != null)
                    {
                        shop.ShopName = user.AccountStatus.getShop().ShopName;
                        shop.ShopDescription = user.AccountStatus.getShop().ShopDescription;

                        _dbContext.SaveChanges();
                    }
                    
                }
            }
            */
            
            _dbContext.SaveChanges();
        }

        public User? GetUser(string uuid)
        {
            PersonalInformation? personalInformation = _dbContext.personalInformation.FirstOrDefault(p => p.UUID == uuid);
            if (personalInformation == null)
            {
                return null;
            }
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

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            Credential? credential = _dbContext.credential.FirstOrDefault(c => c.Username == username && c.Password == password);
            if(credential== null) { return null; }
            PersonalInformation personalInformation = _dbContext.personalInformation.First(p => p.UUID == credential.UUID);
            
            Wallet wallet = _dbContext.wallets.First(w => w.UUID == credential.UUID);
            AccountStatus accountStatus = _dbContext.accountStatus.First(a => a.UUID == credential.UUID);

            if (accountStatus.GetRole == Enumerations.Roles.MECHANIC)
            {

                Shop shop = _dbContext.shops.First(s => s.ShopID == accountStatus.ShopID);
                List<ServiceOffer> serviceOffer = _dbContext.serviceOffers.Where(so => so.ShopID == shop.ShopID).ToList();
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

        public User? GetUserByEmail(string email)
        {
            Credential? credential = _dbContext.credential.FirstOrDefault(c => c.Email == email);
            if (credential == null) { return null; }
            PersonalInformation personalInformation = _dbContext.personalInformation.First(p => p.UUID == credential.UUID);

            Wallet wallet = _dbContext.wallets.First(w => w.UUID == credential.UUID);
            AccountStatus accountStatus = _dbContext.accountStatus.First(a => a.UUID == personalInformation.UUID);

            if (accountStatus.GetRole == Enumerations.Roles.MECHANIC)
            {

                Shop shop = _dbContext.shops.First(s => s.ShopID == accountStatus.ShopID);
                List<ServiceOffer> serviceOffer = _dbContext.serviceOffers.Where(so => so.ShopID == shop.ShopID).ToList();
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

        public User? GetUserByUsername(string username)
        {
            Credential? credential = _dbContext.credential.FirstOrDefault(c => c.Username == username);
            if (credential == null) { return null; }
            PersonalInformation personalInformation = _dbContext.personalInformation.First(p => p.UUID == credential.UUID);

            Wallet wallet = _dbContext.wallets.First(w => w.UUID == credential.UUID);
            AccountStatus accountStatus = _dbContext.accountStatus.First(a => a.UUID == credential.UUID);

            if (accountStatus.GetRole == Enumerations.Roles.MECHANIC)
            {

                Shop shop = _dbContext.shops.First(s => s.ShopID == accountStatus.ShopID);
                List<ServiceOffer> serviceOffer = _dbContext.serviceOffers.Where(so => so.ShopID == shop.ShopID).ToList();
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

        public IEnumerable<User> GetAllUser()
        {
            List<User> users = new List<User>();

            List<PersonalInformation> personalInformation = _dbContext.personalInformation.ToList();
            if (personalInformation == null)
            {
                return users;
            }
            foreach(PersonalInformation personal in personalInformation)
            {
                Credential credential = _dbContext.credential.First(c => c.UUID == personal.UUID);
                Wallet wallet = _dbContext.wallets.First(w => w.UUID == personal.UUID);
                AccountStatus accountStatus = _dbContext.accountStatus.First(a => a.UUID == personal.UUID);

                if (accountStatus.GetRole == Enumerations.Roles.MECHANIC)
                {

                    Shop shop = _dbContext.shops.First(s => s.ShopID == accountStatus.ShopID);
                    List<ServiceOffer> serviceOffer = _dbContext.serviceOffers.Where(so => so.ShopID == shop.ShopID).ToList();
                    serviceOffer.ForEach(offer =>
                    {
                        offer.setService(_dbContext.services.First(service => service.ServiceID == offer.ServiceID));
                    });
                    _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                        bill => shop.Billings.Add(bill)
                    );

                    accountStatus.setShop(shop);
                }
                users.Add(new User(personal, credential, wallet, accountStatus, new List<Vehicle>()));
            }

            return users;
        }


        public List<Session> GetAllSessions()
        {
            return _dbContext.sessions.ToList();
        }

        public Session? GetSession(string uuid)
        {
            return _dbContext.sessions.FirstOrDefault(s => s.SessionID == uuid);
        }

        public void AddSession(Session session)
        {
            _dbContext.sessions.Add(session);
            _dbContext.SaveChanges();
        }

        public void UpdateSession(Session newSession)
        {
            Session? data = GetSession(newSession.SessionID);
            if (data != null)
            {
                data.isActive = newSession.isActive;
                data.TimeEnd = newSession.TimeEnd;
                data.TransactionID = newSession.TransactionID;
            }
            _dbContext.SaveChanges();
        }

        public void AddTransaction(Transaction transaction)
        {
            _dbContext.transactions.Add(transaction);
            _dbContext.SaveChanges();
        }

        public Transaction? GetTransaction(string uuid)
        {
            return _dbContext.transactions.FirstOrDefault(t => t.ID == uuid);
        }

        public List<Transaction> GetAllTransactions()
        {
            return _dbContext.transactions.ToList();
        }
    }
}

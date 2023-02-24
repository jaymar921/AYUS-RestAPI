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
                Shop shop = accountStatus.GetShop();
                shop.ShopID = Guid.NewGuid().ToString();

                _dbContext.shops.Add(shop);
                shop.ServiceOffers.ForEach(offer => {

                    Service service = offer.GetService();
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
                    offer.SetService(_dbContext.services.First(service => service.ServiceID == offer.ServiceID));
                });
                _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                    bill => shop.Billings.Add(bill)
                );
                
                accountStatus.SetShop(shop);
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
                    offer.SetService(_dbContext.services.First(service => service.ServiceID == offer.ServiceID));
                });
                _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                    bill => shop.Billings.Add(bill)
                );

                accountStatus.SetShop(shop);
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
                    offer.SetService(_dbContext.services.First(service => service.ServiceID == offer.ServiceID));
                });
                _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                    bill => shop.Billings.Add(bill)
                );

                accountStatus.SetShop(shop);
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
                    offer.SetService(_dbContext.services.First(service => service.ServiceID == offer.ServiceID));
                });
                _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                    bill => shop.Billings.Add(bill)
                );

                accountStatus.SetShop(shop);
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
                        offer.SetService(_dbContext.services.First(service => service.ServiceID == offer.ServiceID));
                    });
                    _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                        bill => shop.Billings.Add(bill)
                    );

                    accountStatus.SetShop(shop);
                }
                users.Add(new User(personal, credential, wallet, accountStatus, new List<Vehicle>()));
            }

            return users;
        }

        public void DeleteUser(string uuid)
        {
            PersonalInformation? personalInformation = _dbContext.personalInformation.FirstOrDefault(p => p.UUID == uuid);
            if (personalInformation == null)
            {
                return;
            }

            _dbContext.personalInformation.Remove(personalInformation);

            Credential credential = _dbContext.credential.First(c => c.UUID == uuid);
            _dbContext.credential.Remove(credential);

            Wallet wallet = _dbContext.wallets.First(w => w.UUID == uuid);
            _dbContext.wallets.Remove(wallet);

            AccountStatus accountStatus = _dbContext.accountStatus.First(a => a.UUID == uuid);
            _dbContext.accountStatus.Remove(accountStatus);

            if (accountStatus.GetRole == Enumerations.Roles.MECHANIC)
            {
                Shop shop = _dbContext.shops.First(s => s.ShopID == accountStatus.ShopID);
                _dbContext.shops.Remove(shop);

                List<ServiceOffer> serviceOffer = _dbContext.serviceOffers.Where(so => so.ShopID == shop.ShopID).ToList();
                serviceOffer.ForEach(offer =>
                {
                    _dbContext.serviceOffers.Remove(offer);
                });
                /*
                _dbContext.billing.Where(bill => bill.ShopID == shop.ShopID).ToList().ForEach(
                   
                );
                */
                accountStatus.SetShop(shop);
            }

            _dbContext.SaveChanges();

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

        public void ResetDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.SaveChanges();
            _dbContext.Database.EnsureCreated();
            _dbContext.SaveChanges();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            _dbContext.vehicles.Add(vehicle);
            _dbContext.SaveChanges();
        }

        public List<Vehicle> GetVehicle(string uuid)
        {
            return _dbContext.vehicles.Where(v => v.UUID== uuid).ToList();
        }

        public Vehicle? GetVehicleByPlateNumber(string plateNumber)
        {
            return _dbContext.vehicles.FirstOrDefault(v => v.PlateNumber== plateNumber);
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            _dbContext.vehicles.Remove(vehicle);
            _dbContext.SaveChanges();
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            Vehicle? old = GetVehicleByPlateNumber(vehicle.PlateNumber);

            if(old != null)
            {
                old.Model = vehicle.Model;
                old.Color = vehicle.Color;
                old.Brand = vehicle.Brand;
                old.Type = vehicle.Type;
                _dbContext.SaveChanges();
            }
        }

        public Service? GetService(string uuid)
        {
            return _dbContext.services.FirstOrDefault(service => service.ServiceID== uuid || service.ServiceName == uuid);
        }

        public Service? GetServiceByName(string name)
        {
            return GetService(name);
        }

        public void AddService(Service service)
        {
            _dbContext.services.Add(service);
            _dbContext.SaveChanges();
        }

        public void UpdateService(Service service)
        {
            Service? old = GetService(service.ServiceID);

            if(old != null)
            {
                old.ServiceDescription = service.ServiceDescription;
                old.ServiceName = service.ServiceName;
                _dbContext.SaveChanges();
            }
        }

        public void DeleteService(Service service)
        {
            _dbContext.services.Remove(service);
            _dbContext.SaveChanges();
        }

        public List<Service> GetAllServices()
        {
            return _dbContext.services.ToList();
        }

        public void AddServiceOffer(ServiceOffer serviceOffer)
        {
            _dbContext.serviceOffers.Add(serviceOffer);
            _dbContext.SaveChanges();
        }

        public ServiceOffer? GetServiceOffer(string uuid)
        {
            return _dbContext.serviceOffers.FirstOrDefault(s => s.UUID == uuid);
        }

        public List<ServiceOffer> GetAllServiceOffers()
        {
            return _dbContext.serviceOffers.ToList();
        }

        public List<ServiceOffer> GetAllServiceOffers(string shopID)
        {
            return _dbContext.serviceOffers.Where(so => so.ShopID == shopID).ToList();
        }

        public void DeleteServiceOffer(ServiceOffer serviceOffer)
        {
            _dbContext.serviceOffers.Remove(serviceOffer);
            _dbContext.SaveChanges();
        }

        public void UpdateServiceOffer(ServiceOffer offer)
        {
            ServiceOffer? serviceOffer = _dbContext.serviceOffers.FirstOrDefault(s => s.UUID == offer.UUID);

            if(serviceOffer != null)
            {
                serviceOffer.ServiceID = offer.ServiceID;
                serviceOffer.Price = offer.Price;
                serviceOffer.ServiceExpertise = offer.ServiceExpertise;

                _dbContext.SaveChanges();
            }
        }

        public void AddShop(Shop shop)
        {
            _dbContext.shops.Add(shop); 
            _dbContext.SaveChanges();
        }

        public Shop? GetShop(string shopId)
        {
            Shop? shop = _dbContext.shops.FirstOrDefault(shop => shop.ShopID == shopId);
            return shop;
        }

        public List<Shop> GetAllShop()
        {
            return _dbContext.shops.ToList();
        }

        public void UpdateShop(Shop shop)
        {
            Shop? _shop = _dbContext.shops.FirstOrDefault(s => s.ShopID == shop.ShopID);
            if(_shop != null)
            {
                _shop.ShopName = shop.ShopName;
                _shop.ShopDescription = shop.ShopDescription;
                _dbContext.SaveChanges();
            }
        }

        public void DeleteShop(Shop shop)
        {
            _dbContext.shops.Remove(shop);
            _dbContext.SaveChanges();
        }

        public void AddMapLocation(ServiceMapLocationAPI mapLocationAPI)
        {
            _dbContext.serviceMaps.Add(mapLocationAPI);
            _dbContext.SaveChanges();
        }

        public ServiceMapLocationAPI? GetMapLocation(string sessionID)
        {
            return _dbContext.serviceMaps.FirstOrDefault(map => map.SessionID == sessionID);
        }

        public List<ServiceMapLocationAPI> GetAllMapLocation()
        {
            return _dbContext.serviceMaps.ToList();
        }

        public void UpdateMapLocation(ServiceMapLocationAPI serviceMap)
        {
            ServiceMapLocationAPI? s = _dbContext.serviceMaps.FirstOrDefault(sm => sm.SessionID == serviceMap.SessionID);

            if(s != null)
            {
                s.ClientLocLat = serviceMap.ClientLocLat;
                s.ClientLocLon = serviceMap.ClientLocLon;
                s.MechanicLocLat = serviceMap.MechanicLocLat;
                s.MechanicLocLon = serviceMap.MechanicLocLon;

                _dbContext.SaveChanges();
            }
        }

        public void DeleteMapLocation(ServiceMapLocationAPI serviceMap)
        {
            _dbContext.serviceMaps.Remove(serviceMap);
            _dbContext.SaveChanges();
        }

        
    }
}

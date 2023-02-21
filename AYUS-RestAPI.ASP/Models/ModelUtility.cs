using AYUS_RestAPI.ASP.Models.Request;
using AYUS_RestAPI.ASP.Models.Request.MetaData;
using AYUS_RestAPI.Entity.Metadata;
using AYUS_RestAPI.Utility;
using System.Net;

namespace AYUS_RestAPI.ASP.Models
{
    public static class ModelUtility
    {

        public static PersonalInformation ParseModel(this PersonalInformationModel model)
        {
            return new PersonalInformation
            {
                UUID = model.UUID,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Contact = model.Contact,
                Birthdate = model.Birthdate,
                Address = model.Address,
                LicenseNumber = model.LicenseNumber,
                Expiry = model.Expiry
            };
        }

        public static Wallet ParseModel(this WalletModel model, string uuid)
        {
            return new Wallet
            {
                Balance = model.Balance,
                UUID = uuid,
                Pincode = model.Pincode,
            };
        }

        public static Credential ParseModel(this CredentialModel model, string uuid) 
        {
            return new Credential
            {
                UUID = uuid,
                Password = model.Password.HashMD5(),
                Username = model.Username,
                Email = model.Email
            };
        }

        public static AccountStatus ParseModel(this AccountStatusModel model, string uuid)
        {
            var accountStatus = new AccountStatus
            {
                UUID = uuid,
                Role = model.Role,
                ShopID = model.Shop?.ShopID ?? "",
            };
            accountStatus.setShop(new Entity.Metadata.Mechanic.Shop
            {
                ShopID = model.Shop?.ShopID ?? "",
                ShopDescription = model.Shop?.ShopDescription ?? "",
                ShopName = model.Shop?.ShopName ?? ""
            });

            return accountStatus;
        }

        public static MechanicPartial ParseMechanicModel(this User user)
        {
            return new MechanicPartial
            {
                personalInformation = new PersonalInformationModel
                {
                    UUID = user.PersonalInformation.UUID,
                    Firstname = user.PersonalInformation.Firstname,
                    Lastname = user.PersonalInformation.Lastname,
                    Contact = user.PersonalInformation.Contact,
                    Birthdate = user.PersonalInformation.Birthdate,
                    Address = user.PersonalInformation.Address,
                    LicenseNumber = user.PersonalInformation.LicenseNumber,
                    Expiry = user.PersonalInformation.Expiry,
                },
                accountStatus = new AccountStatusModel
                {
                    Role = user.AccountStatus.Role,
                    Shop = user.AccountStatus.Role == "MECHANIC" ? new ShopModel
                    {
                        ShopID = user.AccountStatus.getShop().ShopID,
                        ShopDescription = user.AccountStatus.getShop().ShopDescription,
                        ShopName = user.AccountStatus.getShop().ShopName
                    } : null
                }
            };
        }

        public static AccountModel ParseModel(this User user)
        {
            return new AccountModel
            {
                personalInformation = new PersonalInformationModel
                {
                    UUID = user.PersonalInformation.UUID,
                    Firstname = user.PersonalInformation.Firstname,
                    Lastname = user.PersonalInformation.Lastname,
                    Contact = user.PersonalInformation.Contact,
                    Birthdate = user.PersonalInformation.Birthdate,
                    Address = user.PersonalInformation.Address,
                    LicenseNumber = user.PersonalInformation.LicenseNumber,
                    Expiry = user.PersonalInformation.Expiry,
                },
                credential = new CredentialModel
                {
                    Username = user.Credential.Username,
                    Password = user.Credential.Password,
                    Email = user.Credential.Email,
                },
                wallet = new WalletModel
                {
                    Balance = user.Wallet.Balance,
                    Pincode = user.Wallet.Pincode,
                },
                accountStatus = new AccountStatusModel
                {
                    Role = user.AccountStatus.Role,
                    Shop = user.AccountStatus.Role == "MECHANIC" ? new ShopModel
                    {
                        ShopID = user.AccountStatus.getShop().ShopID,
                        ShopDescription = user.AccountStatus.getShop().ShopDescription,
                        ShopName = user.AccountStatus.getShop().ShopName
                    } : null
                }
            };
        }
    }
}

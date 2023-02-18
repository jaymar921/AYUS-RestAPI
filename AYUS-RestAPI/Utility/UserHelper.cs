using AYUS_RestAPI.Entity.Metadata;

namespace AYUS_RestAPI.Utility
{
    public static class UserHelper
    {
        public static User CreateUser(this User user, PersonalInformation personal, Credential credential, Wallet wallet, AccountStatus account, List<Vehicle> vehicles)
        {
            string uuid = personal.UUID;
            credential.UUID = uuid;
            wallet.UUID = uuid;
            account.UUID = uuid;

            return new User(personal, credential, wallet, account, vehicles);
        }
    }
}

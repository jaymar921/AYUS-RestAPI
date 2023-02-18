namespace AYUS_RestAPI.Entity.Metadata
{
    public class User
    {
        public PersonalInformation PersonalInformation { get; set; }
        public Credential Credential { get; set; }
        public Wallet Wallet { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public User(PersonalInformation personalInformation, Credential credential, Wallet wallet, AccountStatus accountStatus, List<Vehicle> vehicles)
        {
            PersonalInformation = personalInformation;
            Credential = credential;
            Wallet = wallet;
            AccountStatus = accountStatus;
            Vehicles = vehicles;
        }


    }
}

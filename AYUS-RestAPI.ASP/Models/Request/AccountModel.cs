using AYUS_RestAPI.ASP.Models.Request.MetaData;
using AYUS_RestAPI.Entity.Metadata;

namespace AYUS_RestAPI.ASP.Models.Request
{
    public class AccountModel
    {
        public PersonalInformationModel personalInformation { get; set; } = new PersonalInformationModel();
        public CredentialModel credential { get; set; } = new CredentialModel();
        public WalletModel wallet { get; set; } = new WalletModel();
        public AccountStatusModel accountStatus { get; set; } = new AccountStatusModel();
    }
}

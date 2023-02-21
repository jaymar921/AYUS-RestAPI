using AYUS_RestAPI.ASP.Models.Request.MetaData;

namespace AYUS_RestAPI.ASP.Models.Request
{
    public class MechanicPartial
    {
        public PersonalInformationModel personalInformation { get; set; } = new PersonalInformationModel();
        public AccountStatusModel accountStatus { get; set; } = new AccountStatusModel();
    }
}

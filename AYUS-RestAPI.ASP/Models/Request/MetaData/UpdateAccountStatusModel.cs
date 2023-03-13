namespace AYUS_RestAPI.ASP.Models.Request.MetaData
{
    public class UpdateAccountStatusModel
    {
        public string UUID { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public bool IsOnline { get; set; } = false;
    }
}

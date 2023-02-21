using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Data
{
    public class Session
    {
        [Key]
        public string SessionID { get; set; } = Guid.NewGuid().ToString();
        public string ClientUUID { get; set; } = string.Empty;
        public string MechanicUUID { get; set; } = string.Empty;
        public string TransactionID { get; set; } = string.Empty;
        public DateTime TimeStart { get; set; } = DateTime.UtcNow;
        public DateTime? TimeEnd { get; set; } = null;
        public bool isActive { get; set; } = true;
    }
}

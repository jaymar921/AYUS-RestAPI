using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.Data
{
    public class Transaction
    {

        [Key]
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateOfTransaction { get; set; } = DateTime.UtcNow;
        public double ServicePrice { get; set; } = 0;
        public string ServiceName { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace AYUS_RestAPI.ASP.Models.Request
{
    public class TransactionModel
    {
        [Required]
        public double ServicePrice { get; set; }
        [Required]
        public string ServiceName { get; set; } = string.Empty;
        [Required]
        public string Remark { get; set; } = string.Empty; 
    }
}

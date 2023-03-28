namespace AYUS_RestAPI.ASP.Models.Request
{
    public class ReportModel
    {
        private int Id { get; set; } = new Random().Next();
        public string Complainer { get; set; } = string.Empty;
        public string Complainee { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        private string Status { set; get; } = "Pending";

        public int getID() { return Id; }
        public string GetStatus() { return Status; }
        public void SetStatus(string status) { Status = status; }
    }
}

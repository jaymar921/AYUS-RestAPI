namespace AYUS_RestAPI.Data
{
    public class ServiceRequest
    {
        public string RequestID { get; set; } = Guid.NewGuid().ToString();
        public string Requestor { get; set; } = string.Empty;
        public string Recepient { get; set; } = string.Empty;

        public string Contact { get; set; } = string.Empty;
        public string Location { get; set;} = string.Empty; 
        public string Vehicle { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public object? Picture { get; set; }

    }
}

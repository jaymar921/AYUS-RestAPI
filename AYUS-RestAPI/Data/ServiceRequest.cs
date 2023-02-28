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
        public string Status { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;

        public static ServiceRequest parse(ServiceRequestModel model)
        {
            return new ServiceRequest
            {
                Requestor = model.Requestor,
                Recepient = model.Recepient,
                Contact = model.Contact,
                Location = model.Location,
                Vehicle = model.Vehicle,
                Service = model.Service,
                Description = model.Description,
                Picture = model.Picture,
                Status = model.Status,
                NewStatus = model.NewStatus,
            };
        }

    }

    public class ServiceRequestModel
    {
        public string Requestor { get; set; } = string.Empty;
        public string Recepient { get; set; } = string.Empty;

        public string Contact { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Vehicle { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public object? Picture { get; set; } = null;
        public string Status { get; set; } = "Requesting"; 
        public string NewStatus { get; set; } = "Requesting";



    }
}

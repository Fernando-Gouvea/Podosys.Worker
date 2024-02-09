namespace Podosys.Worker.Domain.Models.Reports
{
    public class AddressReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public DateTime UpdateDate { get; set; }

        public string? Street { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }
    }
}

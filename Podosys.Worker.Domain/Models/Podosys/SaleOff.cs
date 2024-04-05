namespace Podosys.Worker.Domain.Models.Podosys
{
    public class SaleOff
    {
        public Guid Id { get; set; }

        public string? BuyerName { get; set; }

        public int NumberOfSection { get; set; }

        public string? PaymentType { get; set; }

        public DateTime Date { get; set; }
    }
}

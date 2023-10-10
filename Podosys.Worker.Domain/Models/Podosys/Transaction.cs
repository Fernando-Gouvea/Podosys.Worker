namespace Podosys.Worker.Domain.Models.Podosys
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? SaleOffId { get; set; }
        public Guid? CashFlowId { get; set; }
        public Guid? MedicalRecordId { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? PaymentTypeId { get; set; }
    }
}
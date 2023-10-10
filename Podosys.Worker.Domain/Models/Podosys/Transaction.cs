using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Podosys
{
    [Table("Transaction_tb")]
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? SaleOffId { get; set; }
        public Guid? CashFlowId { get; set; }
        public Guid? MedicalRecordId { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Movimento")]
        public string FlowType { get; set; }
        public int? TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; }

        [Display(Name = "Pagamento em:")]
        public string PaymentType { get; set; }
        public int? PaymentTypeId { get; set; }
        public PaymentType Payment_Type { get; set; }

        [Display(Name = "Valor")]
        [Column(TypeName = "Decimal(7,2)")]
        public decimal Value { get; set; }
    }
}
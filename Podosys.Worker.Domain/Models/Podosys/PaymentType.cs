using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Podosys
{
    [Table("PaymentType_tb")]
    public class PaymentType
    {
        public int Id { get; set; }

        [Display(Name = "Pagamento em:")]
        [Column("Type")]
        public string Type { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Podosys
{
    [Table("TransactionType_tb")]
    public class TransactionType
    {
        public int Id { get; set; }

        [Column("Type")]
        public string Type { get; set; }
    }
}

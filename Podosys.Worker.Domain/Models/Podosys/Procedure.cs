using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Podosys
{
    [Table("Procedure_tb")]
    public class Procedure
    {
        public int Id { get; set; }

        [Display(Name = "Procedimento")]
        public string ProcedureType { get; set; }

        public Guid MedicalRecordId { get; set; }
    }
}
namespace Podosys.Worker.Domain.Models.Podosys
{
    public class Procedure
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Observation { get; set; }
        public int? GroupId { get; set; }
    }
}
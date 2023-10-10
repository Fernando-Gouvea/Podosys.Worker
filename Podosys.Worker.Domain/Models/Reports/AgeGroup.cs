namespace Podosys.Worker.Domain.Models.Reports
{
    public class AgeGroup
    {
        public DateTime Date { get; set; }
        public int Baby { get; set; }
        public int Child { get; set; }
        public int Teenager { get; set; }
        public int Young { get; set; }
        public int Adult { get; set; }
        public int Elderly { get; set; }
    }
}

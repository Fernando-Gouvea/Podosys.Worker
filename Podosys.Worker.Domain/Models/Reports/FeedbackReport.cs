namespace Podosys.Worker.Domain.Models.Podosys
{
    public class FeedbackReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? User { get; set; }
        public string? Criticism { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
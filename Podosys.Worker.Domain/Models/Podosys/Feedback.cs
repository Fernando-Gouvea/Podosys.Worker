namespace Podosys.Worker.Domain.Models.Podosys
{
    public class Feedback
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public Professional? User { get; set; }
        public int CriticismId { get; set; }
        public Criticism? Criticism { get; set; }
        public DateTime Date { get; set; }
    }

    public class Criticism
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
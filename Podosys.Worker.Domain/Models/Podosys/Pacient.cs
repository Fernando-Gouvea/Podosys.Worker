namespace Podosys.Worker.Domain.Models.Podosys
{
    public class Pacient
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public int Age { get; set; }

        public string? PrimaryPhone { get; set; }

        public string? SecondaryPhone { get; set; }

        public string? Allergies { get; set; }

        public string? Sport { get; set; }

        public string? Standing { get; set; }

        public string? Medicine { get; set; }

        public string? Surgery { get; set; }

        public string? Shoe { get; set; }

        public string? Occupation { get; set; }

        public string? HowMeeted { get; set; }

        public string? Observation { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool Enabler { get; set; }
    }
}

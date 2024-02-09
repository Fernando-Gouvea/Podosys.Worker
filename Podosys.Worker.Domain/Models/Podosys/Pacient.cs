namespace Podosys.Worker.Domain.Models.Podosys
{
    public class Pacient
    {
        public Guid Id { get; set; }

        public Guid? AddressId { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        private int _age;

        public int Age
        {
            get { return DateTime.Now.Year - BirthDate.Year; }
        }

        public string? PrimaryPhone { get; set; }

        public string? SecondaryPhone { get; set; }

        public string? Allergies { get; set; }

        public string? Sport { get; set; }

        public string? Standing { get; set; }

        public string? Medicine { get; set; }

        public string? Surgery { get; set; }

        public string? Shoe { get; set; }

        public string? Occupation { get; set; }

        public int? CommunicationChannelId { get; set; }

        public string? Observation { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool Enabler { get; set; }
    }
}

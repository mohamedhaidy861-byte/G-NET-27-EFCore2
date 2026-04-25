namespace ConsoleApp.Models
{

    public class Attendee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

        
        public Address HomeAddress { get; set; } = null!;

      
        public virtual Badge? Badge { get; set; }

      
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}

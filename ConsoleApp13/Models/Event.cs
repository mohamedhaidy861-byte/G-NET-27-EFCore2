namespace ConsoleApp.Models
{
   
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxAttendees { get; set; }

     
        public int? ParentEventId { get; set; }
        public virtual Event? ParentEvent { get; set; }
        public virtual ICollection<Event> Sessions { get; set; } = new List<Event>();

        // Organizer
        public int OrganizerId { get; set; }
        public virtual Organizer Organizer { get; set; } = null!;

        
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        
    }
}

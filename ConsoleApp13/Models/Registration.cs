namespace ConsoleApp.Models
{
    // ===================================================
    // Configured Using: Fluent API (inline in OnModelCreating)
    // Junction table between Attendee and Event
    // Composite key: (AttendeeId, EventId)
    // ===================================================
    public class Registration
    {
        public int AttendeeId { get; set; }
        public int EventId { get; set; }

        public string? Note { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Navigation Properties
        public virtual Attendee Attendee { get; set; } = null!;
        public virtual Event Event { get; set; } = null!;
    }
}

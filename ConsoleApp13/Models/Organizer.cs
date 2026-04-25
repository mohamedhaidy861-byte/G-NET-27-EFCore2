using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp.Models
{
    // ===================================================
    // Configured Using: Data Annotations
    // ===================================================
    [Table("Organizers")]
    public class Organizer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(200)]
        public string? CompanyName { get; set; }

        public bool IsVerified { get; set; }

        // Navigation Properties
        public virtual OrganizerProfile? Profile { get; set; }
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    }
}

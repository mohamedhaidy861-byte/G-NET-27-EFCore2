using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp.Models
{
    // ===================================================
    // Configured Using: Data Annotations
    // Profile page is tied to organizer and cannot exist without it
    // ===================================================
    [Table("OrganizerProfiles")]
    public class OrganizerProfile
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }

        [MaxLength(300)]
        public string? WebsiteUrl { get; set; }

        [MaxLength(300)]
        public string? LogoUrl { get; set; }

        // Foreign Key — required (profile cannot exist without organizer)
        [Required]
        public int OrganizerId { get; set; }

        // Navigation Property
        [ForeignKey(nameof(OrganizerId))]
        public virtual Organizer Organizer { get; set; } = null!;
    }
}

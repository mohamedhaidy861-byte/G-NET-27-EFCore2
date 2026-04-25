using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp.Models
{
    
    [Table("Badges")]
    public class Badge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string BadgeNumber { get; set; } = null!;

        [Required]
        public DateTime IssuedDate { get; set; }

        [Required]
        public BadgeTier Tier { get; set; }

        
        [Required]
        public int AttendeeId { get; set; }

        
        [ForeignKey(nameof(AttendeeId))]
        public virtual Attendee Attendee { get; set; } = null!;
    }
}

using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Configurations
{
    
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Description)
                   .IsRequired();

            builder.Property(e => e.StartDate)
                   .IsRequired();

            builder.Property(e => e.MaxAttendees)
                   .IsRequired();

            
            builder.Property<DateTime>("CreatedAt");
            builder.Property<DateTime>("LastModifiedAt");

          
            builder.HasOne(e => e.ParentEvent)
                   .WithMany(e => e.Sessions)
                   .HasForeignKey(e => e.ParentEventId)
                   .OnDelete(DeleteBehavior.NoAction); 
            builder.HasOne(e => e.Organizer)
                   .WithMany(o => o.Events)
                   .HasForeignKey(e => e.OrganizerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

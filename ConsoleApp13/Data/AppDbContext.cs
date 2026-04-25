using ConsoleApp.Configurations;
using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=EventHub;Trusted_Connection=True;TrustServerCertificate=True;");

            optionsBuilder.UseLazyLoadingProxies();

            
        }

        // DbSets 
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<OrganizerProfile> OrganizerProfiles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── Apply Separate Configuration Class (Event)
            modelBuilder.ApplyConfiguration(new EventConfiguration());

            // ── Fluent API: Attendee
            modelBuilder.Entity<Attendee>(entity =>
            {
                entity.ToTable("Attendees");

                entity.HasKey(a => a.Id);

                entity.Property(a => a.FullName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(a => a.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                // Owned Entity
                entity.OwnsOne(a => a.HomeAddress, address =>
                {
                    address.Property(x => x.Street)
                           .IsRequired()
                           .HasMaxLength(300)
                           .HasColumnName("Street");

                    address.Property(x => x.City)
                           .IsRequired()
                           .HasMaxLength(100)
                           .HasColumnName("City");

                    address.Property(x => x.Country)
                           .IsRequired()
                           .HasMaxLength(100)
                           .HasColumnName("Country");

                    address.Property(x => x.PostalCode)
                           .IsRequired()
                           .HasMaxLength(20)
                           .HasColumnName("PostalCode");
                });

                // One-to-one with Badge 
                entity.HasOne(a => a.Badge)
                      .WithOne(b => b.Attendee)
                      .HasForeignKey<Badge>(b => b.AttendeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Fluent API: Registration (junction table)
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.ToTable("Registrations");

                // Composite primary key
                entity.HasKey(r => new { r.AttendeeId, r.EventId });

                entity.Property(r => r.Note)
                      .HasMaxLength(500);

                entity.Property(r => r.RegistrationDate)
                      .IsRequired();

                entity.HasOne(r => r.Attendee)
                      .WithMany(a => a.Registrations)
                      .HasForeignKey(r => r.AttendeeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Event)
                      .WithMany(e => e.Registrations)
                      .HasForeignKey(r => r.EventId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

           
            modelBuilder.Entity<Organizer>()
                        .HasOne(o => o.Profile)
                        .WithOne(p => p.Organizer)
                        .HasForeignKey<OrganizerProfile>(p => p.OrganizerId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

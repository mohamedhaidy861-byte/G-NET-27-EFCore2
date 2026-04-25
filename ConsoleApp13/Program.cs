using ConsoleApp.Data;
using ConsoleApp.Models;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using AppDbContext db = new();


            if (!db.Organizers.Any())
            {

                var organizer = new Organizer
                {
                    Name = "Ahmed Hassan",
                    CompanyName = "TechConf Egypt",
                    IsVerified = true,
                    Profile = new OrganizerProfile
                    {
                        Bio = "Passionate about bringing tech communities together.",
                        WebsiteUrl = "https://techconf.eg",
                        LogoUrl = "https://techconf.eg/logo.png"
                    }
                };

                db.Organizers.Add(organizer);
                db.SaveChanges();
                Console.WriteLine($"Organizer created — Id: {organizer.Id}");

                // Create a main Event with two Sessions 
                var mainEvent = new Event
                {
                    Title = "Egypt Tech Summit 2025",
                    Description = "The largest annual technology conference in Egypt.",
                    StartDate = new DateTime(2025, 10, 1),
                    EndDate = new DateTime(2025, 10, 3),
                    MaxAttendees = 2000,
                    OrganizerId = organizer.Id
                };

                db.Events.Add(mainEvent);
                db.SaveChanges();

                // Set shadow properties on the event
                db.Entry(mainEvent).Property("CreatedAt").CurrentValue = DateTime.Now;
                db.Entry(mainEvent).Property("LastModifiedAt").CurrentValue = DateTime.Now;

                var session1 = new Event
                {
                    Title = "AI & Machine Learning Workshop",
                    Description = "Hands-on workshop covering modern ML techniques.",
                    StartDate = new DateTime(2025, 10, 1, 9, 0, 0),
                    MaxAttendees = 100,
                    OrganizerId = organizer.Id,
                    ParentEventId = mainEvent.Id
                };

                var session2 = new Event
                {
                    Title = "Cloud Architecture Breakout",
                    Description = "Deep dive into cloud-native patterns.",
                    StartDate = new DateTime(2025, 10, 1, 14, 0, 0),
                    MaxAttendees = 80,
                    OrganizerId = organizer.Id,
                    ParentEventId = mainEvent.Id
                };

                db.Events.AddRange(session1, session2);

                
                var attendee = new Attendee
                {
                    FullName = "Sara Khaled",
                    Email = "sara.khaled@example.com",
                    HomeAddress = new Address
                    {
                        Street = "15 Tahrir Square",
                        City = "Cairo",
                        Country = "Egypt",
                        PostalCode = "11511"
                    }
                };

                db.Attendees.Add(attendee);
                db.SaveChanges();

                
                var badge = new Badge
                {
                    BadgeNumber = "BADGE-0001",
                    IssuedDate = DateTime.Today,
                    Tier = BadgeTier.VIP,
                    AttendeeId = attendee.Id
                };

                db.Badges.Add(badge);

                // Fluent API junction
                var registration = new Registration
                {
                    AttendeeId = attendee.Id,
                    EventId = mainEvent.Id,
                    Note = "Looking forward to the keynote!",
                    RegistrationDate = DateTime.Now
                };

                db.Registrations.Add(registration);
                db.SaveChanges();

                Console.WriteLine("Data seeded successfully.");
            }

           
            Console.WriteLine("\n===== EventHub Data =====");

            var events = db.Events.ToList();
            foreach (var ev in events)
            {
                var parentLabel = ev.ParentEventId.HasValue ? $"(Session of Event #{ev.ParentEventId})" : "(Main Event)";
                Console.WriteLine($"\nEvent: {ev.Title} {parentLabel}");
                Console.WriteLine($"  Start     : {ev.StartDate:d}");
                Console.WriteLine($"  MaxAttend : {ev.MaxAttendees}");

                // Read shadow properties
                var createdAt = db.Entry(ev).Property<DateTime>("CreatedAt").CurrentValue;
                var lastModifiedAt = db.Entry(ev).Property<DateTime>("LastModifiedAt").CurrentValue;
                Console.WriteLine($"  CreatedAt (shadow)      : {createdAt}");
                Console.WriteLine($"  LastModifiedAt (shadow) : {lastModifiedAt}");
            }

            var attendees = db.Attendees.ToList();
            foreach (var a in attendees)
            {
                Console.WriteLine($"\nAttendee : {a.FullName}  ({a.Email})");
                Console.WriteLine($"  Address : {a.HomeAddress.Street}, {a.HomeAddress.City}, {a.HomeAddress.Country} {a.HomeAddress.PostalCode}");

                // Lazy-loaded navigation
                if (a.Badge != null)
                    Console.WriteLine($"  Badge   : #{a.Badge.BadgeNumber}  Tier={a.Badge.Tier}  Issued={a.Badge.IssuedDate:d}");

                foreach (var reg in a.Registrations)
                    Console.WriteLine($"  Registered to Event #{reg.EventId}  Note: {reg.Note ?? "-"}");
            }
        }
    }
}




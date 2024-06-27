using Microsoft.EntityFrameworkCore;
using WebAppSimplifica2.Entities;

namespace WebAppSimplifica2.Persistence
{
    public class DevEventsDbContext:DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {
            
        }

        public DbSet<DevEvent> DevEvents { get; set; }
        public DbSet<DevEventSpeaker> DevEventsSpeakers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevEvent>(e =>
            {
                e.HasKey(de => de.Id);

                //PERMITE VALORES NULOS
                e.Property(de => de.Title).IsRequired(false);

                e.Property(de => de.Description)
                    .HasMaxLength(200)
                    .HasColumnType("varchar(200)");

                e.Property(de => de.StartDate)
                    .HasColumnName("Start_Date");

                e.Property(de => de.EndDate)
                    .HasColumnName("End_Date");

                e.HasMany(de => de.Speakers)
                    .WithOne()
                    .HasForeignKey(s => s.DevEventId);

            });

            builder.Entity<DevEventSpeaker>(e =>
            {
                e.HasKey(de => de.Id);


            });


        }

    }
}

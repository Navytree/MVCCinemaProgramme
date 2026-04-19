using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MVCCinemaProgramme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCinemaProgramme.Data
{
    public class MVCCinemaProgrammeContext : DbContext
    {
        public MVCCinemaProgrammeContext (DbContextOptions<MVCCinemaProgrammeContext> options)
            : base(options)
        {
        }

        public DbSet<MVCCinemaProgramme.Models.Movie> Movie { get; set; } = default!;
        public DbSet<MVCCinemaProgramme.Models.Programme> Programme { get; set; } = default!;
        public DbSet<MVCCinemaProgramme.Models.Hall> Hall { get; set; } = default!;
        public DbSet<MVCCinemaProgramme.Models.Seat> Seat { get; set; } = default!;
        public DbSet<MVCCinemaProgramme.Models.Ticket> Ticket { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Hall>()
                .HasMany(e => e.Seats)
                .WithOne(e => e.Hall)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany()
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Programme)
                .WithMany()
                .HasForeignKey(t => t.ProgrammeId)
                .OnDelete(DeleteBehavior.NoAction);

        }




    }
}

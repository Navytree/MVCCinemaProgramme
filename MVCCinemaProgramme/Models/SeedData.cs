using Microsoft.EntityFrameworkCore;
using MVCCinemaProgramme.Data;

namespace MVCCinemaProgramme.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MVCCinemaProgrammeContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MVCCinemaProgrammeContext>>()))
        {
            if (context.Movie.Any()) return;

            var movie1 = new Movie { Title = "Star Wars: A New Hope", Genre = "Sci-Fi", Rating = "PG", TicketPrice = 25.00M };
            var movie2 = new Movie { Title = "Monty Python: Holy Grail", Genre = "Comedy", Rating = "PG", TicketPrice = 0.00M };
            var movie3 = new Movie { Title = "Guardians of the Galaxy: vol1", Genre = "Comedy, Sci-Fi", Rating = "PG", TicketPrice = 20.00M };
            context.Movie.AddRange(movie1, movie2, movie3);
            context.SaveChanges();

            var hall1 = new Hall { Name = "1A" };
            var hall2 = new Hall { Name = "2A" };
            var hall3 = new Hall { Name = "3A" };             
            context.Hall.AddRange(hall1, hall2, hall3);
            context.SaveChanges();


            for (int i = 1; i <= 20; i++) {
                context.Seat.Add(new Seat { Number = i, HallId = hall1.Id });
                context.Seat.Add(new Seat { Number = i, HallId = hall2.Id });
                context.Seat.Add(new Seat { Number = i, HallId = hall3.Id });
            }
            context.SaveChanges();

            context.Programme.AddRange(
                new Programme
                {
                    MovieId = movie1.Id,
                    HallId = hall1.Id,
                    Begin = DateTime.Now.AddDays(5).Date.AddHours(18),
                    End = DateTime.Now.AddDays(5).Date.AddHours(20)
                },
                new Programme
                {
                    MovieId = movie2.Id,
                    HallId = hall2.Id,
                    Begin = DateTime.Now.AddDays(5).Date.AddHours(20),
                    End = DateTime.Now.AddDays(5).Date.AddHours(23)
                },
                new Programme
                {
                    MovieId = movie3.Id,
                    HallId = hall2.Id,
                    Begin = DateTime.Now.AddDays(4).Date.AddHours(12),
                    End = DateTime.Now.AddDays(4).Date.AddHours(15)
                }
            );
            context.SaveChanges();


        }
    }
}
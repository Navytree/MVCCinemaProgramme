using System.ComponentModel.DataAnnotations;

namespace MVCCinemaProgramme.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Genre { get; set; }
        public string? Rating { get; set; }

        [Display(Name = "Ticket Price")]
        [DataType(DataType.Currency)]
        public decimal TicketPrice { get; set; }

    }
}
